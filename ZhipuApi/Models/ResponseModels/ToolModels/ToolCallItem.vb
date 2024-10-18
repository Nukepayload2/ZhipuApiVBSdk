Imports System

Namespace ZhipuApi.Models.ResponseModels.ToolModels
	' Token: 0x0200000D RID: 13
	Public Class ToolCallItem
		' Token: 0x17000014 RID: 20
		' (get) Token: 0x0600003E RID: 62 RVA: 0x00002645 File Offset: 0x00000845
		' (set) Token: 0x0600003F RID: 63 RVA: 0x0000264D File Offset: 0x0000084D
		Public Property id As String

		' Token: 0x17000015 RID: 21
		' (get) Token: 0x06000040 RID: 64 RVA: 0x00002656 File Offset: 0x00000856
		' (set) Token: 0x06000041 RID: 65 RVA: 0x0000265E File Offset: 0x0000085E
		Public Property [function] As FunctionDescriptor

		' Token: 0x17000016 RID: 22
		' (get) Token: 0x06000042 RID: 66 RVA: 0x00002667 File Offset: 0x00000867
		' (set) Token: 0x06000043 RID: 67 RVA: 0x0000266F File Offset: 0x0000086F
		Public Property index As Integer

		' Token: 0x17000017 RID: 23
		' (get) Token: 0x06000044 RID: 68 RVA: 0x00002678 File Offset: 0x00000878
		' (set) Token: 0x06000045 RID: 69 RVA: 0x00002680 File Offset: 0x00000880
		Public Property type As String
	End Class
End Namespace
