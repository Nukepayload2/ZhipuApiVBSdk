Public Class ClientV4
	Public Property Chat As Chat

	Public Property Images As Images

	Public Property Embeddings As Embeddings

	Sub New(apiKey As String)
		Chat = New Chat(apiKey)
		Images = New Images(apiKey)
		Embeddings = New Embeddings(apiKey)
	End Sub

End Class
