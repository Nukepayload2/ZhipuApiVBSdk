Namespace Models

	' 丐板的 JSchema (https://github.com/JamesNK/Newtonsoft.Json.Schema/blob/master/Src/Newtonsoft.Json.Schema/JSchema.cs)
	' 那个类型许可证太严格了不能用。

	Public Class FunctionParameterDescriptor
		Public Property Type As String

		Public Property Description As String

		Public Property [Default] As Object

		Public Property [Enum] As Object()

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

		Sub New(type As String, description As String)
			Me.Type = type
			Me.Description = description
		End Sub
	End Class
End Namespace
