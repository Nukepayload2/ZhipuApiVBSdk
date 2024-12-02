Imports Newtonsoft.Json
Imports System.IO

Namespace Models
    Public Class ImageRequestBase
        Public Property Model As String

        Public Property Prompt As String

        Public Function ToJson() As String
            Using sw As New StringWriter, jsonWriter As New JsonTextWriter(sw)
                ToJsonInternal(jsonWriter)
                Return sw.ToString()
            End Using
        End Function

        Public Function ToJsonUtf8() As MemoryStream
            Dim ms As New MemoryStream
            Using sw As New StreamWriter(ms, Nothing, -1, True), jsonWriter As New JsonTextWriter(sw)
                ToJsonInternal(jsonWriter)
            End Using
            ms.Position = 0
            Return ms
        End Function

        Private Sub ToJsonInternal(jsonWriter As JsonTextWriter)
            jsonWriter.WriteStartObject()

            If Model IsNot Nothing Then
                jsonWriter.WritePropertyName("model")
                jsonWriter.WriteValue(Model)
            End If

            If Prompt IsNot Nothing Then
                jsonWriter.WritePropertyName("prompt")
                jsonWriter.WriteValue(Prompt)
            End If

            jsonWriter.WriteEndObject()
        End Sub
    End Class
End Namespace
