Imports System.Net.Http

Public Class ClientV4
	Friend Shared ReadOnly Property DefaultHttpClient As New HttpClient

	Public Property Chat As Chat

	Public Property Images As Images

	Public Property Embeddings As Embeddings

	Sub New(apiKey As String)
		Chat = New Chat(apiKey)
		Images = New Images(apiKey)
		Embeddings = New Embeddings(apiKey)
	End Sub

	Sub New(apiKey As String, client As HttpClient)
		Chat = New Chat(apiKey, client)
		Images = New Images(apiKey, client)
		Embeddings = New Embeddings(apiKey, client)
	End Sub

End Class
