Namespace ZhipuApi.Models.RequestModels.ImageToTextModels
	Public Class ContentType
		Public Property type As String

		Public Property text As String

		Public Property image_url As ImageUrlType

		Public Function setImageUrl(image_url As String) As ContentType
			Me.image_url = New ImageUrlType(image_url)
			Return Me
		End Function
	End Class
End Namespace
