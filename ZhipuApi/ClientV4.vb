Imports System.Net.Http

Public Class ClientV4
	Friend Shared ReadOnly Property DefaultHttpClient As New HttpClient

	''' <summary>
	''' 与语言模型对话。一般而言，这是最常用的功能。
	''' </summary>
	Public Property Chat As Chat

	''' <summary>
	''' 生成图片
	''' </summary>
	Public Property Images As Images

	''' <summary>
	''' 生成文本的向量检索键
	''' </summary>
	Public Property Embeddings As Embeddings

	''' <summary>
	''' 管理用于模型微调、知识库、Batch、文件抽取等功能所使用的文件
	''' </summary>
	Public Property Files As Files

	''' <summary>
	''' 专为处理大规模数据请求而设计，适用于无需即时反馈的任务。开发者可以通过文件提交大量任务。
	''' </summary>
	Public Property Batches As Batches

	Sub New(apiKey As String)
		Chat = New Chat(apiKey)
		Images = New Images(apiKey)
		Embeddings = New Embeddings(apiKey)
		Files = New Files(apiKey)
		Batches = New Batches(apiKey)
	End Sub

	Sub New(apiKey As String, client As HttpClient)
		Chat = New Chat(apiKey, client)
		Images = New Images(apiKey, client)
		Embeddings = New Embeddings(apiKey, client)
		Files = New Files(apiKey, client)
		Batches = New Batches(apiKey, client)
	End Sub

End Class
