Namespace Models
	Public Class FunctionParameterDescriptor
		Public Property Type As String

		Public Property Description As String

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

		Sub New(type As ParameterType, description As String)
			Me.Type = ToTypeString(type)
			Me.Description = description
		End Sub
	End Class
End Namespace
