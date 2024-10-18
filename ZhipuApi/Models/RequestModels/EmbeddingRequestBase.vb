Imports System

Namespace ZhipuApi.Models.RequestModels
	' Token: 0x02000012 RID: 18
	Public Class EmbeddingRequestBase
		' Token: 0x17000024 RID: 36
		' (get) Token: 0x06000065 RID: 101 RVA: 0x000027BA File Offset: 0x000009BA
		' (set) Token: 0x06000066 RID: 102 RVA: 0x000027C2 File Offset: 0x000009C2
		Public Property model As String

		' Token: 0x17000025 RID: 37
		' (get) Token: 0x06000067 RID: 103 RVA: 0x000027CB File Offset: 0x000009CB
		' (set) Token: 0x06000068 RID: 104 RVA: 0x000027D3 File Offset: 0x000009D3
		Public Property input As String

		' Token: 0x06000069 RID: 105 RVA: 0x000027DC File Offset: 0x000009DC
		Public Function SetModel(model As String) As EmbeddingRequestBase
			Me.model = model
			Return Me
		End Function

		' Token: 0x0600006A RID: 106 RVA: 0x000027F8 File Offset: 0x000009F8
		Public Function SetInput(input As String) As EmbeddingRequestBase
			Me.input = input
			Return Me
		End Function
	End Class
End Namespace
