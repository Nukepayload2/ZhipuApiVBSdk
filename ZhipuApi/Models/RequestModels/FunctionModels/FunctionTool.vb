Imports System
Imports System.Collections.Generic

Namespace ZhipuApi.Models.RequestModels.FunctionModels
	' Token: 0x0200001C RID: 28
	Public Class FunctionTool
		' Token: 0x1700003C RID: 60
		' (get) Token: 0x060000B0 RID: 176 RVA: 0x00002C6B File Offset: 0x00000E6B
		' (set) Token: 0x060000B1 RID: 177 RVA: 0x00002C73 File Offset: 0x00000E73
		Public Property type As String

		' Token: 0x1700003D RID: 61
		' (get) Token: 0x060000B2 RID: 178 RVA: 0x00002C7C File Offset: 0x00000E7C
		' (set) Token: 0x060000B3 RID: 179 RVA: 0x00002C84 File Offset: 0x00000E84
		Public Property [function] As Dictionary(Of String, Object)

		' Token: 0x060000B4 RID: 180 RVA: 0x00002C8D File Offset: 0x00000E8D
		Public Sub New()
			Me.type = "function"
			Me.[function] = New Dictionary(Of String, Object)()
		End Sub

		' Token: 0x060000B5 RID: 181 RVA: 0x00002CB0 File Offset: 0x00000EB0
		Public Function SetName(name As String) As FunctionTool
			Me.[function]("name") = name
			Return Me
		End Function

		' Token: 0x060000B6 RID: 182 RVA: 0x00002CD8 File Offset: 0x00000ED8
		Public Function SetDescription(desc As String) As FunctionTool
			Me.[function]("description") = desc
			Return Me
		End Function

		' Token: 0x060000B7 RID: 183 RVA: 0x00002D00 File Offset: 0x00000F00
		Public Function SetParameters(param As FunctionParameters) As FunctionTool
			Me.[function]("parameters") = param
			Return Me
		End Function
	End Class
End Namespace
