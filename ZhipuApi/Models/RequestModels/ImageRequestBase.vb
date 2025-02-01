Imports Newtonsoft.Json
Imports Nukepayload2.AI.Providers.Zhipu.Utils
Imports System.IO

Namespace Models
    ''' <summary>
    ''' For cogview models.
    ''' </summary>
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
            Using sw As New StreamWriter(ms, IoUtils.UTF8NoBOM, 8192, True), jsonWriter As New JsonTextWriter(sw)
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
