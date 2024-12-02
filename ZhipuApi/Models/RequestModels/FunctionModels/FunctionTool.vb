Namespace ZhipuApi.Models.RequestModels.FunctionModels
	Public Class FunctionTool
		Public Property Type As String = "function"

		' key: object property name is string
		' value: If JToken is String, then use StringValue. If JToken is object, use ObjectValue.
		Public ReadOnly Property [Function] As New Dictionary(Of String, StringOrObject(Of FunctionParameters))

		Public Property Name As String
			Get
				Return [Function]!name.StringValue
			End Get
			Set(name As String)
				[Function]!name = New StringOrObject(Of FunctionParameters) With {.StringValue = name}
			End Set
		End Property

		Public Property Description As String
			Get
				Return [Function]!description.StringValue
			End Get
			Set(desc As String)
				[Function]!description = New StringOrObject(Of FunctionParameters) With {.StringValue = desc}
			End Set
		End Property

		Public Property Parameters As FunctionParameters
			Get
				Return [Function]!parameters.ObjectValue
			End Get
			Set(param As FunctionParameters)
				[Function]!parameters = New StringOrObject(Of FunctionParameters) With {.ObjectValue = param}
			End Set
		End Property
	End Class

	''' <summary>
	''' If JToken is String, then use StringValue. If JToken is object, use ObjectValue.
	''' </summary>
	Public Class StringOrObject(Of T)
		Public Property StringValue As String
		Public Property ObjectValue As T
	End Class
End Namespace
