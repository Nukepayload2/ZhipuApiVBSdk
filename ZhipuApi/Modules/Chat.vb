Imports System.IO
Imports System.Net.Http
Imports System.Text
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

    Private Async Function CompleteRawAsync(textRequestBody As TextRequestBase, cancellation As CancellationToken) As Task(Of MemoryStream)
        Dim json = textRequestBody?.ToJsonUtf8
        Const requestUrl = "https://open.bigmodel.cn/api/paas/v4/chat/completions"
#If DEBUG Then
        Debug.WriteLine("Sending chat request: ")
        Debug.WriteLine(IoUtils.UTF8NoBOM.GetString(json.ToArray()))
#End If
        Return Await PostAsync(requestUrl, json, cancellation)
    End Function

    Public Async Function CompleteAsync(textRequestBody As TextRequestBase,
                                        Optional cancellationToken As CancellationToken = Nothing) As Task(Of ResponseBase)
        If textRequestBody.Stream Then Throw New ArgumentException("You must set Stream to False.", NameOf(textRequestBody))
        Return ResponseBase.FromJson(Await CompleteRawAsync(textRequestBody, cancellationToken))
    End Function

    Private Async Function StreamUtf8Async(textRequestBody As TextRequestBase,
                                           yieldCallback As Func(Of ReadOnlyMemory(Of Byte), Task),
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
            Await yieldCallback(buffer.AsMemory(0, bytesRead))
        End While
    End Function

    Public Async Function StreamAsync(textRequestBody As TextRequestBase,
                                      yieldCallback As Action(Of ResponseBase),
                                      Optional cancellationToken As CancellationToken = Nothing) As Task
        Await StreamAsync(textRequestBody,
                          Function(resp)
                              yieldCallback(resp)
                              Return Task.CompletedTask
                          End Function, cancellationToken)
    End Function

    Private Shared ReadOnly s_streamStartUtf8 As Byte() = IoUtils.UTF8NoBOM.GetBytes("data: ")
    Private Shared ReadOnly s_streamEndUtf8 As Byte() = IoUtils.UTF8NoBOM.GetBytes(vbLf & vbLf)

    Public Async Function StreamAsync(textRequestBody As TextRequestBase,
                                      yieldCallback As Func(Of ResponseBase, Task),
                                      Optional cancellationToken As CancellationToken = Nothing) As Task
        If Not textRequestBody.Stream Then Throw New ArgumentException("You must set Stream to True.", NameOf(textRequestBody))
        ' C# SDK 就写成这样了，字符串这样拼也是醉了
        ' 原始代码的含义是，chunk 是滑动窗口的大小，要找出每一段数据来调用 yieldCallback。
        ' 数据的起始是 `"data: "`，结束是 `vbLf & vbLf`。
        ' 最后一段数据是 "data: [DONE]"。
        ' C# SDK 的 bug 是，UTF8 字符会被 chunk 的边界截断，从而引发乱码。我们这里会修复这个乱码问题。
        Dim buffer As String = String.Empty
        Await StreamUtf8Async(textRequestBody,
            Async Function(chunk As ReadOnlyMemory(Of Byte))
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
                        Await yieldCallback(response)
                    End If
                    buffer = buffer.Substring(endPos + 2)
                End While
            End Function, cancellationToken)
        If Not buffer.StartsWith("data: [DONE]") Then
            Dim finalResponse = ResponseBase.FromJson(buffer)
            If finalResponse IsNot Nothing Then
                Await yieldCallback(finalResponse)
            End If
        End If
    End Function

End Class
