Imports System
Imports System.Collections.Generic
Imports System.Text.Json

Namespace ZhipuApi.Models.ResponseModels.EmbeddingModels
	' Token: 0x02000011 RID: 17
	Public Class EmbeddingResponseBase
		' Token: 0x1700001F RID: 31
		' (get) Token: 0x0600005A RID: 90 RVA: 0x00002749 File Offset: 0x00000949
		' (set) Token: 0x06000059 RID: 89 RVA: 0x00002740 File Offset: 0x00000940
		Public Property model As String

		' Token: 0x17000020 RID: 32
		' (get) Token: 0x0600005C RID: 92 RVA: 0x0000275A File Offset: 0x0000095A
		' (set) Token: 0x0600005B RID: 91 RVA: 0x00002751 File Offset: 0x00000951
		Public Property _object As String

		' Token: 0x17000021 RID: 33
		' (get) Token: 0x0600005E RID: 94 RVA: 0x0000276B File Offset: 0x0000096B
		' (set) Token: 0x0600005D RID: 93 RVA: 0x00002762 File Offset: 0x00000962
		Public Property usage As Dictionary(Of String, Integer)

		' Token: 0x17000022 RID: 34
		' (get) Token: 0x0600005F RID: 95 RVA: 0x00002773 File Offset: 0x00000973
		' (set) Token: 0x06000060 RID: 96 RVA: 0x0000277B File Offset: 0x0000097B
		Public Property data As EmbeddingDataItem()

		' Token: 0x17000023 RID: 35
		' (get) Token: 0x06000061 RID: 97 RVA: 0x00002784 File Offset: 0x00000984
		' (set) Token: 0x06000062 RID: 98 RVA: 0x0000278C File Offset: 0x0000098C
		Public Property [error] As Dictionary(Of String, String)

		' Token: 0x06000063 RID: 99 RVA: 0x00002798 File Offset: 0x00000998
		Public Shared Function FromJson(json As String) As EmbeddingResponseBase
			Return JsonSerializer.Deserialize(Of EmbeddingResponseBase)(json, Nothing)
		End Function
	End Class
End Namespace
