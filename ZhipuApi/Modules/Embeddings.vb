Imports System.IO
Imports System.Net.Http
Imports System.Threading
Imports Nukepayload2.AI.Providers.Zhipu.Models

Public Class Embeddings
    Inherits ClientFeatureBase

    Public Sub New(apiKey As String)
        MyBase.New(apiKey)
    End Sub

    Sub New(apiKey As String, client As HttpClient)
        MyBase.New(apiKey, client)
    End Sub

    Private Async Function ProcessRawAsync(requestBody As EmbeddingRequestBase, cancellation As CancellationToken) As Task(Of MemoryStream)
        Dim json = requestBody?.ToJsonUtf8
        Const requestUrl = "https://open.bigmodel.cn/api/paas/v4/embeddings"
        Return Await PostAsync(requestUrl, json, cancellation)
    End Function

    Public Async Function ProcessAsync(requestBody As EmbeddingRequestBase, Optional cancellation As CancellationToken = Nothing) As Task(Of EmbeddingResponseBase)
        Return EmbeddingResponseBase.FromJson(
            Await ProcessRawAsync(requestBody, cancellation))
    End Function

End Class
