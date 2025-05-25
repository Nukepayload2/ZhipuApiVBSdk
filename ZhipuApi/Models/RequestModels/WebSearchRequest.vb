Imports System.IO
Imports Newtonsoft.Json
Imports Nukepayload2.AI.Providers.Zhipu.Serialization
Imports Nukepayload2.AI.Providers.Zhipu.Utils

Namespace Models
    ''' <summary>
    ''' Represents <c>WebSearchRequest</c> in JSON.
    ''' </summary>
    Public Class WebSearchRequest
        ''' <summary>
        ''' 要调用的搜索引擎编码。目前支持：
        ''' search_std: 智谱基础版搜索引擎，
        ''' search_pro: 智谱高阶版搜索引擎，
        ''' search_pro_sogou:搜狗，
        ''' search_pro_quark: 夸克搜索，
        ''' search_pro_jina: jina.ai搜索
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>search_engine</c> in json.
        ''' </remarks>
        Public Property SearchEngine As String
        ''' <summary>
        ''' 需要进行搜索的内容, 建议搜索 query 不超过 78 个字符
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>search_query</c> in json.
        ''' </remarks>
        Public Property SearchQuery As String
        ''' <summary>
        ''' 由用户端传递，需要唯一；用于区分每次请求的唯一标识符。如果用户端未提供，平台将默认生成
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>request_id</c> in json.
        ''' </remarks>
        Public Property RequestId As String
        ''' <summary>
        ''' 终端用户的唯一ID，帮助平台对终端用户的非法活动、生成非法不当信息或其他滥用行为进行干预。ID长度要求：至少6个字符，最多128个字符
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>user_id</c> in json.
        ''' </remarks>
        Public Property UserId As String

        Public Function ToJsonUtf8() As MemoryStream
            Dim ms As New MemoryStream
            Using sw As New StreamWriter(ms, IoUtils.UTF8NoBOM, 8192, True), jsonWriter As New JsonTextWriter(sw)
                WebSearchRequestWriter.WriteWebSearchRequest(jsonWriter, Me)
            End Using
            ms.Position = 0
            Return ms
        End Function

        Public Function ToJson() As String
            Using stringWriter = New StringWriter, jsonWriter = New JsonTextWriter(stringWriter)
                WebSearchRequestWriter.WriteWebSearchRequest(jsonWriter, Me)
                Return stringWriter.ToString()
            End Using
        End Function

    End Class ' WebSearchRequest
End Namespace
