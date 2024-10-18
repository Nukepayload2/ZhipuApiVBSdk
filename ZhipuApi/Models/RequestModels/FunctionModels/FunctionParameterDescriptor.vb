Imports System

Namespace ZhipuApi.Models.RequestModels.FunctionModels
	' Token: 0x0200001A RID: 26
	Public Class FunctionParameterDescriptor
		' Token: 0x17000037 RID: 55
		' (get) Token: 0x060000A2 RID: 162 RVA: 0x00002B5F File Offset: 0x00000D5F
		' (set) Token: 0x060000A3 RID: 163 RVA: 0x00002B67 File Offset: 0x00000D67
		Public Property type As String

		' Token: 0x17000038 RID: 56
		' (get) Token: 0x060000A4 RID: 164 RVA: 0x00002B70 File Offset: 0x00000D70
		' (set) Token: 0x060000A5 RID: 165 RVA: 0x00002B78 File Offset: 0x00000D78
		Public Property description As String

		' Token: 0x060000A6 RID: 166 RVA: 0x00002B84 File Offset: 0x00000D84
		Private Shared Function ToTypeString(type As ParameterType) As String
			If Not True Then
			End If
			Dim text As String
			If type <> ParameterType.[String] Then
				If type <> ParameterType.[Integer] Then
					text = Nothing
				Else
					text = "int"
				End If
			Else
				text = "string"
			End If
			If Not True Then
			End If
			Return text
		End Function

		' Token: 0x060000A7 RID: 167 RVA: 0x00002BBE File Offset: 0x00000DBE
		Public Sub New(type As ParameterType, description As String)
			Me.type = FunctionParameterDescriptor.ToTypeString(type)
			Me.description = description
		End Sub
	End Class
End Namespace
