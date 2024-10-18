Imports System
Imports System.Collections.Generic
Imports System.Text.Json

Namespace ZhipuApi.Models.ResponseModels
	' Token: 0x02000009 RID: 9
	Public Class ResponseBase
		' Token: 0x17000004 RID: 4
		' (get) Token: 0x06000019 RID: 25 RVA: 0x000024DE File Offset: 0x000006DE
		' (set) Token: 0x0600001A RID: 26 RVA: 0x000024E6 File Offset: 0x000006E6
		Public Property id As String

		' Token: 0x17000005 RID: 5
		' (get) Token: 0x0600001B RID: 27 RVA: 0x000024EF File Offset: 0x000006EF
		' (set) Token: 0x0600001C RID: 28 RVA: 0x000024F7 File Offset: 0x000006F7
		Public Property request_id As String

		' Token: 0x17000006 RID: 6
		' (get) Token: 0x0600001D RID: 29 RVA: 0x00002500 File Offset: 0x00000700
		' (set) Token: 0x0600001E RID: 30 RVA: 0x00002508 File Offset: 0x00000708
		Public Property created As Long

		' Token: 0x17000007 RID: 7
		' (get) Token: 0x0600001F RID: 31 RVA: 0x00002511 File Offset: 0x00000711
		' (set) Token: 0x06000020 RID: 32 RVA: 0x00002519 File Offset: 0x00000719
		Public Property model As String

		' Token: 0x17000008 RID: 8
		' (get) Token: 0x06000021 RID: 33 RVA: 0x00002522 File Offset: 0x00000722
		' (set) Token: 0x06000022 RID: 34 RVA: 0x0000252A File Offset: 0x0000072A
		Public Property usage As Dictionary(Of String, Integer)

		' Token: 0x17000009 RID: 9
		' (get) Token: 0x06000023 RID: 35 RVA: 0x00002533 File Offset: 0x00000733
		' (set) Token: 0x06000024 RID: 36 RVA: 0x0000253B File Offset: 0x0000073B
		Public Property choices As ResponseChoiceItem()

		' Token: 0x1700000A RID: 10
		' (get) Token: 0x06000025 RID: 37 RVA: 0x00002544 File Offset: 0x00000744
		' (set) Token: 0x06000026 RID: 38 RVA: 0x0000254C File Offset: 0x0000074C
		Public Property [error] As Dictionary(Of String, String)

		' Token: 0x06000027 RID: 39 RVA: 0x00002558 File Offset: 0x00000758
		Public Shared Function FromJson(json As String) As ResponseBase
			Dim responseBase As ResponseBase
			Try
				responseBase = JsonSerializer.Deserialize(Of ResponseBase)(json, Nothing)
			Catch ex As JsonException
				responseBase = Nothing
			End Try
			Return responseBase
		End Function
	End Class
End Namespace
