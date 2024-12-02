Imports System.IO
Imports System.Net.Http
Imports System.Text
Imports System.Threading
Imports Nukepayload2.AI.Providers.Zhipu.Models
Imports Nukepayload2.AI.Providers.Zhipu.Utils

Public Class Chat
    Private Const API_TOKEN_TTL_SECONDS = 300

    Private Shared ReadOnly client As New HttpClient()

    Private ReadOnly _apiKey As String

    Public Sub New(apiKey As String)
        _apiKey = apiKey
    End Sub

    Private Async Function CompletionUtf8Async(textRequestBody As TextRequestBase, yieldCallback As Action(Of ReadOnlyMemory(Of Byte)), Optional cancellationToken As CancellationToken = Nothing) As Task
        Dim json As String = textRequestBody?.ToJson
        Dim data As New StringContent(json, Encoding.UTF8, "application/json")
        Dim api_key As String = AuthenticationUtils.GenerateToken(_apiKey, Chat.API_TOKEN_TTL_SECONDS)
        Dim request As New HttpRequestMessage() With {
            .Method = HttpMethod.Post,
            .RequestUri = New Uri("https://open.bigmodel.cn/api/paas/v4/chat/completions"),
            .Content = data
        }
        request.Headers.Add("Authorization", api_key)
        Dim response As HttpResponseMessage = Await Chat.client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
#If NET6_0_OR_GREATER Then
        Dim stream As Stream = Await response.Content.ReadAsStreamAsync(cancellationToken)
#Else
			Dim stream As Stream = Await response.Content.ReadAsStreamAsync()
#End If

        Dim buffer As Byte() = New Byte(8191) {}
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
            yieldCallback(buffer.AsMemory(0, bytesRead))
        End While
    End Function

    Public Async Function CompletionAsync(textRequestBody As TextRequestBase, Optional cancellationToken As CancellationToken = Nothing) As Task(Of ResponseBase)
        textRequestBody.Stream = False
        Dim ms As New MemoryStream
        Await CompletionUtf8Async(textRequestBody,
            Sub(str As ReadOnlyMemory(Of Byte))
                cancellationToken.ThrowIfCancellationRequested()
#If NET6_0_OR_GREATER Then
                ms.Write(str.Span)
#Else
					ms.Write(str.Span.ToArray, 0, str.Span.Length)
#End If
            End Sub, cancellationToken)
        ms.Position = 0L
        Return ResponseBase.FromJson(ms)
    End Function

    Public Async Function Stream(textRequestBody As TextRequestBase, yieldCallback As Action(Of ResponseBase)) As Task
        textRequestBody.Stream = True
        ' 原版 SDK 就写成这样了，字符串这样拼也是醉了
        Dim buffer As String = String.Empty
        Dim bufferBuilder As New StringBuilder
        Await CompletionUtf8Async(textRequestBody,
            Sub(chunk As ReadOnlyMemory(Of Byte))
#If NET6_0_OR_GREATER Then
                buffer += Encoding.UTF8.GetString(chunk.Span)
#Else
					buffer += Encoding.UTF8.GetString(chunk.Span.ToArray)
#End If

                While True
                    Dim startPos = buffer.IndexOf("data: ", StringComparison.Ordinal)
                    If startPos = -1 Then
                        ' No data
                        Exit While
                    End If
                    Dim endPos = buffer.IndexOf(vbLf & vbLf, startPos, StringComparison.Ordinal)
                    If endPos = -1 Then
                        ' We don't have a complete JSON in our buffer yet, break the loop until we get more chunks
                        Exit While
                    End If
                    startPos += "data: ".Length
                    Dim jsonString = buffer.Substring(startPos, endPos - startPos)
                    If jsonString.Equals("[DONE]") Then
                        Exit While
                    End If
                    Dim response = ResponseBase.FromJson(jsonString)
                    If response IsNot Nothing Then
                        yieldCallback(response)
                    End If
                    buffer = buffer.Substring(endPos + 2)
                End While
            End Sub, Nothing)
        Dim finalResponse = ResponseBase.FromJson(buffer)
        If finalResponse IsNot Nothing Then
            yieldCallback(finalResponse)
        End If
    End Function

End Class
