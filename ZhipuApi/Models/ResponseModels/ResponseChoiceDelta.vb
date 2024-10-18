Imports System
Imports ZhipuApi.Models.ResponseModels.ToolModels

Namespace ZhipuApi.Models.ResponseModels
	' Token: 0x0200000A RID: 10
	Public Class ResponseChoiceDelta
		' Token: 0x1700000B RID: 11
		' (get) Token: 0x06000029 RID: 41 RVA: 0x00002591 File Offset: 0x00000791
		' (set) Token: 0x0600002A RID: 42 RVA: 0x00002599 File Offset: 0x00000799
		Public Property role As String

		' Token: 0x1700000C RID: 12
		' (get) Token: 0x0600002B RID: 43 RVA: 0x000025A2 File Offset: 0x000007A2
		' (set) Token: 0x0600002C RID: 44 RVA: 0x000025AA File Offset: 0x000007AA
		Public Property content As String

		' Token: 0x1700000D RID: 13
		' (get) Token: 0x0600002D RID: 45 RVA: 0x000025B3 File Offset: 0x000007B3
		' (set) Token: 0x0600002E RID: 46 RVA: 0x000025BB File Offset: 0x000007BB
		Public Property tool_calls As ToolCallItem()
	End Class
End Namespace
