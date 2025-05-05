Imports System.IO

Namespace Models
    ''' <summary>
    ''' Represents <c>FileUploadRequest</c> in JSON.
    ''' </summary>
    Public Class FileUploadRequest
        ''' <summary>
        ''' 要上传的文件名
        ''' </summary>
        Public Property FileName As String
        ''' <summary>
        ''' 要上传的文件的内容
        ''' </summary>
        Public Property FileContent As Stream
        ''' <summary>
        ''' 上传文件的用途
        ''' </summary>
        ''' <value>
        ''' Can be value of <c>"batch"</c>, <c>"retrieval"</c>, <c>"file-extract"</c>, <c>"code-interpreter"</c>, <c>"fine-tune"</c>, <c>"fine-tune-function-calling"</c>, <c>"fine-tune-vision-cogview"</c>, <c>"fine-tune-vision-cogvlm"</c>
        ''' </value>
        Public Property Purpose As String
        ''' <summary>
        ''' 知识库ID（当purpose为retrieval时必填）
        ''' </summary>
        Public Property KnowledgeId As String
    End Class ' FileUploadRequest

    ''' <summary>
    ''' Represents <c>FileDeleteRequest</c> in JSON.
    ''' </summary>
    Public Class FileDeleteRequest
        ''' <summary>
        ''' 要删除的文件ID
        ''' </summary>
        Public Property FileId As String
    End Class ' FileDeleteRequest

    ''' <summary>
    ''' 查询文件列表
    ''' </summary>
    Public Class FilesListRequest
        ''' <summary>
        ''' 文件用途，支持batch、file-extract、fine-tune
        ''' </summary>
        Public Property Purpose As String
        ''' <summary>
        ''' 知识库ID（当purpose为retrieval时必填）
        ''' </summary>
        Public Property KnowledgeId As String
        ''' <summary>
        ''' 分页时的页码
        ''' </summary>
        ''' <value>
        ''' The default value is <c>1</c>
        ''' </value>
        Public Property Page As Long?
        ''' <summary>
        ''' 最大返回数量
        ''' </summary>
        ''' <value>
        ''' The default value is <c>10</c>
        ''' </value>
        Public Property Limit As Long?
        ''' <summary>
        ''' 查询指定fileID之后的文件列表（当文件用途为 fine-tune 时需要）
        ''' </summary>
        ''' <remarks>
        ''' Reads or writes <c>after</c> in json.
        ''' </remarks>
        Public Property After As String
        ''' <summary>
        ''' 排序规则，可选值[desc，asc]，默认desc（当文件用途为 fine-tune 时需要）
        ''' </summary>
        Public Property Order As String
    End Class ' FilesListRequest
End Namespace
