
Namespace ZhipuApi.Models.RequestModels.FunctionModels
	Public Class FunctionParameterDescriptor
		Public Property type As String

		Public Property description As String

		Private Shared Function ToTypeString(type As ParameterType) As String
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
			Return text
		End Function
		Sub New()

		End Sub
		Public Sub New(type As ParameterType, description As String)
			Me.type = FunctionParameterDescriptor.ToTypeString(type)
			Me.description = description
		End Sub
	End Class
End Namespace
