Namespace ZhipuApi.Models.RequestModels.FunctionModels
	Public Class FunctionParameters
		Public Property type As String

		Public ReadOnly Property properties As Dictionary(Of String, FunctionParameterDescriptor)

		Public Property required As String()

		Public Sub New()
			Me.type = "object"
			Me.properties = New Dictionary(Of String, FunctionParameterDescriptor)()
		End Sub

		Public Function AddParameter(name As String, type As ParameterType, description As String) As FunctionParameters
			Me.properties(name) = New FunctionParameterDescriptor(type, description)
			Return Me
		End Function

		Public Function SetRequiredParameter(required As String()) As FunctionParameters
			Me.required = required
			Return Me
		End Function
	End Class
End Namespace
