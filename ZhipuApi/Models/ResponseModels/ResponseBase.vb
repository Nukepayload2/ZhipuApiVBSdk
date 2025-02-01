Imports System.IO
Imports Newtonsoft.Json
Imports Nukepayload2.AI.Providers.Zhipu.Serialization
Imports Nukepayload2.IO.Json.Serialization.NewtonsoftJson

Namespace Models
    Public Class ResponseBase
        Public Property Id As String

        Public Property RequestId As String

        Public Property Created As Long?

        Public Property Model As String

        Public Property Usage As Dictionary(Of String, Integer?)

        Public Property Choices As IReadOnlyList(Of ResponseChoiceItem)

        Public Property [Error] As Dictionary(Of String, String)

        Private Shared ReadOnly s_defaultErrorHandler As New JsonReadErrorHandler

        Public Shared Function FromJson(json As Stream) As ResponseBase
            Using jsonReader As New JsonTextReader(New StreamReader(json))
                Return ResponseReader.ReadResponseBase(jsonReader, s_defaultErrorHandler)
            End Using
        End Function

        Public Shared Function FromJson(json As String) As ResponseBase
            Using jsonReader As New JsonTextReader(New StringReader(json))
                Return ResponseReader.ReadResponseBase(jsonReader, s_defaultErrorHandler)
            End Using
        End Function

    End Class
End Namespace
