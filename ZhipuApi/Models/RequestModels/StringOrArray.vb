Namespace Models
    ''' <summary>
    ''' If JToken is String, then use StringValue. If JToken is array, use ArrayValue.
    ''' </summary>
    Public Class StringOrArray(Of T)
        Public Property StringValue As String
        Public Property ArrayValue As IReadOnlyList(Of T)
    End Class
End Namespace
