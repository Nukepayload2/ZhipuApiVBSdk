﻿Imports Newtonsoft.Json
Imports System.IO
Imports Newtonsoft.Json.Linq
Imports Nukepayload2.AI.Providers.Zhipu.Serialization
Imports Nukepayload2.AI.Providers.Zhipu.Utils

Namespace Models
    ''' <summary>
    ''' 创建批量请求
    ''' </summary>
    Public Class BatchCreateRequest
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
        ''' 用于存储与 Batch 相关的数据，如客户ID、描述或其他任务管理和跟踪所需的额外信息。每个键的长度最多为 64 个字符，值的长度最多为 512 个字符。
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>metadata</c> in json.
        ''' </remarks>
        Public Property Metadata As IReadOnlyDictionary(Of String, String)
            Get
                Return JsonReadOnlyStringDictionary.Wrap(MetadataInternal)
            End Get
            Set(value As IReadOnlyDictionary(Of String, String))
                MetadataInternal = JsonReadOnlyStringDictionary.ToJsonObject(value)
            End Set
        End Property
        Friend Property MetadataInternal As JObject

        Public Function ToJsonUtf8() As MemoryStream
            Dim ms As New MemoryStream
            Using sw As New StreamWriter(ms, IoUtils.UTF8NoBOM, 8192, True), jsonWriter As New JsonTextWriter(sw)
                BatchRequestWriter.WriteBatchCreateParams(jsonWriter, Me)
            End Using
            ms.Position = 0
            Return ms
        End Function

        Public Function ToJson() As String
            Using stringWriter = New StringWriter, jsonWriter = New JsonTextWriter(stringWriter)
                BatchRequestWriter.WriteBatchCreateParams(jsonWriter, Me)
                Return stringWriter.ToString()
            End Using
        End Function

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