Imports System

Namespace ZhipuApi.Models.RequestModels
	' Token: 0x02000013 RID: 19
	Public Class ImageRequestBase
		' Token: 0x17000026 RID: 38
		' (get) Token: 0x0600006C RID: 108 RVA: 0x0000281C File Offset: 0x00000A1C
		' (set) Token: 0x0600006D RID: 109 RVA: 0x00002824 File Offset: 0x00000A24
		Public Property model As String

		' Token: 0x17000027 RID: 39
		' (get) Token: 0x0600006E RID: 110 RVA: 0x0000282D File Offset: 0x00000A2D
		' (set) Token: 0x0600006F RID: 111 RVA: 0x00002835 File Offset: 0x00000A35
		Public Property prompt As String

		' Token: 0x06000070 RID: 112 RVA: 0x00002840 File Offset: 0x00000A40
		Public Function SetModel(model As String) As ImageRequestBase
			Me.model = model
			Return Me
		End Function

		' Token: 0x06000071 RID: 113 RVA: 0x0000285C File Offset: 0x00000A5C
		Public Function SetPrompt(prompt As String) As ImageRequestBase
			Me.prompt = prompt
			Return Me
		End Function
	End Class
End Namespace
