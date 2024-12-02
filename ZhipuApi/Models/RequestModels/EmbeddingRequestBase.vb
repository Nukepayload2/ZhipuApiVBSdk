Imports Newtonsoft.Json
Imports System.IO

Namespace Models
    Public Class EmbeddingRequestBase
        Public Property Model As String

        Public Property Input As String

        Public Function ToJson() As String
            Using writer As New StringWriter, jsonWriter As New JsonTextWriter(writer)
                jsonWriter.WriteStartObject()
                If Model IsNot Nothing Then
                    jsonWriter.WritePropertyName("model")
                    jsonWriter.WriteValue(Model)
                End If
                If Input IsNot Nothing Then
                    jsonWriter.WritePropertyName("input")
                    jsonWriter.WriteValue(Input)
                End If
                jsonWriter.WriteEndObject()

                Return writer.ToString()
            End Using
        End Function
    End Class
End Namespace
