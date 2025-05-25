Imports System.IO
Imports Newtonsoft.Json
Imports Nukepayload2.AI.Providers.Zhipu.Serialization
Imports Nukepayload2.IO.Json.Serialization.NewtonsoftJson

Namespace Models
    ''' <summary>
    ''' 搜索意图结果
    ''' </summary>
    Public Class SearchIntent
        ''' <summary>
        ''' 原始搜索query
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>query</c> in json.
        ''' </remarks>
        Public Property Query As String
        ''' <summary>
        ''' 识别的意图类型（SEARCH_ALL=全网搜索，SEARCH_NONE=无搜索意图）
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>intent</c> in json.
        ''' </remarks>
        Public Property Intent As String
        ''' <summary>
        ''' 改写后的搜索关键词
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>keywords</c> in json.
        ''' </remarks>
        Public Property Keywords As String
    End Class ' SearchIntent

    ''' <summary>
    ''' 搜索结果
    ''' </summary>
    Public Class SearchResult
        ''' <summary>
        ''' 结果标题
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>title</c> in json.
        ''' </remarks>
        Public Property Title As String
        ''' <summary>
        ''' 内容摘要
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>content</c> in json.
        ''' </remarks>
        Public Property Content As String
        ''' <summary>
        ''' 结果链接
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>link</c> in json.
        ''' </remarks>
        Public Property Link As String
        ''' <summary>
        ''' 网站名称
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>media</c> in json.
        ''' </remarks>
        Public Property Media As String
        ''' <summary>
        ''' 网站图标URL
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>icon</c> in json.
        ''' </remarks>
        Public Property Icon As String
        ''' <summary>
        ''' 角标序号（如ref_1）
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>refer</c> in json.
        ''' </remarks>
        Public Property Refer As String
    End Class ' SearchResult

    ''' <summary>
    ''' 网络搜索响应
    ''' </summary>
    Public Class WebSearchResponse
        ''' <summary>
        ''' 任务ID
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>id</c> in json.
        ''' </remarks>
        Public Property Id As String
        ''' <summary>
        ''' 请求创建时间（Unix时间戳秒）
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>created</c> in json.
        ''' </remarks>
        Public Property Created As Long?
        ''' <summary>
        ''' 搜索意图结果列表
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>search_intent</c> in json.
        ''' </remarks>
        Public Property SearchIntent As IReadOnlyList(Of SearchIntent)
        ''' <summary>
        ''' 搜索结果条目列表
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>search_result</c> in json.
        ''' </remarks>
        Public Property SearchResult As IReadOnlyList(Of SearchResult)

        Public Shared Function FromJson(json As Stream) As WebSearchResponse
            Using jsonReader As New JsonTextReader(New StreamReader(json))
                Return WebSearchResponseReader.ReadWebSearchResponse(jsonReader, JsonReadErrorHandler.DefaultHandler)
            End Using
        End Function

        Public Shared Function FromJson(json As String) As WebSearchResponse
            Using jsonReader As New JsonTextReader(New StringReader(json))
                Return WebSearchResponseReader.ReadWebSearchResponse(jsonReader, JsonReadErrorHandler.DefaultHandler)
            End Using
        End Function

    End Class ' WebSearchResponse
End Namespace
