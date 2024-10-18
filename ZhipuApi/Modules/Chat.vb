Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Net.Http
Imports System.Text
Imports System.Text.Json
Imports System.Threading
Imports System.Threading.Tasks
Imports Newtonsoft.Json
Imports ZhipuApi.Models.RequestModels
Imports ZhipuApi.Models.ResponseModels
Imports ZhipuApi.Utils
Imports ZhipuApi.Utils.JsonResolver

Namespace ZhipuApi.Modules
	' Token: 0x02000006 RID: 6
	Public Class Chat
		' Token: 0x0600000C RID: 12 RVA: 0x00002245 File Offset: 0x00000445
		Public Sub New(apiKey As String)
			Me._apiKey = apiKey
		End Sub

		' Token: 0x0600000D RID: 13 RVA: 0x00002258 File Offset: 0x00000458
		Private Async Function CompletionUtf8Async(textRequestBody As TextRequestBase, yieldCallback As Action(Of ReadOnlyMemory(Of Byte)), Optional cancellationToken As CancellationToken = Nothing) As Task
			Dim settings As JsonSerializerSettings = New JsonSerializerSettings() With { .ContractResolver = New JsonResolver(), .Formatting = 1 }
			Dim json As String = JsonConvert.SerializeObject(textRequestBody, settings)
			Dim data As StringContent = New StringContent(json, Encoding.UTF8, "application/json")
			Dim api_key As String = AuthenticationUtils.GenerateToken(Me._apiKey, Chat.API_TOKEN_TTL_SECONDS)
			Dim request As HttpRequestMessage = New HttpRequestMessage() With { .Method = HttpMethod.Post, .RequestUri = New Uri("https://open.bigmodel.cn/api/paas/v4/chat/completions"), .Content = data, .Headers = { { "Authorization", api_key } } }
			Dim httpResponseMessage As HttpResponseMessage = Await Chat.client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
			Dim response As HttpResponseMessage = httpResponseMessage
			httpResponseMessage = Nothing
			Dim stream2 As Stream = Await response.Content.ReadAsStreamAsync(cancellationToken)
			Dim stream As Stream = stream2
			stream2 = Nothing
			Dim buffer As Byte() = New Byte(8191) {}
			While True
				Dim num As Integer = Await stream.ReadAsync(buffer, cancellationToken)
				Dim num2 As Integer = num
				Dim bytesRead As Integer = num2
				If num2 <= 0 Then
					Exit For
				End If
				cancellationToken.ThrowIfCancellationRequested()
				yieldCallback(buffer.AsMemory(0, bytesRead))
			End While
		End Function

		' Token: 0x0600000E RID: 14 RVA: 0x000022B4 File Offset: 0x000004B4
		Public Async Function CompletionAsync(textRequestBody As TextRequestBase, Optional cancellationToken As CancellationToken = Nothing) As Task(Of ResponseBase)
			textRequestBody.stream = False
			Dim ms As MemoryStream = New MemoryStream()
			Await Me.CompletionUtf8Async(textRequestBody, Sub(str As ReadOnlyMemory(Of Byte))
				cancellationToken.ThrowIfCancellationRequested()
				ms.Write(str.Span)
			End Sub, cancellationToken)
			ms.Position = 0L
			Return JsonSerializer.Deserialize(Of ResponseBase)(ms, Nothing)
		End Function

		' Token: 0x0600000F RID: 15 RVA: 0x00002308 File Offset: 0x00000508
		Public Async Function Stream(textRequestBody As TextRequestBase, yieldCallback As Action(Of ResponseBase)) As Task
			textRequestBody.stream = True
			Dim buffer As String = String.Empty
			Await Me.CompletionUtf8Async(textRequestBody, Sub(chunk As ReadOnlyMemory(Of Byte))
				buffer += Encoding.UTF8.GetString(chunk.Span)
				While True
					Dim startPos As Integer = buffer.IndexOf("data: ", StringComparison.Ordinal)
					Dim flag As Boolean = startPos = -1
					If flag Then
						Exit For
					End If
					Dim endPos As Integer = buffer.IndexOf(vbLf & vbLf, startPos, StringComparison.Ordinal)
					Dim flag2 As Boolean = endPos = -1
					If flag2 Then
						Exit For
					End If
					startPos += "data: ".Length
					Dim jsonString As String = buffer.Substring(startPos, endPos - startPos)
					Dim flag3 As Boolean = jsonString.Equals("[DONE]")
					If flag3 Then
						Exit For
					End If
					Dim response As ResponseBase = ResponseBase.FromJson(jsonString)
					Dim flag4 As Boolean = response IsNot Nothing
					If flag4 Then
						yieldCallback(response)
					End If
					buffer = buffer.Substring(endPos + vbLf & vbLf.Length)
				End While
			End Sub, Nothing)
			Dim finalResponse As ResponseBase = ResponseBase.FromJson(buffer.Trim())
			If finalResponse IsNot Nothing Then
				yieldCallback(finalResponse)
			End If
		End Function

		' Token: 0x04000008 RID: 8
		Private _apiKey As String

		' Token: 0x04000009 RID: 9
		Private Shared API_TOKEN_TTL_SECONDS As Integer = 300

		' Token: 0x0400000A RID: 10
		Private Shared client As HttpClient = New HttpClient()

		' Token: 0x0400000B RID: 11
		Private Shared PORTAL_URLS As Dictionary(Of ModelPortal, String) = New Dictionary(Of ModelPortal, String)() From { { ModelPortal.Regular, "https://open.bigmodel.cn/api/paas/v4/chat/completions" } }
	End Class
End Namespace
