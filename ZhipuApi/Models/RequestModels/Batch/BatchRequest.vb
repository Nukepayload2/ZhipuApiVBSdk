Imports Newtonsoft.Json.Linq

Namespace Models
    ''' <summary>
    ''' Represents <c>BatchCreateParams</c> in JSON.
    ''' </summary>
    Public Class BatchCreateParams
        ''' <summary>
        ''' Batch 中所有请求将使用的端点。
        ''' </summary>
        ''' <value>
        ''' Can be value of <c>"/v4/chat/completions"</c>, <c>"/v4/embeddings"</c>
        ''' </value>
        ''' <remarks>
        ''' Reads or writes <c>endpoint</c> in json.
        ''' </remarks>
        Public Property Endpoint As String
        ''' <summary>
        ''' 上传文件的 ID，该文件包含Batch的请求。输入文件必须是 .Jsonl 格式，并且文件上传时的目的必须标记为"batch"。
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>input_file_id</c> in json.
        ''' </remarks>
        Public Property InputFileId As String
        ''' <summary>
        ''' 是否自动删除batch原始文件。
        ''' </summary>
        ''' <value>
        ''' The default value is <c>True</c>
        ''' </value>
        ''' <remarks>
        ''' Reads or writes <c>auto_delete_input_file</c> in json.
        ''' </remarks>
        Public Property AutoDeleteInputFile As Boolean?
        ''' <summary>
        ''' 用于存储与 Batch 相关的数据，如客户ID、描述或其他任务管理和跟踪所需的额外信息。每个键的长度最多为 64 个字符。
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>metadata</c> in json.
        ''' </remarks>
        Friend Property Metadata As JObject
    End Class ' BatchCreateParams

    ''' <summary>
    ''' Represents <c>BatchListRequest</c> in JSON.
    ''' </summary>
    Public Class BatchListRequest
        ''' <summary>
        ''' Cursor for pagination
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>after</c> in json.
        ''' </remarks>
        Public Property After As String
        ''' <summary>
        ''' Reads or writes <c>limit</c> in json.
        ''' </summary>
        Public Property Limit As Long?
    End Class ' BatchListRequest
End Namespace