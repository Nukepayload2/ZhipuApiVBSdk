Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Threading
Imports Nukepayload2.AI.Providers.Zhipu.Models
Imports Nukepayload2.AI.Providers.Zhipu.Utils

Public Module ChatBatchFactory

    ''' <summary>
    ''' 创建一个批量对话任务
    ''' </summary>
    ''' <param name="client">客户端</param>
    ''' <param name="requests">每个对话任务。其中，<see cref="BatchChatRequestItem.Method"/> 和
    ''' <see cref="BatchChatRequestItem.Url"/> 如果是空，则会被强制填充为唯一的有效值。如果要确保线程安全，则务必提前填写这些属性。</param>
    ''' <param name="cancellationToken">用于取消任务</param>
    ''' <returns>创建的批量任务</returns>
    <Extension>
    Public Async Function CreateChatBatchAsync(client As ClientV4,
                                               requests As IEnumerable(Of BatchChatRequestItem),
                                               Optional cancellationToken As CancellationToken = Nothing) As Task(Of ChatBatch)
        If client Is Nothing Then
            Throw New ArgumentNullException(NameOf(client))
        End If

        If requests Is Nothing Then
            Throw New ArgumentNullException(NameOf(requests))
        End If

        Dim endpoint = "/v4/chat/completions"
        Dim post = "POST"
        Dim ms As New MemoryStream
        Using sw As New StreamWriter(ms, IoUtils.UTF8NoBOM, 8192, True)
            Dim countWritten = 0
            For Each req In requests
                If req Is Nothing Then
                    Throw New ArgumentException("Collection contains null or Nothing.", NameOf(requests))
                End If
                If req.CustomId = Nothing Then
                    Throw New ArgumentException("CustomId is required.", NameOf(requests))
                End If
                If req.Body Is Nothing Then
                    Throw New ArgumentException("Body is required.", NameOf(requests))
                End If
                If req.Url = Nothing Then req.Url = endpoint
                If req.Method = Nothing Then req.Method = post
                req.WriteTo(sw)
                sw.Write(vbLf)
                countWritten += 1
                cancellationToken.ThrowIfCancellationRequested()
            Next
            If countWritten = 0 Then
                Throw New ArgumentException("Collection is empty.", NameOf(requests))
            End If
        End Using
        ms.Position = 0
        Dim uploadRequest As New FileUploadRequest With {
            .FileContent = ms,
            .FileName = $"{Guid.NewGuid:N}.jsonl",
            .Purpose = "batch"
        }
        Dim batchFileItem = Await client.Files.UploadAsync(uploadRequest, cancellationToken)
        Dim batchRequest As New BatchCreateRequest With {
            .InputFileId = batchFileItem.Id,
            .Endpoint = endpoint,
            .AutoDeleteInputFile = True
        }

        Dim batchStatus = Await client.Batches.CreateAsync(batchRequest, cancellationToken)
        Return New ChatBatch(client, batchStatus)
    End Function

End Module

Public Class ChatBatch
    Private ReadOnly _client As ClientV4
    Private ReadOnly _batchStatus As BatchStatus

    Sub New(client As ClientV4, batchStatus As BatchStatus)
        _client = client
        _batchStatus = batchStatus
    End Sub

    ''' <summary>
    ''' 任务状态
    ''' </summary>
    Public ReadOnly Property Status As TaskStatus
        Get
            Return Volatile.Read(_batchStatus).TaskStatus
        End Get
    End Property

    ''' <summary>
    ''' 用于在释放此实例后，稍后再获取 BatchStatus
    ''' </summary>
    Public ReadOnly Property Id As String
        Get
            Return Volatile.Read(_batchStatus).Id
        End Get
    End Property

    ''' <summary>
    ''' 任务是否已经结束执行，包括 <see cref="TaskStatus.Completed"/>, <see cref="TaskStatus.Cancelled"/>, <see cref="TaskStatus.Failed"/>, <see cref="TaskStatus.Expired"/>
    ''' </summary>
    Public ReadOnly Property IsCompleted As Boolean
        Get
            Select Case Volatile.Read(_batchStatus).TaskStatus
                Case TaskStatus.Completed, TaskStatus.Expired, TaskStatus.Failed, TaskStatus.Cancelled
                    Return True
                Case Else
                    Return False
            End Select
        End Get
    End Property

    Public Async Function UpdateStatusAsync(Optional cancellationToken As CancellationToken = Nothing) As Task
        Dim result = Await _client.Batches.GetStatusAsync(_batchStatus.Id, cancellationToken)
        Volatile.Write(_batchStatus, result)
    End Function

    Public Async Function GetResultAsync(Optional cancellationToken As CancellationToken = Nothing) As Task(Of IEnumerable(Of BatchChatResponseItem))
        Dim status = _batchStatus
        If status.TaskStatus <> TaskStatus.Completed Then
            Throw New InvalidOperationException("Batch task is not completed.")
        End If
        Dim content = Await _client.Files.DownloadAsync(status.OutputFileId, cancellationToken)
        Return BatchChatResponseItem.FromJsonLines(content)
    End Function
End Class
