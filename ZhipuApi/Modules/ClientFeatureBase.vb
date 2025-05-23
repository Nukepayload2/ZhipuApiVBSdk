﻿Imports System.IO
Imports System.Net.Http
Imports System.Threading
Imports Nukepayload2.AI.Providers.Zhipu.Utils

Public MustInherit Class ClientFeatureBase

    Private Const ApiTokenTtlSeconds = 300
    Private ReadOnly _apiKey As String
    Private ReadOnly _client As HttpClient

    Sub New(apiKey As String)
        MyClass.New(apiKey, ClientV4.DefaultHttpClient)
    End Sub

    Sub New(apiKey As String, client As HttpClient)
        _apiKey = apiKey
        _client = client
    End Sub

    Protected Async Function PostAsync(requestUrl As String, json As Stream, cancellation As CancellationToken) As Task(Of MemoryStream)
        Dim data As New StreamContent(json)
        data.Headers.ContentType = New Headers.MediaTypeHeaderValue("application/json")
        Dim response As HttpResponseMessage = Await PostRawAsync(requestUrl, data, cancellation)
        Return Await ReadAndCheckErrorAsync(response, cancellation)
    End Function

    Protected Async Function ReadAndCheckErrorAsync(response As HttpResponseMessage, cancellation As CancellationToken) As Task(Of MemoryStream)
        Dim result = Await IoUtils.CopyToMemoryStreamAsync(response, cancellation)
        ErrorHandler.ThrowForNonSuccess(response, result)
        Return result
    End Function

    Protected Async Function PostRawAsync(requestUrl As String, data As HttpContent, cancellation As CancellationToken) As Task(Of HttpResponseMessage)
        Dim apiKey = AuthenticationUtils.GenerateToken(_apiKey, ApiTokenTtlSeconds)
        Dim request As New HttpRequestMessage With {
            .Method = HttpMethod.Post,
            .RequestUri = New Uri(requestUrl),
            .Content = data
        }
        request.Headers.Add("Authorization", apiKey)
        Dim response = Await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellation)
        Return response
    End Function

    Protected Async Function GetAsync(requestUrl As String, cancellation As CancellationToken, Optional accept As String = "application/json") As Task(Of MemoryStream)
        Dim request As New HttpRequestMessage With {
            .Method = HttpMethod.Get,
            .RequestUri = New Uri(requestUrl)
        }
        Dim apiKey = AuthenticationUtils.GenerateToken(_apiKey, ApiTokenTtlSeconds)
        With request.Headers
            .Accept.TryParseAdd(accept)
            .Add("Authorization", apiKey)
        End With
        Dim response = Await _client.SendAsync(request, cancellation)
        Return Await ReadAndCheckErrorAsync(response, cancellation)
    End Function

    Protected Async Function DeleteAsync(requestUrl As String, cancellation As CancellationToken) As Task(Of MemoryStream)
        Dim request As New HttpRequestMessage(HttpMethod.Delete, New Uri(requestUrl))
        Dim apiKey = AuthenticationUtils.GenerateToken(_apiKey, ApiTokenTtlSeconds)
        With request.Headers
            .Accept.TryParseAdd("application/json")
            .Add("Authorization", apiKey)
        End With
        Dim response = Await _client.SendAsync(request, cancellation)
        Return Await ReadAndCheckErrorAsync(response, cancellation)
    End Function
End Class
