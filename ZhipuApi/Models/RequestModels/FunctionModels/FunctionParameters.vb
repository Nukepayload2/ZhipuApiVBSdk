Namespace Models
	Public Class FunctionParameters
		Public Property Type As String = "object"

		Public ReadOnly Property Properties As New Dictionary(Of String, FunctionParameterDescriptor)

		Public Property Required As String()

		Public Function AddParameter(name As String, type As ParameterType, description As String) As FunctionParameters
			Properties(name) = New FunctionParameterDescriptor(type, description)
			Return Me
		End Function

		Public Function SetRequiredParameter(required As String()) As FunctionParameters
			Me.Required = required
			Return Me
		End Function
	End Class
End Namespace
