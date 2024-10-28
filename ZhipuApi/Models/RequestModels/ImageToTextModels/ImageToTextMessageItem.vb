Namespace ZhipuApi.Models.RequestModels.ImageToTextModels
	Public Class ImageToTextMessageItem
		Inherits MessageItem

		Public Sub New(role As String)
			MyBase.New(role, Nothing)
			content = New StringOrArray(Of ContentType) With {.ArrayValue = New ContentType(1) {}}
		End Sub

		Public Function setText(text As String) As ImageToTextMessageItem
			content.ArrayValue(0) = New ContentType With {.text = text, .type = "text"}
			Return Me
		End Function

		Public Function setImageUrl(image_url As String) As ImageToTextMessageItem
			content.ArrayValue(1) = New ContentType With {.type = "Image_url", .image_url = New ImageUrlType(image_url)}
			Return Me
		End Function
	End Class
End Namespace
