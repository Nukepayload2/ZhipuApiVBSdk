Imports System.IO
Imports System.Net.Http
Imports System.Text
Imports ZhipuApi.Models.RequestModels
Imports ZhipuApi.Models.ResponseModels.EmbeddingModels
Imports ZhipuApi.Utils

Namespace ZhipuApi.Modules
	Public Class Embeddings
		Private Const API_TOKEN_TTL_SECONDS = 300
		Private Shared ReadOnly client As New HttpClient
		Private ReadOnly _apiKey As String

		Public Sub New(apiKey As String)
			_apiKey = apiKey
		End Sub

		Private Iterator Function ProcessBase(requestBody As EmbeddingRequestBase) As IEnumerable(Of String)
			Dim json As String = requestBody?.ToJson
			Dim data As StringContent = New StringContent(json, Encoding.UTF8, "application/json")
			Dim api_key As String = AuthenticationUtils.GenerateToken(_apiKey, Embeddings.API_TOKEN_TTL_SECONDS)
			Dim request As New HttpRequestMessage() With {
				.Method = HttpMethod.Post,
				.RequestUri = New Uri("https://open.bigmodel.cn/api/paas/v4/embeddings"),
				.Content = data
			}
			request.Headers.Add("Authorization", api_key)
			Dim response As HttpResponseMessage = Embeddings.client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).Result
			Dim stream As Stream = response.Content.ReadAsStreamAsync().Result
			Dim buffer As Byte() = New Byte(8191) {}
			While True
				Dim num As Integer = stream.Read(buffer, 0, buffer.Length)
				Dim num2 As Integer = num
				Dim bytesRead As Integer = num
				If num2 <= 0 Then
					Exit While
				End If
				Yield Encoding.UTF8.GetString(buffer, 0, bytesRead)
			End While
			Return
		End Function

		Public Function Process(requestBody As EmbeddingRequestBase) As EmbeddingResponseBase
			Dim sb As StringBuilder = New StringBuilder()
			For Each str As String In ProcessBase(requestBody)
				sb.Append(str)
			Next
			Return EmbeddingResponseBase.FromJson(sb.ToString())
		End Function

	End Class
End Namespace
