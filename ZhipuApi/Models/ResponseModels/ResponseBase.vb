Imports System.ComponentModel
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

        Public Property Usage As UsageData

        Public Property Choices As IReadOnlyList(Of ResponseChoiceItem)
        Public Property WebSearch As WebSearchResponse
        Public Property ContentFilter As ContentFilter
        ''' <summary>
        ''' 包含错误码和异常描述。已迁移到 <see cref="ZhipuHttpRequestException"/>。此属性用作 Beta 版二进制兼容，在后续版本将被删除。
        ''' </summary>
        <EditorBrowsable(EditorBrowsableState.Never)>
        <Obsolete("Catch ZhipuHttpRequestException instead", True)>
        Public Property [Error] As Dictionary(Of String, String)

        Public Shared Function FromJson(json As Stream) As ResponseBase
            Using jsonReader As New JsonTextReader(New StreamReader(json))
                Return ResponseReader.ReadResponseBase(jsonReader, JsonReadErrorHandler.DefaultHandler)
            End Using
        End Function

        Public Shared Function FromJson(json As String) As ResponseBase
            Using jsonReader As New JsonTextReader(New StringReader(json))
                Return ResponseReader.ReadResponseBase(jsonReader, JsonReadErrorHandler.DefaultHandler)
            End Using
        End Function

    End Class
End Namespace
