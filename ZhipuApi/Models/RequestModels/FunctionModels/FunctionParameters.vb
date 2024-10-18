Imports System
Imports System.Collections.Generic

Namespace ZhipuApi.Models.RequestModels.FunctionModels
	' Token: 0x0200001B RID: 27
	Public Class FunctionParameters
		' Token: 0x17000039 RID: 57
		' (get) Token: 0x060000A8 RID: 168 RVA: 0x00002BDD File Offset: 0x00000DDD
		' (set) Token: 0x060000A9 RID: 169 RVA: 0x00002BE5 File Offset: 0x00000DE5
		Public Property type As String

		' Token: 0x1700003A RID: 58
		' (get) Token: 0x060000AA RID: 170 RVA: 0x00002BEE File Offset: 0x00000DEE
		Public ReadOnly Property properties As Dictionary(Of String, FunctionParameterDescriptor)

		' Token: 0x1700003B RID: 59
		' (get) Token: 0x060000AB RID: 171 RVA: 0x00002BF6 File Offset: 0x00000DF6
		' (set) Token: 0x060000AC RID: 172 RVA: 0x00002BFE File Offset: 0x00000DFE
		Public Property required As String()

		' Token: 0x060000AD RID: 173 RVA: 0x00002C07 File Offset: 0x00000E07
		Public Sub New()
			Me.type = "object"
			Me.properties = New Dictionary(Of String, FunctionParameterDescriptor)()
		End Sub

		' Token: 0x060000AE RID: 174 RVA: 0x00002C28 File Offset: 0x00000E28
		Public Function AddParameter(name As String, type As ParameterType, description As String) As FunctionParameters
			Me.properties(name) = New FunctionParameterDescriptor(type, description)
			Return Me
		End Function

		' Token: 0x060000AF RID: 175 RVA: 0x00002C50 File Offset: 0x00000E50
		Public Function SetRequiredParameter(required As String()) As FunctionParameters
			Me.required = required
			Return Me
		End Function
	End Class
End Namespace
