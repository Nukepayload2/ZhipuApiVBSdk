Namespace Models
    Public Class MessageItem
        Public Property Role As String

        ' If JToken is String, then use StringValue. If JToken is array, use ArrayValue.
        Public Property Content As StringOrArray(Of ContentType)

        Public Property ToolCallId As String

        Sub New()

        End Sub

        Sub New(role As String, content As String)
            Me.Role = role
            Me.Content = New StringOrArray(Of ContentType) With {.StringValue = content}
        End Sub
    End Class

    ''' <summary>
    ''' If JToken is String, then use StringValue. If JToken is array, use ArrayValue.
    ''' </summary>
    Public Class StringOrArray(Of T)
        Public Property StringValue As String
        Public Property ArrayValue As T()
    End Class
End Namespace
