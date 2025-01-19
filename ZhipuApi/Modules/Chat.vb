Imports System.IO
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

        Dim buffer(8) As Byte
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
    Private Shared ReadOnly s_streamDoneUtf8 As Byte() = IoUtils.UTF8NoBOM.GetBytes("[DONE]")

    Public Async Function StreamAsync(textRequestBody As TextRequestBase,
                                      yieldCallback As Func(Of ResponseBase, Task),
                                      Optional cancellationToken As CancellationToken = Nothing) As Task
        If Not textRequestBody.Stream Then Throw New ArgumentException("You must set Stream to True.", NameOf(textRequestBody))
        ' chunk 是滑动窗口的数据，要循环每一个窗口，找出每一段数据来调用 yieldCallback。
        ' 数据的起始是 "data: "，结束是 `vbLf & vbLf`。
        ' 最后一段数据是 "data: [DONE]"。
        ' C# SDK 的 bug 是，UTF-8 字符会被 chunk 的边界截断，从而引发乱码。我们这里会修复这个乱码问题，顺便通过减少解码的次数来改善性能。
        Dim rawBuffer As Byte() = Array.Empty(Of Byte)
        Dim rawBufferSize = 0
        Await StreamUtf8Async(textRequestBody,
            Async Function(chunk As ReadOnlyMemory(Of Byte))
                Debug.WriteLine("当前 Chunk 是: " & IoUtils.UTF8NoBOM.GetString(chunk.ToArray))
                ' 小心：Async Function 的闭包里面不能有 Span。编译之前看清楚每个变量的类型。

                ' 参数检查
                If chunk.Length < 1 Then Return

                ' 扩容以便追加 chunk
                Dim newLength = rawBufferSize + chunk.Length
                If newLength > rawBuffer.Length Then
                    Dim newSize = Math.Max(chunk.Length + rawBuffer.Length, rawBuffer.Length * 2)
                    Array.Resize(rawBuffer, newSize)
                End If
                rawBufferSize = newLength

                ' 把 chunk 追加到 rawBuffer
                chunk.Span.CopyTo(rawBuffer.AsSpan(0, rawBufferSize))

                ' 此时 rawBuffer 中可能有多条数据，一个个找。
                Dim startPos = 0
                Do
                    ' 寻找一段数据起始位置
                    Dim nextStartPos = rawBuffer.AsSpan(startPos, rawBufferSize - startPos).IndexOf(s_streamStartUtf8)
                    If nextStartPos = -1 Then
                        ' 没有起始位置，等下一个窗口
                        Exit Do
                    End If

                    ' 移动 startPos，跳过头部
                    startPos += nextStartPos + s_streamStartUtf8.Length

                    ' 从数据的起始位置开始寻找一段数据的结束位置
                    Dim dataLength = rawBuffer.AsSpan(startPos, rawBufferSize - startPos).IndexOf(s_streamEndUtf8)
                    If dataLength = -1 Then
                        ' 没有结束位置，等下一个窗口
                        Exit Do
                    End If

                    ' 这是最后一段数据吗？
                    If rawBuffer.AsSpan(startPos, dataLength).StartsWith(s_streamDoneUtf8) Then
                        ' 最后一段数据用来终止迭代，不应向外部报告
                        Exit Do
                    End If

                    ' 报告一段数据
                    Dim jsonStream As New MemoryStream(rawBuffer, startPos, dataLength)
                    Debug.WriteLine("处理 JSON: " & IoUtils.UTF8NoBOM.GetString(rawBuffer, startPos, dataLength))
                    Dim response = ResponseBase.FromJson(jsonStream)
                    If response IsNot Nothing Then
                        Await yieldCallback(response)
                    End If

                    ' 移动 startPos 到上一段数据的末尾之后，下次就从这里开始查找
                    startPos += dataLength + s_streamEndUtf8.Length

                    ' 这个 chunk 处理完了就退出循环
                Loop While startPos < rawBufferSize

                ' 把处理过的数据删掉，给下个 chunk 腾空间
                If startPos > 0 Then
                    rawBufferSize -= startPos
                    rawBuffer.AsSpan(startPos, rawBufferSize).CopyTo(rawBuffer.AsSpan(0, rawBufferSize))
                End If
            End Function, cancellationToken)

    End Function

End Class
