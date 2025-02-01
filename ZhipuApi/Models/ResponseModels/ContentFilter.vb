Namespace Models
    Public Class ContentFilter

        ' 安全生效环节，包括 assistant、user 和 history
        Public Property Role As String

        ' 严重程度 level 0-3，level 0 表示最严重，3 表示轻微
        Public Property Level As Integer?

    End Class
End Namespace
