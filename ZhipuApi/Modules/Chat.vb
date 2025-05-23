﻿Imports System.IO
Imports System.Net.Http
Imports System.Threading
Imports Nukepayload2.AI.Providers.Zhipu.Models
Imports Nukepayload2.AI.Providers.Zhipu.Utils

Public Class Chat
    Inherits ClientFeatureBase

    Public Sub New(apiKey As String)
        MyBase.New(apiKey)
    End Sub

    Sub New(apiKey As String, client As HttpClient)
        MyBase.New(apiKey, client)
    End Sub

    Protected Overridable ReadOnly Property RequestUrl As String = "https://open.bigmodel.cn/api/paas/v4/chat/completions"

    Private Async Function CompleteRawAsync(textRequestBody As TextRequestBase, cancellation As CancellationToken) As Task(Of MemoryStream)
        Dim json = textRequestBody?.ToJsonUtf8
#If DEBUG Then
        Debug.WriteLine("Sending chat request: ")
        Debug.WriteLine(IoUtils.UTF8NoBOM.GetString(json.ToArray()))
#End If
        Return Await PostAsync(RequestUrl, json, cancellation)
    End Function

    ''' <summary>
    ''' Chat completion without streaming.
    ''' </summary>
    ''' <param name="textRequestBody">The request message.</param>
    ''' <param name="cancellationToken">Use it to cancel the request.</param>
    ''' <returns>The response message or error message.</returns>
    ''' <exception cref="HttpRequestException">
    ''' The request was not successful and the response stream is empty.
    ''' </exception>
    ''' <remarks>
    ''' <see href="https://bigmodel.cn/dev/api/normal-model/glm-4">Online documentation</see>
    ''' </remarks>
    Public Async Function CompleteAsync(textRequestBody As TextRequestBase,
                                        Optional cancellationToken As CancellationToken = Nothing) As Task(Of ResponseBase)
        If textRequestBody.Stream Then Throw New ArgumentException("You must set Stream to False.", NameOf(textRequestBody))
        Return ResponseBase.FromJson(Await CompleteRawAsync(textRequestBody, cancellationToken))
    End Function

    Private Async Function StreamUtf8Async(textRequestBody As TextRequestBase,
                                           yieldCallback As Func(Of ReadOnlyMemory(Of Byte), Task),
                                           cancellationToken As CancellationToken) As Task
        Dim json = textRequestBody?.ToJsonUtf8

        Dim stream = Await PostAsync(RequestUrl, json, cancellationToken)

        Dim buffer(8192) As Byte
        While True
#If NET6_0_OR_GREATER Then
            Dim bytesRead = Await stream.ReadAsync(buffer, cancellationToken)
#Else
            Dim bytesRead = Await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken)
#End If

            If bytesRead <= 0 Then
                Exit While
            End If
            cancellationToken.ThrowIfCancellationRequested()
            Await yieldCallback(buffer.AsMemory(0, bytesRead))
        End While
    End Function

    ''' <summary>
    ''' Chat completion with streaming.
    ''' </summary>
    ''' <param name="textRequestBody">The request message.</param>
    ''' <param name="yieldCallback">The body of async for each loop.</param>
    ''' <param name="cancellationToken">Use it to cancel the request.</param>
    ''' <returns>The response message or error message.</returns>
    ''' <exception cref="HttpRequestException">
    ''' The request was not successful and the response stream is empty.
    ''' </exception>
    ''' <remarks>
    ''' <see href="https://bigmodel.cn/dev/api/normal-model/glm-4">Online documentation</see>
    ''' </remarks>
    Public Async Function StreamAsync(textRequestBody As TextRequestBase,
                                      yieldCallback As Action(Of ResponseBase),
                                      Optional cancellationToken As CancellationToken = Nothing) As Task
        Await StreamAsync(textRequestBody,
                          Function(resp)
                              yieldCallback(resp)
                              Return Task.CompletedTask
                          End Function, cancellationToken)
    End Function

    ''' <summary>
    ''' Chat completion with streaming.
    ''' </summary>
    ''' <param name="textRequestBody">The request message.</param>
    ''' <param name="yieldCallback">The body of async for each loop.</param>
    ''' <param name="cancellationToken">Use it to cancel the request.</param>
    ''' <returns>The response message or error message.</returns>
    ''' <exception cref="HttpRequestException">
    ''' The request was not successful and the response stream is empty.
    ''' </exception>
    ''' <remarks>
    ''' <see href="https://bigmodel.cn/dev/api/normal-model/glm-4">Online documentation</see>
    ''' </remarks>
    Public Async Function StreamAsync(textRequestBody As TextRequestBase,
                                      yieldCallback As Func(Of ResponseBase, Task),
                                      Optional cancellationToken As CancellationToken = Nothing) As Task
        If Not textRequestBody.Stream Then Throw New ArgumentException("You must set Stream to True.", NameOf(textRequestBody))

        Dim reader As New ServerSentEventReader(
            Async Function(jsonStream) As Task
                Dim response = ResponseBase.FromJson(jsonStream)
                If response IsNot Nothing Then
                    Await yieldCallback(response)
                End If
            End Function,
            Function(jsonStream) As Task
                ErrorHandler.ThrowErrorWithDetailedMessage(jsonStream)
                Return Task.CompletedTask
            End Function)

        Await StreamUtf8Async(textRequestBody, AddressOf reader.OnChunkAsync, cancellationToken)
    End Function

End Class
