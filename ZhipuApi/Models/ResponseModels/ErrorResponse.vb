Imports Newtonsoft.Json
Imports Nukepayload2.AI.Providers.Zhipu.Serialization
Imports Nukepayload2.IO.Json.Serialization.NewtonsoftJson
Imports System.IO

Namespace Models
    Public Class ErrorResponse
        Public Property [Error] As RequestError

        Public Shared Function FromJson(json As Stream) As ErrorResponse
            Using jsonReader As New JsonTextReader(New StreamReader(json))
                jsonReader.DateParseHandling = DateParseHandling.None
                Return ErrorResponseReader.ReadErrorResponse(jsonReader, JsonReadErrorHandler.DefaultHandler)
            End Using
        End Function
    End Class

    Public Class RequestError
        ''' <summary>
        ''' 错误消息
        ''' </summary>
        Public Property Message As String
        ''' <summary>
        ''' 错误码
        ''' </summary>
        Public Property Code As String

    End Class

End Namespace