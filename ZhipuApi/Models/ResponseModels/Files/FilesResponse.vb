Imports Newtonsoft.Json
Imports Nukepayload2.AI.Providers.Zhipu.Serialization
Imports Nukepayload2.IO.Json.Serialization.NewtonsoftJson
Imports System.IO

Namespace Models
    ''' <summary>
    ''' 单个文件的基本信息
    ''' </summary>
    Public Class FileItem
        ''' <summary>
        ''' 该文件的唯一标识符。
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>id</c> in json.
        ''' </remarks>
        Public Property Id As String
        ''' <summary>
        ''' 返回对象的类型。
        ''' </summary>
        ''' <value>
        ''' Can be value of <c>"file"</c>
        ''' </value>
        ''' <remarks>
        ''' Reads or writes <c>object</c> in json.
        ''' </remarks>
        Public Property TypeName As String
        ''' <summary>
        ''' 文件大小（字节为单位）。
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>bytes</c> in json.
        ''' </remarks>
        Public Property Bytes As Long?
        ''' <summary>
        ''' 创建时间戳。
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>created_at</c> in json.
        ''' </remarks>
        Public Property CreatedAt As Long?
        ''' <summary>
        ''' 文件名称。
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>filename</c> in json.
        ''' </remarks>
        Public Property Filename As String
        ''' <summary>
        ''' 文件用途说明。
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>purpose</c> in json.
        ''' </remarks>
        Public Property Purpose As String
        ''' <summary>
        ''' 文件当前状态，可能为：已上传、已处理、待处理、错误、正在删除或已删除。
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>status</c> in json.
        ''' </remarks>
        Public Property Status As String
        ''' <summary>
        ''' 文件状态的附加说明信息。若文件处于错误状态，此处将包含具体的错误描述信息。
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>status_details</c> in json.
        ''' </remarks>
        Public Property StatusDetails As String

        Public Shared Function FromJson(json As Stream) As FileItem
            Using jsonReader As New JsonTextReader(New StreamReader(json))
                jsonReader.DateParseHandling = DateParseHandling.None
                Return FileResponseReader.ReadFileItem(jsonReader, JsonReadErrorHandler.DefaultHandler)
            End Using
        End Function
    End Class ' FileItem

    ''' <summary>
    ''' 删除文件的响应
    ''' </summary>
    Public Class FileDeleteResponse
        ''' <summary>
        ''' 该文件的唯一标识符。
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>id</c> in json.
        ''' </remarks>
        Public Property Id As String
        ''' <summary>
        ''' 是否已删除。
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>deleted</c> in json.
        ''' </remarks>
        Public Property Deleted As Boolean?
        ''' <summary>
        ''' 被删除对象的类型。
        ''' </summary>
        ''' <value>
        ''' Can be value of <c>"file"</c>
        ''' </value>
        ''' <remarks>
        ''' Reads or writes <c>object</c> in json.
        ''' </remarks>
        Public Property TypeName As String

        Public Shared Function FromJson(json As Stream) As FileDeleteResponse
            Using jsonReader As New JsonTextReader(New StreamReader(json))
                jsonReader.DateParseHandling = DateParseHandling.None
                Return FileResponseReader.ReadFileDeleteResponse(jsonReader, JsonReadErrorHandler.DefaultHandler)
            End Using
        End Function
    End Class ' FileDeleteResponse

    ''' <summary>
    ''' 列出文件的响应
    ''' </summary>
    Public Class FilesListResponse
        ''' <summary>
        ''' Reads or writes <c>object</c> in json.
        ''' </summary>
        Public Property TypeName As String
        ''' <summary>
        ''' Reads or writes <c>data</c> in json.
        ''' </summary>
        Public Property Data As IReadOnlyList(Of FileItem)
        ''' <summary>
        ''' 下一页的页码标识（用于分页）
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>next_page</c> in json.
        ''' </remarks>
        Public Property NextPage As String
        Public Shared Function FromJson(json As Stream) As FilesListResponse
            Using jsonReader As New JsonTextReader(New StreamReader(json))
                jsonReader.DateParseHandling = DateParseHandling.None
                Return FileResponseReader.ReadFilesListResponse(jsonReader, JsonReadErrorHandler.DefaultHandler)
            End Using
        End Function
    End Class ' FilesListResponse
End Namespace
