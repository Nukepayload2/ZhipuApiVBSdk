Imports System

Namespace ZhipuApi.Models.ResponseModels.EmbeddingModels
	' Token: 0x02000010 RID: 16
	Public Class EmbeddingDataItem
		' Token: 0x1700001C RID: 28
		' (get) Token: 0x06000052 RID: 82 RVA: 0x00002704 File Offset: 0x00000904
		' (set) Token: 0x06000053 RID: 83 RVA: 0x0000270C File Offset: 0x0000090C
		Public Property index As Integer

		' Token: 0x1700001D RID: 29
		' (get) Token: 0x06000054 RID: 84 RVA: 0x00002715 File Offset: 0x00000915
		' (set) Token: 0x06000055 RID: 85 RVA: 0x0000271D File Offset: 0x0000091D
		Public Property _object As String

		' Token: 0x1700001E RID: 30
		' (get) Token: 0x06000056 RID: 86 RVA: 0x00002726 File Offset: 0x00000926
		' (set) Token: 0x06000057 RID: 87 RVA: 0x0000272E File Offset: 0x0000092E
		Public Property embedding As Double()
	End Class
End Namespace
