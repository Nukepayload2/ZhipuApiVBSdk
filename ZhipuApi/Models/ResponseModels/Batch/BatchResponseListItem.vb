Imports System.IO
Imports Newtonsoft.Json
Imports Nukepayload2.AI.Providers.Zhipu.Serialization
Imports Nukepayload2.IO.Json.Serialization.NewtonsoftJson

Namespace Models
    ''' <summary>
    ''' Represents <c>BatchChatResponseItem</c> in JSON.
    ''' </summary>
    Public Class BatchResponseListItem(Of T)
        ''' <summary>
        ''' Reads or writes <c>custom_id</c> in json.
        ''' </summary>
        Public Property CustomId As String
        ''' <summary>
        ''' Reads or writes <c>id</c> in json.
        ''' </summary>
        Public Property Id As String
        ''' <summary>
        ''' Reads or writes <c>response</c> in json.
        ''' </summary>
        Public Property Response As BatchResponseListItemContent(Of T)
    End Class ' BatchChatResponseItem

    ''' <summary>
    ''' The status code and response body of a single request.
    ''' </summary>
    Public Class BatchResponseListItemContent(Of T)
        ''' <summary>
        ''' Reads or writes <c>status_code</c> in json.
        ''' </summary>
        Public Property StatusCode As Long?
        ''' <summary>
        ''' Reads or writes <c>body</c> in json.
        ''' </summary>
        Public Property Body As T
    End Class ' BatchChatResponseContent

    ''' <summary>
    ''' 代表批量聊天回应的 JSONL 文件内容
    ''' </summary>
    Public Class BatchChatResponseItem
        Inherits BatchResponseListItem(Of ResponseBase)

        Public Shared Function FromJsonLines(jsonl As Stream) As IEnumerable(Of BatchChatResponseItem)
            Using streamReader As New StreamReader(jsonl)
                Return FromJsonLines(streamReader).ToArray
            End Using
        End Function

        Public Shared Iterator Function FromJsonLines(jsonl As TextReader) As IEnumerable(Of BatchChatResponseItem)
            Do
                Dim line = jsonl.ReadLine
                If line = Nothing Then Exit Do
                Using jsonReader As New JsonTextReader(New StringReader(line))
                    jsonReader.DateParseHandling = DateParseHandling.None
                    Yield BatchChatItemReader.ReadBatchChatResponseItem(jsonReader, JsonReadErrorHandler.DefaultHandler)
                End Using
            Loop
        End Function

        Public Shared Function FromJsonLines(jsonl As String) As IEnumerable(Of BatchChatResponseItem)
            Using streamReader As New StringReader(jsonl)
                Return FromJsonLines(streamReader)
            End Using
        End Function
    End Class

End Namespace
