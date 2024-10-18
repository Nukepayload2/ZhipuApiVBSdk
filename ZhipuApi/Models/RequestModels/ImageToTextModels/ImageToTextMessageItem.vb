Imports System

Namespace ZhipuApi.Models.RequestModels.ImageToTextModels
	' Token: 0x02000017 RID: 23
	Public Class ImageToTextMessageItem
		Inherits MessageItem

		' Token: 0x17000035 RID: 53
		' (get) Token: 0x0600009A RID: 154 RVA: 0x00002AB1 File Offset: 0x00000CB1
		' (set) Token: 0x0600009B RID: 155 RVA: 0x00002AB9 File Offset: 0x00000CB9
		Public Property content As ContentType()

		' Token: 0x0600009C RID: 156 RVA: 0x00002AC2 File Offset: 0x00000CC2
		Public Sub New(role As String)
			MyBase.New(role, Nothing)
			Me.content = New ContentType(1) {}
		End Sub

		' Token: 0x0600009D RID: 157 RVA: 0x00002ADC File Offset: 0x00000CDC
		Public Function setText(text As String) As ImageToTextMessageItem
			Me.content(0) = New ContentType().setType("text").setText(text)
			Return Me
		End Function

		' Token: 0x0600009E RID: 158 RVA: 0x00002B0C File Offset: 0x00000D0C
		Public Function setImageUrl(image_url As String) As ImageToTextMessageItem
			Me.content(1) = New ContentType().setType("Image_url").setImageUrl(image_url)
			Return Me
		End Function
	End Class
End Namespace
