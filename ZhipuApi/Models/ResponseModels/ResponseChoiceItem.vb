Imports System

Namespace ZhipuApi.Models.ResponseModels
	' Token: 0x0200000B RID: 11
	Public Class ResponseChoiceItem
		' Token: 0x1700000E RID: 14
		' (get) Token: 0x06000030 RID: 48 RVA: 0x000025CD File Offset: 0x000007CD
		' (set) Token: 0x06000031 RID: 49 RVA: 0x000025D5 File Offset: 0x000007D5
		Public Property finish_reason As String

		' Token: 0x1700000F RID: 15
		' (get) Token: 0x06000032 RID: 50 RVA: 0x000025DE File Offset: 0x000007DE
		' (set) Token: 0x06000033 RID: 51 RVA: 0x000025E6 File Offset: 0x000007E6
		Public Property index As Integer

		' Token: 0x17000010 RID: 16
		' (get) Token: 0x06000034 RID: 52 RVA: 0x000025EF File Offset: 0x000007EF
		' (set) Token: 0x06000035 RID: 53 RVA: 0x000025F7 File Offset: 0x000007F7
		Public Property message As ResponseChoiceDelta

		' Token: 0x17000011 RID: 17
		' (get) Token: 0x06000036 RID: 54 RVA: 0x00002600 File Offset: 0x00000800
		' (set) Token: 0x06000037 RID: 55 RVA: 0x00002608 File Offset: 0x00000808
		Public Property delta As ResponseChoiceDelta
	End Class
End Namespace
