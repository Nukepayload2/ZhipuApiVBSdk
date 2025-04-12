Imports Newtonsoft.Json
Imports Nukepayload2.AI.Providers.Zhipu.Utils
Imports System.IO

Namespace Models
    Public Class EmbeddingRequestBase
        Public Property Model As String

        Public Property Input As String

        Public Property InputList As IReadOnlyList(Of String)

        Public Property Dimensions As Integer?

        Public Function ToJsonUtf8() As MemoryStream
            Dim ms As New MemoryStream
            Using sw As New StreamWriter(ms, IoUtils.UTF8NoBOM, 8192, True), jsonWriter As New JsonTextWriter(sw)
                ToJsonInternal(jsonWriter)
            End Using
            ms.Position = 0
            Return ms
        End Function

        Public Function ToJson() As String
            Using writer As New StringWriter, jsonWriter As New JsonTextWriter(writer)
                ToJsonInternal(jsonWriter)
                Return writer.ToString()
            End Using
        End Function

        Private Sub ToJsonInternal(jsonWriter As JsonTextWriter)
            jsonWriter.WriteStartObject()
            If Model IsNot Nothing Then
                jsonWriter.WritePropertyName("model")
                jsonWriter.WriteValue(Model)
            End If
            If Input IsNot Nothing Then
                jsonWriter.WritePropertyName("input")
                jsonWriter.WriteValue(Input)
            ElseIf InputList IsNot Nothing Then
                jsonWriter.WritePropertyName("input")
                jsonWriter.WriteStartArray()
                For Each i In InputList
                    jsonWriter.WriteValue(i)
                Next
                jsonWriter.WriteEndArray()
            End If
            If Dimensions IsNot Nothing Then
                jsonWriter.WritePropertyName("dimensions")
                jsonWriter.WriteValue(Dimensions)
            End If
            jsonWriter.WriteEndObject()
        End Sub
    End Class
End Namespace
