Namespace Models
    Public Class ImageToTextMessageItem
        Inherits MessageItem

        Public Sub New(role As String)
            MyBase.New(role, Nothing)
            Content = New StringOrArray(Of ContentType) With {.ArrayValue = New ContentType(1) {}}
        End Sub

        ''' <remarks>
        ''' This is a migration compatibility member for the official C# SDK. For more advanced cases, use <see cref="Content"/> property.
        ''' </remarks>
        Public Property Text As String
            Get
                Return Content.ArrayValue.FirstOrDefault?.Text
            End Get
            Set(text As String)
                Content.ArrayValue = {New ContentType With {.Text = text, .Type = "text"}, Content.ArrayValue(1)}
            End Set
        End Property

        ''' <remarks>
        ''' This is a migration compatibility member for the official C# SDK. For more advanced cases, use <see cref="Content"/> property.
        ''' </remarks>
        Public Property ImageUrl As String
            Get
                If Content.ArrayValue.Count > 1 Then
                    Return Content.ArrayValue(1)?.ImageUrl?.Url
                End If
                Return Nothing
            End Get
            Set(imageUrl As String)
                Content.ArrayValue = {Content.ArrayValue(0), New ContentType With {.Type = "Image_url", .ImageUrl = New ImageUrlType(imageUrl)}}
            End Set
        End Property
    End Class
End Namespace
