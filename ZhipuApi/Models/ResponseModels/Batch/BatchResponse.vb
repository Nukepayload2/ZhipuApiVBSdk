Imports Newtonsoft.Json
Imports System.IO
Imports Newtonsoft.Json.Linq
Imports Nukepayload2.AI.Providers.Zhipu.Serialization
Imports Nukepayload2.AI.Providers.Zhipu.Utils
Imports Nukepayload2.IO.Json.Serialization.NewtonsoftJson

Namespace Models
    ''' <summary>
    ''' Represents <c>BatchError</c> in JSON.
    ''' </summary>
    Public Class BatchError
        ''' <summary>
        ''' 错误代码标识符
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>code</c> in json.
        ''' </remarks>
        Public Property Code As String
        ''' <summary>
        ''' 输入文件中出错的行号（从1开始计数）
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>line</c> in json.
        ''' </remarks>
        Public Property Line As Long?
        ''' <summary>
        ''' 具体的错误信息描述
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>message</c> in json.
        ''' </remarks>
        Public Property Message As String
        ''' <summary>
        ''' 导致错误的具体参数名称（如适用）
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>param</c> in json.
        ''' </remarks>
        Public Property Param As String
    End Class ' BatchError
    ''' <summary>
    ''' Represents <c>BatchRequestCounts</c> in JSON.
    ''' </summary>
    Public Class BatchRequestCounts
        ''' <summary>
        ''' 已成功完成的请求数量
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>completed</c> in json.
        ''' </remarks>
        Public Property Completed As Long?
        ''' <summary>
        ''' 失败请求的数量
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>failed</c> in json.
        ''' </remarks>
        Public Property Failed As Long?
        ''' <summary>
        ''' 批处理中的总请求数量（等于completed+failed）
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>total</c> in json.
        ''' </remarks>
        Public Property Total As Long?
    End Class ' BatchRequestCounts
    ''' <summary>
    ''' Represents <c>BatchErrors</c> in JSON.
    ''' </summary>
    Public Class BatchErrors
        ''' <summary>
        ''' Reads or writes <c>data</c> in json.
        ''' </summary>
        Public Property Data As IReadOnlyList(Of BatchError)
        ''' <summary>
        ''' 对象类型标识，固定值'error'
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>object</c> in json.
        ''' </remarks>
        Public Property TypeName As String
    End Class ' BatchErrors
    ''' <summary>
    ''' Represents <c>BatchStatus</c> in JSON.
    ''' </summary>
    Public Class BatchStatus
        ''' <summary>
        ''' 批处理的唯一标识符（UUID格式）
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>id</c> in json.
        ''' </remarks>
        Public Property Id As String
        ''' <summary>
        ''' 任务完成的时间窗口，固定值'24h'
        ''' </summary>
        ''' <value>
        ''' Can be value of <c>"24h"</c>
        ''' </value>
        ''' <remarks>
        ''' Reads or writes <c>completion_window</c> in json.
        ''' </remarks>
        Public Property CompletionWindow As String
        ''' <summary>
        ''' 批处理创建时间的Unix时间戳（秒级）
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>created_at</c> in json.
        ''' </remarks>
        Public Property CreatedAt As Long?
        ''' <summary>
        ''' 目标API端点，必须是指定的两个值之一
        ''' </summary>
        ''' <value>
        ''' Can be value of <c>"24h"</c>
        ''' </value>
        ''' <remarks>
        ''' Reads or writes <c>endpoint</c> in json.
        ''' </remarks>
        Public Property Endpoint As String
        ''' <summary>
        ''' 批处理输入文件的唯一标识符
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>input_file_id</c> in json.
        ''' </remarks>
        Public Property InputFileId As String
        ''' <summary>
        ''' 对象类型标识，固定值'batch'
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>object</c> in json.
        ''' </remarks>
        Public Property TypeName As String
        ''' <summary>
        ''' 当前状态（pending/in_progress/completed/cancelled/failed/expired）
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>status</c> in json.
        ''' </remarks>
        Public Property Status As String
        ''' <summary>
        ''' 批处理取消完成时间的Unix时间戳
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>cancelled_at</c> in json.
        ''' </remarks>
        Public Property CancelledAt As Long?
        ''' <summary>
        ''' 批处理开始取消操作的时间戳
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>cancelling_at</c> in json.
        ''' </remarks>
        Public Property CancellingAt As Long?
        ''' <summary>
        ''' 所有请求完成时间的时间戳
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>completed_at</c> in json.
        ''' </remarks>
        Public Property CompletedAt As Long?
        ''' <summary>
        ''' 包含错误信息的输出文件ID（当有失败时）
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>error_file_id</c> in json.
        ''' </remarks>
        Public Property ErrorFileId As String
        ''' <summary>
        ''' Reads or writes <c>errors</c> in json.
        ''' </summary>
        Public Property Errors As BatchErrors
        ''' <summary>
        ''' 批处理过期时间的时间戳
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>expired_at</c> in json.
        ''' </remarks>
        Public Property ExpiredAt As Long?
        ''' <summary>
        ''' 预设的到期时间（基于created_at+24小时）
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>expires_at</c> in json.
        ''' </remarks>
        Public Property ExpiresAt As Long?
        ''' <summary>
        ''' 批处理失败的时间戳
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>failed_at</c> in json.
        ''' </remarks>
        Public Property FailedAt As Long?
        ''' <summary>
        ''' 开始最终化操作的时间戳
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>finalizing_at</c> in json.
        ''' </remarks>
        Public Property FinalizingAt As Long?
        ''' <summary>
        ''' 批处理开始执行的时间戳
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>in_progress_at</c> in json.
        ''' </remarks>
        Public Property InProgressAt As Long?
        ''' <summary>
        ''' 用于存储与 Batch 相关的数据，如客户ID、描述或其他任务管理和跟踪所需的额外信息。每个键的长度最多为 64 个字符，值的长度最多为 512 个字符。
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>metadata</c> in json.
        ''' </remarks>
        Friend Property Metadata As IReadOnlyDictionary(Of String, String)
            Get
                Return JsonReadOnlyStringDictionary.Wrap(MetadataInternal)
            End Get
            Set(value As IReadOnlyDictionary(Of String, String))
                MetadataInternal = JsonReadOnlyStringDictionary.ToJsonObject(value)
            End Set
        End Property
        Friend Property MetadataInternal As JObject
        ''' <summary>
        ''' 包含成功请求结果的输出文件ID
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>output_file_id</c> in json.
        ''' </remarks>
        Public Property OutputFileId As String
        ''' <summary>
        ''' Reads or writes <c>request_counts</c> in json.
        ''' </summary>
        Public Property RequestCounts As BatchRequestCounts

        Public Shared Function FromJson(json As Stream) As BatchStatus
            Using jsonReader As New JsonTextReader(New StreamReader(json))
                jsonReader.DateParseHandling = DateParseHandling.None
                Return BatchResponseReader.ReadBatchStatus(jsonReader, JsonReadErrorHandler.DefaultHandler)
            End Using
        End Function

        Public Shared Function FromJson(json As String) As BatchStatus
            Using jsonReader As New JsonTextReader(New StringReader(json))
                jsonReader.DateParseHandling = DateParseHandling.None
                Return BatchResponseReader.ReadBatchStatus(jsonReader, JsonReadErrorHandler.DefaultHandler)
            End Using
        End Function
    End Class ' BatchStatus

    ''' <summary>
    ''' Represents <c>BatchPage</c> in JSON.
    ''' </summary>
    Public Class BatchPage
        ''' <summary>
        ''' 分页容器对象类型标识，固定值'list'
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>object</c> in json.
        ''' </remarks>
        Public Property TypeName As String
        ''' <summary>
        ''' Reads or writes <c>data</c> in json.
        ''' </summary>
        Public Property Data As IReadOnlyList(Of BatchStatus)

        Public Shared Function FromJson(json As Stream) As BatchPage
            Using jsonReader As New JsonTextReader(New StreamReader(json))
                jsonReader.DateParseHandling = DateParseHandling.None
                Return BatchResponseReader.ReadBatchPage(jsonReader, JsonReadErrorHandler.DefaultHandler)
            End Using
        End Function

        Public Shared Function FromJson(json As String) As BatchPage
            Using jsonReader As New JsonTextReader(New StringReader(json))
                jsonReader.DateParseHandling = DateParseHandling.None
                Return BatchResponseReader.ReadBatchPage(jsonReader, JsonReadErrorHandler.DefaultHandler)
            End Using
        End Function
    End Class ' BatchPage
End Namespace
