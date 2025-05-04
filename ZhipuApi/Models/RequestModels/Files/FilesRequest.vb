Namespace Models
    ''' <summary>
    ''' 上传文件
    ''' </summary>
    Public Class FileUploadRequest
        ''' <summary>
        ''' 要上传的文件的完整路径
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>file</c> in json.
        ''' </remarks>
        Public Property File As String
        ''' <summary>
        ''' 上传文件的用途
        ''' </summary>
        ''' <value>
        ''' Can be value of <c>"batch"</c>, <c>"retrieval"</c>, <c>"file-extract"</c>, <c>"code-interpreter"</c>, <c>"fine-tune"</c>, <c>"fine-tune-function-calling"</c>, <c>"fine-tune-vision-cogview"</c>, <c>"fine-tune-vision-cogvlm"</c>
        ''' </value>
        ''' <remarks>
        ''' Reads or writes <c>purpose</c> in json.
        ''' </remarks>
        Public Property Purpose As String
        ''' <summary>
        ''' 知识库ID（当purpose为retrieval时必填）
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>knowledge_id</c> in json.
        ''' </remarks>
        Public Property KnowledgeId As String
    End Class

    ''' <summary>
    ''' 删除文件
    ''' </summary>
    Public Class FileDeleteRequest
        ''' <summary>
        ''' 要删除的文件ID
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>file_id</c> in json.
        ''' </remarks>
        Public Property FileId As String
    End Class

    ''' <summary>
    ''' 查询文件列表
    ''' </summary>
    Public Class FilesListRequest
        ''' <summary>
        ''' 文件用途
        ''' </summary>
        ''' <value>
        ''' Can be value of <c>"batch"</c>, <c>"retrieval"</c>, <c>"file-extract"</c>, <c>"code-interpreter"</c>, <c>"fine-tune"</c>, <c>"fine-tune-function-calling"</c>, <c>"fine-tune-vision-cogview"</c>, <c>"fine-tune-vision-cogvlm"</c>
        ''' </value>
        ''' <remarks>
        ''' Reads or writes <c>purpose</c> in json.
        ''' </remarks>
        Public Property Purpose As String
        ''' <summary>
        ''' 知识库ID（当purpose为retrieval时必填）
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>knowledge_id</c> in json.
        ''' </remarks>
        Public Property KnowledgeId As String
        ''' <summary>
        ''' Reads or writes <c>page</c> in json.
        ''' </summary>
        ''' <value>
        ''' The default value is <c>1</c>
        ''' </value>
        Public Property Page As Long?
        ''' <summary>
        ''' Reads or writes <c>limit</c> in json.
        ''' </summary>
        ''' <value>
        ''' The default value is <c>10</c>
        ''' </value>
        Public Property Limit As Long?
        ''' <summary>
        ''' 查询指定fileID之后的文件（当purpose为fine-tune时需要）
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>after</c> in json.
        ''' </remarks>
        Public Property After As String
        ''' <summary>
        ''' Reads or writes <c>order</c> in json.
        ''' </summary>
        ''' <value>
        ''' Can be value of <c>"batch"</c>, <c>"retrieval"</c>, <c>"file-extract"</c>, <c>"code-interpreter"</c>, <c>"fine-tune"</c>, <c>"fine-tune-function-calling"</c>, <c>"fine-tune-vision-cogview"</c>, <c>"fine-tune-vision-cogvlm"</c><br/>
        '''The default value is <c>&quot;desc&quot;</c>
        ''' </value>
        Public Property Order As String
    End Class

    ''' <summary>
    ''' 知识库文件详情
    ''' </summary>
    Public Class DocumentRetrieveRequest
        ''' <summary>
        ''' 知识库文件ID
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>document_id</c> in json.
        ''' </remarks>
        Public Property DocumentId As String
    End Class

    ''' <summary>
    ''' 编辑知识库文件
    ''' </summary>
    Public Class DocumentEditRequest
        ''' <summary>
        ''' 知识库文件ID
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>document_id</c> in json.
        ''' </remarks>
        Public Property DocumentId As String
        ''' <summary>
        ''' Reads or writes <c>knowledge_type</c> in json.
        ''' </summary>
        Public Property KnowledgeType As Long?
        ''' <summary>
        ''' 自定义切片规则（knowledge_type=5时生效，默认['\n']）
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>custom_separator</c> in json.
        ''' </remarks>
        Public Property CustomSeparator As IReadOnlyList(Of String)
        ''' <summary>
        ''' Reads or writes <c>sentence_size</c> in json.
        ''' </summary>
        Public Property SentenceSize As Long?
    End Class
End Namespace
