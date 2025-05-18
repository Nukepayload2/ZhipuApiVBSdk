Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Threading
Imports Nukepayload2.AI.Providers.Zhipu.Models
Imports Nukepayload2.AI.Providers.Zhipu.Utils

Public Module ChatBatch

    <Extension>
    Public Async Function CreateChatBatchAsync(client As ClientV4,
                                               requests As IEnumerable(Of BatchChatItem),
                                               Optional cancellationToken As CancellationToken = Nothing) As Task(Of BatchStatus)
        If client Is Nothing Then
            Throw New ArgumentNullException(NameOf(client))
        End If

        If requests Is Nothing Then
            Throw New ArgumentNullException(NameOf(requests))
        End If

        Dim ms As New MemoryStream
        Using sw As New StreamWriter(ms, IoUtils.UTF8NoBOM, 8192, True)
            Dim countWritten = 0
            For Each req In requests
                req.WriteTo(sw)
                sw.Write(vbLf)
                cancellationToken.ThrowIfCancellationRequested()
            Next
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
            .Endpoint = "/v4/chat/completions",
            .AutoDeleteInputFile = True
        }

        Dim batchStatus = Await client.Batches.CreateAsync(batchRequest, cancellationToken)
        Return batchStatus
    End Function

End Module
