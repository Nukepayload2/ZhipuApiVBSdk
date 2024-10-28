Imports Newtonsoft.Json
Imports System.IO
Namespace ZhipuApi.Models.RequestModels
    Public Class EmbeddingRequestBase
        Public Property model As String

        Public Property input As String

        Public Function ToJson() As String
            Dim writer As New StringWriter
            Dim jsonWriter As New JsonTextWriter(writer)
            jsonWriter.WriteStartObject()
            If model IsNot Nothing Then
                jsonWriter.WritePropertyName("model")
                jsonWriter.WriteValue(model)
            End If
            If input IsNot Nothing Then
                jsonWriter.WritePropertyName("input")
                jsonWriter.WriteValue(input)
            End If
            jsonWriter.WriteEndObject()
            Return writer.ToString()
        End Function
    End Class
End Namespace
