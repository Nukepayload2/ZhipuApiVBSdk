Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Threading
Imports Nukepayload2.AI.Providers.Zhipu.Models
Imports Nukepayload2.AI.Providers.Zhipu.Utils

Public Module ChatBatch

    ''' <summary>
    ''' 创建一个批量对话任务
    ''' </summary>
    ''' <param name="client">客户端</param>
    ''' <param name="requests">每个对话任务。其中，<see cref="BatchChatRequestItem.Method"/> 和
    ''' <see cref="BatchChatRequestItem.Url"/> 如果是空，则会被强制填充为唯一的有效值。如果要确保线程安全，则务必提前填写这些属性。</param>
    ''' <param name="cancellationToken">用于取消任务</param>
    ''' <returns>批量任务的状态</returns>
    <Extension>
    Public Async Function CreateChatBatchAsync(client As ClientV4,
                                               requests As IEnumerable(Of BatchChatRequestItem),
                                               Optional cancellationToken As CancellationToken = Nothing) As Task(Of BatchStatus)
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
        Return batchStatus
    End Function

End Module
