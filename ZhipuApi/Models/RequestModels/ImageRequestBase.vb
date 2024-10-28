Imports Newtonsoft.Json
Imports System.IO
Namespace ZhipuApi.Models.RequestModels
    Public Class ImageRequestBase
        Public Property model As String

        Public Property prompt As String

        Public Function ToJson() As String
            Using sw As New StringWriter
                Dim jsonWriter As New JsonTextWriter(sw)

                jsonWriter.WriteStartObject()

                If model IsNot Nothing Then
                    jsonWriter.WritePropertyName("model")
                    jsonWriter.WriteValue(model)
                End If

                If prompt IsNot Nothing Then
                    jsonWriter.WritePropertyName("prompt")
                    jsonWriter.WriteValue(prompt)
                End If

                jsonWriter.WriteEndObject()

                Return sw.ToString()
            End Using
        End Function
    End Class
End Namespace
