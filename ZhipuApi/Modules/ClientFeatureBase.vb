﻿Imports System.IO
Imports System.Net.Http
Imports System.Threading
Imports Nukepayload2.AI.Providers.Zhipu.Utils

Public MustInherit Class ClientFeatureBase

    Private Const ApiTokenTtlSeconds = 300
    Private Shared ReadOnly s_client As New HttpClient
    Private ReadOnly _apiKey As String

    Sub New(apiKey As String)
        _apiKey = apiKey
    End Sub

    Protected Async Function PostAsync(requestUrl As String, json As Stream, cancellation As CancellationToken) As Task(Of MemoryStream)
        Dim response As HttpResponseMessage = Await PostRawAsync(requestUrl, json, cancellation)
#If NET6_0_OR_GREATER Then
        Dim stream = Await response.Content.ReadAsStreamAsync(cancellation)
#Else
        Dim stream = Await response.Content.ReadAsStreamAsync()
#End If
        Dim result As New MemoryStream
        Await stream.CopyToAsync(result, 81920, cancellation)
        result.Position = 0
        Return result
    End Function

    Protected Async Function PostRawAsync(requestUrl As String, json As Stream, cancellation As CancellationToken) As Task(Of HttpResponseMessage)
        Dim data As New StreamContent(json)
        data.Headers.ContentType = New Headers.MediaTypeHeaderValue("application/json")
        Dim apiKey = AuthenticationUtils.GenerateToken(_apiKey, ApiTokenTtlSeconds)
        Dim request As New HttpRequestMessage With {
            .Method = HttpMethod.Post,
            .RequestUri = New Uri(requestUrl),
            .Content = data
        }
        request.Headers.Add("Authorization", apiKey)
        Dim response = Await s_client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellation)
        Return response
    End Function
End Class