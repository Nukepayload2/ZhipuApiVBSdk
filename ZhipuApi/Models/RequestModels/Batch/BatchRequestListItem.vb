Imports System.IO
Imports Newtonsoft.Json
Imports Nukepayload2.AI.Providers.Zhipu.Serialization
Imports Nukepayload2.AI.Providers.Zhipu.Utils

Namespace Models
    ''' <summary>
    ''' Batch request JSON list item
    ''' </summary>
    Public Class BatchRequestListItem(Of T)
        ''' <summary>
        ''' Reads or writes <c>custom_id</c> in json.
        ''' </summary>
        Public Property CustomId As String
        ''' <summary>
        ''' Reads or writes <c>method</c> in json.
        ''' </summary>
        ''' <value>
        ''' Can be value of <c>"POST"</c>
        ''' </value>
        Public Property Method As String
        ''' <summary>
        ''' Reads or writes <c>url</c> in json.
        ''' </summary>
        Public Property Url As String
        ''' <summary>
        ''' Reads or writes <c>body</c> in json.
        ''' </summary>
        Public Property Body As T
    End Class ' BatchRequestListItem

    ''' <summary>
    ''' Batch chat request JSON list item
    ''' </summary>
    Public Class BatchChatItem
        Inherits BatchRequestListItem(Of TextRequestBase)

        Public Sub WriteTo(target As Stream)
            Using sw As New StreamWriter(target, IoUtils.UTF8NoBOM, 8192, True)
                WriteTo(sw)
            End Using
        End Sub

        Public Sub WriteTo(target As StreamWriter)
            Using jsonWriter As New JsonTextWriter(target)
                BatchChatItemWriter.WriteBatchChatItem(jsonWriter, Me)
            End Using
        End Sub

        Public Function ToJson() As String
            Using stringWriter = New StringWriter, jsonWriter = New JsonTextWriter(stringWriter)
                BatchChatItemWriter.WriteBatchChatItem(jsonWriter, Me)
                Return stringWriter.ToString()
            End Using
        End Function

    End Class
End Namespace