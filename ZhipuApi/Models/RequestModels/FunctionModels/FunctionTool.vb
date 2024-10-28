Namespace ZhipuApi.Models.RequestModels.FunctionModels
	Public Class FunctionTool
		Public Property type As String = "function"

		' key: object property name is string
		' value: If JToken is String, then use StringValue. If JToken is object, use ObjectValue.
		Public Property [function] As New Dictionary(Of String, StringOrObject(Of FunctionParameters))

		Public Function SetName(name As String) As FunctionTool
			[function]!name = New StringOrObject(Of FunctionParameters) With {.StringValue = name}
			Return Me
		End Function

		Public Function SetDescription(desc As String) As FunctionTool
			[function]!description = New StringOrObject(Of FunctionParameters) With {.StringValue = desc}
			Return Me
		End Function

		Public Function SetParameters(param As FunctionParameters) As FunctionTool
			[function]!parameters = New StringOrObject(Of FunctionParameters) With {.ObjectValue = param}
			Return Me
		End Function
	End Class

	''' <summary>
	''' If JToken is String, then use StringValue. If JToken is object, use ObjectValue.
	''' </summary>
	Public Class StringOrObject(Of T)
		Public Property StringValue As String
		Public Property ObjectValue As T
	End Class
End Namespace
