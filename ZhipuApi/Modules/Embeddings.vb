Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Net.Http
Imports System.Text
Imports System.Text.Json
Imports ZhipuApi.Models.RequestModels
Imports ZhipuApi.Models.ResponseModels.EmbeddingModels
Imports ZhipuApi.Utils

Namespace ZhipuApi.Modules
	' Token: 0x02000007 RID: 7
	Public Class Embeddings
		' Token: 0x06000011 RID: 17 RVA: 0x00002387 File Offset: 0x00000587
		Public Sub New(apiKey As String)
			Me._apiKey = apiKey
		End Sub

		' Token: 0x06000012 RID: 18 RVA: 0x00002398 File Offset: 0x00000598
		Private Iterator Function ProcessBase(requestBody As EmbeddingRequestBase) As IEnumerable(Of String)
			Dim json As String = JsonSerializer.Serialize(Of EmbeddingRequestBase)(requestBody, Nothing)
			Dim data As StringContent = New StringContent(json, Encoding.UTF8, "application/json")
			Dim api_key As String = AuthenticationUtils.GenerateToken(Me._apiKey, Embeddings.API_TOKEN_TTL_SECONDS)
			Dim request As HttpRequestMessage = New HttpRequestMessage() With { .Method = HttpMethod.Post, .RequestUri = New Uri("https://open.bigmodel.cn/api/paas/v4/embeddings"), .Content = data, .Headers = { { "Authorization", api_key } } }
			Dim response As HttpResponseMessage = Embeddings.client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).Result
			Dim stream As Stream = response.Content.ReadAsStreamAsync().Result
			Dim buffer As Byte() = New Byte(8191) {}
			While True
				Dim num As Integer = stream.Read(buffer, 0, buffer.Length)
				Dim num2 As Integer = num
				Dim bytesRead As Integer = num
				If num2 <= 0 Then
					Exit For
				End If
				Yield Encoding.UTF8.GetString(buffer, 0, bytesRead)
			End While
			Return
		End Function

		' Token: 0x06000013 RID: 19 RVA: 0x000023B0 File Offset: 0x000005B0
		Public Function Process(requestBody As EmbeddingRequestBase) As EmbeddingResponseBase
			Dim sb As StringBuilder = New StringBuilder()
			For Each str As String In Me.ProcessBase(requestBody)
				sb.Append(str)
			Next
			Return EmbeddingResponseBase.FromJson(sb.ToString())
		End Function

		' Token: 0x0400000C RID: 12
		Private _apiKey As String

		' Token: 0x0400000D RID: 13
		Private Shared API_TOKEN_TTL_SECONDS As Integer = 300

		' Token: 0x0400000E RID: 14
		Private Shared client As HttpClient = New HttpClient()
	End Class
End Namespace
