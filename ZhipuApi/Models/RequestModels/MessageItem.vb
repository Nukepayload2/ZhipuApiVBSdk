Imports System

Namespace ZhipuApi.Models.RequestModels
	' Token: 0x02000014 RID: 20
	Public Class MessageItem
		' Token: 0x17000028 RID: 40
		' (get) Token: 0x06000073 RID: 115 RVA: 0x00002880 File Offset: 0x00000A80
		' (set) Token: 0x06000074 RID: 116 RVA: 0x00002888 File Offset: 0x00000A88
		Public Property role As String

		' Token: 0x17000029 RID: 41
		' (get) Token: 0x06000075 RID: 117 RVA: 0x00002891 File Offset: 0x00000A91
		' (set) Token: 0x06000076 RID: 118 RVA: 0x00002899 File Offset: 0x00000A99
		Public Overridable Property content As String

		' Token: 0x06000077 RID: 119 RVA: 0x000028A2 File Offset: 0x00000AA2
		Public Sub New(role As String, content As String)
			Me.role = role
			Me.content = content
		End Sub
	End Class
End Namespace
