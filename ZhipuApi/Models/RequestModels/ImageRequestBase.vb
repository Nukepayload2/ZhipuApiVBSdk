Imports Newtonsoft.Json
Imports System.IO

Namespace Models
    Public Class ImageRequestBase
        Public Property Model As String

        Public Property Prompt As String

        Public Function ToJson() As String
            Using sw As New StringWriter, jsonWriter As New JsonTextWriter(sw)
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

                Return sw.ToString()
            End Using
        End Function
    End Class
End Namespace
