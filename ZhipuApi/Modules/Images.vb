Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Net.Http
Imports System.Text
Imports System.Text.Json
Imports ZhipuApi.Models.RequestModels
Imports ZhipuApi.Models.ResponseModels.ImageGenerationModels
Imports ZhipuApi.Utils

Namespace ZhipuApi.Modules
	' Token: 0x02000008 RID: 8
	Public Class Images
		' Token: 0x06000015 RID: 21 RVA: 0x00002432 File Offset: 0x00000632
		Public Sub New(apiKey As String)
			Me._apiKey = apiKey
		End Sub

		' Token: 0x06000016 RID: 22 RVA: 0x00002443 File Offset: 0x00000643
		Private Iterator Function GenerateBase(requestBody As ImageRequestBase) As IEnumerable(Of String)
			Dim json As String = JsonSerializer.Serialize(Of ImageRequestBase)(requestBody, Nothing)
			Dim data As StringContent = New StringContent(json, Encoding.UTF8, "application/json")
			Dim api_key As String = AuthenticationUtils.GenerateToken(Me._apiKey, Images.API_TOKEN_TTL_SECONDS)
			Dim request As HttpRequestMessage = New HttpRequestMessage() With { .Method = HttpMethod.Post, .RequestUri = New Uri("https://open.bigmodel.cn/api/paas/v4/images/generations"), .Content = data, .Headers = { { "Authorization", api_key } } }
			Dim response As HttpResponseMessage = Images.client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).Result
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

		' Token: 0x06000017 RID: 23 RVA: 0x0000245C File Offset: 0x0000065C
		Public Function Generation(requestBody As ImageRequestBase) As ImageResponseBase
			Dim sb As StringBuilder = New StringBuilder()
			For Each str As String In Me.GenerateBase(requestBody)
				sb.Append(str)
			Next
			Return ImageResponseBase.FromJson(sb.ToString())
		End Function

		' Token: 0x0400000F RID: 15
		Private _apiKey As String

		' Token: 0x04000010 RID: 16
		Private Shared API_TOKEN_TTL_SECONDS As Integer = 300

		' Token: 0x04000011 RID: 17
		Private Shared client As HttpClient = New HttpClient()
	End Class
End Namespace
