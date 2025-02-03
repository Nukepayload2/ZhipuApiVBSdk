Imports System.IO
Imports Nukepayload2.AI.Providers.Zhipu.Utils

Friend Class ServerSentEventReader
    Private _rawBuffer As Byte() = Array.Empty(Of Byte)
    Private _rawBufferSize As Integer = 0
    Private ReadOnly _onEventDataReceivedAsync As Func(Of MemoryStream, Task)

    Private Shared ReadOnly s_streamStartUtf8 As Byte() = IoUtils.UTF8NoBOM.GetBytes("data: ")
    Private Shared ReadOnly s_streamEndUtf8 As Byte() = IoUtils.UTF8NoBOM.GetBytes(vbLf & vbLf)
    Private Shared ReadOnly s_streamDoneUtf8 As Byte() = IoUtils.UTF8NoBOM.GetBytes("[DONE]")

    Sub New(onEventDataReceivedAsync As Func(Of MemoryStream, Task))
        _onEventDataReceivedAsync = onEventDataReceivedAsync
    End Sub

    ' chunk 是滑动窗口的数据，要循环每一个窗口，找出每一段数据来调用 yieldCallback。
    ' 数据的起始是 "data: "，结束是 `vbLf & vbLf`。
    ' 最后一段数据是 "data: [DONE]"。
    ' C# SDK 的 bug 是，UTF-8 字符会被 chunk 的边界截断，从而引发乱码。我们这里会修复这个乱码问题，顺便通过减少解码的次数来改善性能。
    Async Function OnChunkAsync(chunk As ReadOnlyMemory(Of Byte)) As Task
        ' 小心：Async Function 的闭包里面不能有 Span。编译之前看清楚每个变量的类型。

        ' 参数检查
        If chunk.Length < 1 Then Return

        ' 扩容以便追加 chunk
        Dim newLength = _rawBufferSize + chunk.Length
        If newLength > _rawBuffer.Length Then
            Dim newSize = Math.Max(chunk.Length + _rawBuffer.Length, _rawBuffer.Length * 2)
            Array.Resize(_rawBuffer, newSize)
        End If

        ' 把 chunk 追加到 rawBuffer
        chunk.Span.CopyTo(_rawBuffer.AsSpan(_rawBufferSize, chunk.Length))
        _rawBufferSize = newLength

        ' 此时 rawBuffer 中可能有多条数据，一个个找。
        Dim startPos = 0
        Dim lastSuccessfulStartPos = 0
        Do
            ' 寻找一段数据起始位置
            Dim nextStartPos = _rawBuffer.AsSpan(startPos, _rawBufferSize - startPos).IndexOf(s_streamStartUtf8)
            If nextStartPos = -1 Then
                ' 没有起始位置，等下一个窗口
                Exit Do
            End If

            ' 移动 startPos，跳过头部
            lastSuccessfulStartPos = startPos + nextStartPos
            startPos += nextStartPos + s_streamStartUtf8.Length

            ' 从数据的起始位置开始寻找一段数据的结束位置
            Dim dataLength = _rawBuffer.AsSpan(startPos, _rawBufferSize - startPos).IndexOf(s_streamEndUtf8)
            If dataLength = -1 Then
                ' 没有结束位置，等下一个窗口
                Exit Do
            End If

            ' 这是最后一段数据吗？
            If _rawBuffer.AsSpan(startPos, dataLength).StartsWith(s_streamDoneUtf8) Then
                ' 最后一段数据用来终止迭代，不应向外部报告，也不用给下次调用清理空间。
                _rawBufferSize = 0
                Return
            End If

            ' 报告一段数据
            Dim jsonStream As New MemoryStream(_rawBuffer, startPos, dataLength)
            Await _onEventDataReceivedAsync(jsonStream)

            ' 移动 startPos 到上一段数据的末尾之后，下次就从这里开始查找
            startPos += dataLength + s_streamEndUtf8.Length
            lastSuccessfulStartPos = startPos
            ' 这个 chunk 处理完了就退出循环
        Loop While startPos < _rawBufferSize

        ' 把处理过的数据删掉，给下个 chunk 腾空间
        If lastSuccessfulStartPos > 0 Then
            _rawBufferSize -= lastSuccessfulStartPos
            _rawBuffer.AsSpan(lastSuccessfulStartPos, _rawBufferSize).CopyTo(_rawBuffer.AsSpan(0, _rawBufferSize))
        End If
    End Function
End Class
