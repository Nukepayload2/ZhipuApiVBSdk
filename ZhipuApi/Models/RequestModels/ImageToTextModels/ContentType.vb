Imports System

Namespace ZhipuApi.Models.RequestModels.ImageToTextModels
	' Token: 0x02000016 RID: 22
	Public Class ContentType
		' Token: 0x17000032 RID: 50
		' (get) Token: 0x06000090 RID: 144 RVA: 0x00002A1B File Offset: 0x00000C1B
		' (set) Token: 0x06000091 RID: 145 RVA: 0x00002A23 File Offset: 0x00000C23
		Public Property type As String

		' Token: 0x17000033 RID: 51
		' (get) Token: 0x06000093 RID: 147 RVA: 0x00002A35 File Offset: 0x00000C35
		' (set) Token: 0x06000092 RID: 146 RVA: 0x00002A2C File Offset: 0x00000C2C
		Public Property text As String

		' Token: 0x17000034 RID: 52
		' (get) Token: 0x06000095 RID: 149 RVA: 0x00002A46 File Offset: 0x00000C46
		' (set) Token: 0x06000094 RID: 148 RVA: 0x00002A3D File Offset: 0x00000C3D
		Public Property image_url As ImageUrlType

		' Token: 0x06000096 RID: 150 RVA: 0x00002A50 File Offset: 0x00000C50
		Public Function setType(type As String) As ContentType
			Me.type = type
			Return Me
		End Function

		' Token: 0x06000097 RID: 151 RVA: 0x00002A6C File Offset: 0x00000C6C
		Public Function setText(text As String) As ContentType
			Me.text = text
			Return Me
		End Function

		' Token: 0x06000098 RID: 152 RVA: 0x00002A88 File Offset: 0x00000C88
		Public Function setImageUrl(image_url As String) As ContentType
			Me.image_url = New ImageUrlType(image_url)
			Return Me
		End Function
	End Class
End Namespace
