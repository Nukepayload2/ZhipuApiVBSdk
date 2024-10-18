Imports System
Imports ZhipuApi.Modules

Namespace ZhipuApi
	' Token: 0x02000002 RID: 2
	Public Class ClientV4
		' Token: 0x17000001 RID: 1
		' (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		' (set) Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
		Public Property chat As Chat

		' Token: 0x17000002 RID: 2
		' (get) Token: 0x06000003 RID: 3 RVA: 0x00002061 File Offset: 0x00000261
		' (set) Token: 0x06000004 RID: 4 RVA: 0x00002069 File Offset: 0x00000269
		Public Property images As Images

		' Token: 0x17000003 RID: 3
		' (get) Token: 0x06000005 RID: 5 RVA: 0x00002072 File Offset: 0x00000272
		' (set) Token: 0x06000006 RID: 6 RVA: 0x0000207A File Offset: 0x0000027A
		Public Property embeddings As Embeddings

		' Token: 0x06000007 RID: 7 RVA: 0x00002083 File Offset: 0x00000283
		Public Sub New(apiKey As String)
			Me._apiKey = apiKey
			Me.chat = New Chat(apiKey)
			Me.images = New Images(apiKey)
			Me.embeddings = New Embeddings(apiKey)
		End Sub

		' Token: 0x04000001 RID: 1
		Private _apiKey As String
	End Class
End Namespace
