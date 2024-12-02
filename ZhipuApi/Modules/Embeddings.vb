Imports System.IO
Imports System.Net.Http
Imports System.Text
Imports System.Threading
Imports Nukepayload2.AI.Providers.Zhipu.Models
Imports Nukepayload2.AI.Providers.Zhipu.Utils

Public Class Embeddings
    Private Const API_TOKEN_TTL_SECONDS = 300
    Private Shared ReadOnly client As New HttpClient
    Private ReadOnly _apiKey As String

    Public Sub New(apiKey As String)
        _apiKey = apiKey
    End Sub

    Private Async Function ProcessRawAsync(requestBody As EmbeddingRequestBase, cancellation As CancellationToken) As Task(Of MemoryStream)
        Dim json As String = requestBody?.ToJson
        Dim data As New StringContent(json, Encoding.UTF8, "application/json")
        Dim api_key As String = AuthenticationUtils.GenerateToken(_apiKey, API_TOKEN_TTL_SECONDS)
        Dim request As New HttpRequestMessage With {
            .Method = HttpMethod.Post,
            .RequestUri = New Uri("https://open.bigmodel.cn/api/paas/v4/embeddings"),
            .Content = data
        }
        request.Headers.Add("Authorization", api_key)
        Dim response = Await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellation)
#If NET6_0_OR_GREATER Then
        Dim stream = Await response.Content.ReadAsStreamAsync(cancellation)
#Else
        Dim stream = Await response.Content.ReadAsStreamAsync()
#End If
        Dim result As New MemoryStream
        Await stream.CopyToAsync(result, 81920, cancellation)
        Return result
    End Function

    Public Async Function ProcessAsync(requestBody As EmbeddingRequestBase, Optional cancellation As CancellationToken = Nothing) As Task(Of EmbeddingResponseBase)
        Return EmbeddingResponseBase.FromJson(
            Await ProcessRawAsync(requestBody, cancellation))
    End Function

End Class
