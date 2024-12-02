Namespace Models
    Public Class ImageToTextMessageItem
        Inherits MessageItem

        Public Sub New(role As String)
            MyBase.New(role, Nothing)
            Content = New StringOrArray(Of ContentType) With {.ArrayValue = New ContentType(1) {}}
        End Sub

        Public Property Text As String
            Get
                Return Content.ArrayValue.FirstOrDefault?.Text
            End Get
            Set(text As String)
                Content.ArrayValue(0) = New ContentType With {.Text = text, .Type = "text"}
            End Set
        End Property

        Public Property ImageUrl As String
            Get
                If Content.ArrayValue.Length > 1 Then
                    Return Content.ArrayValue(1)?.ImageUrl?.Url
                End If
                Return Nothing
            End Get
            Set(imageUrl As String)
                Content.ArrayValue(1) = New ContentType With {.Type = "Image_url", .ImageUrl = New ImageUrlType(imageUrl)}
            End Set
        End Property
    End Class
End Namespace
