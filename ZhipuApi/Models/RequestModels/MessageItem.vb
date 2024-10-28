Imports ZhipuApi.Models.RequestModels.ImageToTextModels

Namespace ZhipuApi.Models.RequestModels
    Public Class MessageItem
        Public Property role As String

        ' If JToken is String, then use StringValue. If JToken is array, use ArrayValue.
        Public Property content As StringOrArray(Of ContentType)

        Public Sub New(role As String, content As String)
            Me.role = role
            Me.content = New StringOrArray(Of ContentType) With {.StringValue = content}
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
