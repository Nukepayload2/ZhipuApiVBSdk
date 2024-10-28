Imports ZhipuApi.Modules

Namespace ZhipuApi
	Public Class ClientV4
		Public Property chat As Chat

		Public Property images As Images

		Public Property embeddings As Embeddings

		Public Sub New(apiKey As String)
			Me._apiKey = apiKey
			Me.chat = New Chat(apiKey)
			Me.images = New Images(apiKey)
			Me.embeddings = New Embeddings(apiKey)
		End Sub

		Private _apiKey As String
	End Class
End Namespace
