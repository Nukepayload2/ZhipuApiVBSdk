Namespace Models
    ''' <summary>
    ''' 表示网络搜索操作的元数据。
    ''' </summary>
    Public Class WebSearchMetadata
        ''' <summary>
        ''' 获取或设置一个值，表示是否启用网络搜索。
        ''' </summary>
        ''' <value>
        ''' 如果启用网络搜索，则为 <c>true</c>；否则为 <c>false</c>。
        ''' </value>
        Public Property IsEnabled As Boolean?

        ''' <summary>
        ''' 获取或设置自定义搜索查询内容。
        ''' </summary>
        ''' <value>
        ''' 自定义搜索查询内容。
        ''' </value>
        Public Property SearchQuery As String

        ''' <summary>
        ''' 获取或设置搜索引擎输出结果的提示语。
        ''' </summary>
        ''' <value>
        ''' 搜索引擎输出结果的提示语。
        ''' </value>
        Public Property SearchPrompt As String

        ''' <summary>
        ''' 获取或设置一个值，表示是否返回获取网页搜索来源的详细信息。
        ''' </summary>
        ''' <value>
        ''' 如果需要返回详细信息，则为 <c>true</c>；否则为 <c>false</c>。
        ''' </value>
        Public Property SearchResult As Boolean?

        ''' <summary>
        ''' 获取或设置搜索引擎类型。
        ''' </summary>
        ''' <value>
        ''' 搜索引擎类型。
        ''' </value>
        Public Property SearchEngine As String

        ''' <summary>
        ''' 获取或设置搜索结果在响应中的顺序。
        ''' </summary>
        ''' <value>
        ''' 搜索结果的顺序，可以是 "before" 或 "after"。
        ''' </value>
        Public Property ResultSequence As String

        ''' <summary>
        ''' 获取或设置一个值，表示是否必须在响应前获得搜索结果。
        ''' </summary>
        ''' <value>
        ''' 如果必须在响应前获得搜索结果，则为 <c>true</c>；否则为 <c>false</c>。
        ''' </value>
        Public Property RequireSearch As Boolean?
    End Class


End Namespace
