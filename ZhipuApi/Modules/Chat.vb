Imports System.IO
Imports System.Text
Imports System.Threading
Imports Nukepayload2.AI.Providers.Zhipu.Models

Public Class Chat
    Inherits ClientFeatureBase

    Public Sub New(apiKey As String)
        MyBase.New(apiKey)
    End Sub

    Private Async Function CompleteRawAsync(textRequestBody As TextRequestBase, cancellation As CancellationToken) As Task(Of MemoryStream)
        Dim json = textRequestBody?.ToJsonUtf8
        Const requestUrl = "https://open.bigmodel.cn/api/paas/v4/chat/completions"
        Return Await PostAsync(requestUrl, json, cancellation)
    End Function

    Public Async Function CompleteAsync(textRequestBody As TextRequestBase,
                                        Optional cancellationToken As CancellationToken = Nothing) As Task(Of ResponseBase)
        If textRequestBody.Stream Then Throw New ArgumentException("You must set Stream to False.", NameOf(textRequestBody))
        Return ResponseBase.FromJson(Await CompleteRawAsync(textRequestBody, cancellationToken))
    End Function

    Private Async Function StreamUtf8Async(textRequestBody As TextRequestBase,
                                           yieldCallback As Action(Of ReadOnlyMemory(Of Byte)),
                                           cancellationToken As CancellationToken) As Task
        Dim json = textRequestBody?.ToJsonUtf8
        Const requestUrl = "https://open.bigmodel.cn/api/paas/v4/chat/completions"

        Dim response = Await PostRawAsync(requestUrl, json, cancellationToken)
#If NET6_0_OR_GREATER Then
        Dim stream = Await response.Content.ReadAsStreamAsync(cancellationToken)
#Else
		Dim stream = Await response.Content.ReadAsStreamAsync()
#End If

        Dim buffer(8191) As Byte
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

    Public Async Function StreamAsync(textRequestBody As TextRequestBase, yieldCallback As Action(Of ResponseBase)) As Task
        If Not textRequestBody.Stream Then Throw New ArgumentException("You must set Stream to True.", NameOf(textRequestBody))
        ' 原版 SDK 就写成这样了，字符串这样拼也是醉了
        Dim buffer As String = String.Empty
        Await StreamUtf8Async(textRequestBody,
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
        If Not buffer.StartsWith("data: [DONE]") Then
            Try
                Dim finalResponse = ResponseBase.FromJson(buffer)
                If finalResponse IsNot Nothing Then
                    yieldCallback(finalResponse)
                End If
            Catch ex As Exception
            End Try
        End If
    End Function

End Class
