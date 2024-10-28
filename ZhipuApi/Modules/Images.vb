Imports System.IO
Imports System.Net.Http
Imports System.Text
Imports ZhipuApi.Models.RequestModels
Imports ZhipuApi.Models.ResponseModels.ImageGenerationModels
Imports ZhipuApi.Utils

Namespace ZhipuApi.Modules
	Public Class Images
		Public Sub New(apiKey As String)
			Me._apiKey = apiKey
		End Sub

		Private Iterator Function GenerateBase(requestBody As ImageRequestBase) As IEnumerable(Of String)
			Dim json As String = requestBody?.ToJson()
			Dim data As StringContent = New StringContent(json, Encoding.UTF8, "application/json")
			Dim api_key As String = AuthenticationUtils.GenerateToken(Me._apiKey, Images.API_TOKEN_TTL_SECONDS)
			Dim request As New HttpRequestMessage() With {
				.Method = HttpMethod.Post,
				.RequestUri = New Uri("https://open.bigmodel.cn/api/paas/v4/images/generations"),
				.Content = data
			}
			request.Headers.Add("Authorization", api_key)
			Dim response As HttpResponseMessage = Images.client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).Result
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

		Public Function Generation(requestBody As ImageRequestBase) As ImageResponseBase
			Dim sb As StringBuilder = New StringBuilder()
			For Each str As String In Me.GenerateBase(requestBody)
				sb.Append(str)
			Next
			Return ImageResponseBase.FromJson(sb.ToString())
		End Function

		Private _apiKey As String

		Private Shared API_TOKEN_TTL_SECONDS As Integer = 300

		Private Shared client As New HttpClient
	End Class
End Namespace
