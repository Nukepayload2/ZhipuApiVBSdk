Imports System
Imports System.Collections.Generic
Imports System.Text.Json

Namespace ZhipuApi.Models.ResponseModels.ImageGenerationModels
	' Token: 0x0200000E RID: 14
	Public Class ImageResponseBase
		' Token: 0x17000018 RID: 24
		' (get) Token: 0x06000047 RID: 71 RVA: 0x00002692 File Offset: 0x00000892
		' (set) Token: 0x06000048 RID: 72 RVA: 0x0000269A File Offset: 0x0000089A
		Public Property created As Long

		' Token: 0x17000019 RID: 25
		' (get) Token: 0x06000049 RID: 73 RVA: 0x000026A3 File Offset: 0x000008A3
		' (set) Token: 0x0600004A RID: 74 RVA: 0x000026AB File Offset: 0x000008AB
		Public Property data As List(Of ImageResponseDataItem)

		' Token: 0x1700001A RID: 26
		' (get) Token: 0x0600004B RID: 75 RVA: 0x000026B4 File Offset: 0x000008B4
		' (set) Token: 0x0600004C RID: 76 RVA: 0x000026BC File Offset: 0x000008BC
		Public Property [error] As Dictionary(Of String, String)

		' Token: 0x0600004D RID: 77 RVA: 0x000026C8 File Offset: 0x000008C8
		Public Shared Function FromJson(json As String) As ImageResponseBase
			Return JsonSerializer.Deserialize(Of ImageResponseBase)(json, Nothing)
		End Function
	End Class
End Namespace
