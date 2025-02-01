Imports Newtonsoft.Json.Linq

Namespace Models

	' 丐板的 JSchema (https://github.com/JamesNK/Newtonsoft.Json.Schema/blob/master/Src/Newtonsoft.Json.Schema/JSchema.cs)
	' 那个类型许可证太严格了不能用。

	Public Class FunctionParameterDescriptor
		Public Property Type As String

		Public Property Description As String

		Public Property [Default] As Object
			Get
				Return ConvertJTokenToObject(DefaultJson)
			End Get
			Set(value As Object)
				If value Is Nothing Then
					DefaultJson = Nothing
					Return
				End If

				DefaultJson = New JValue(value)
			End Set
		End Property

		' 避免导出具体 JSON 库的类型
		Friend Property DefaultJson As JValue

		Public Property [Enum] As IReadOnlyList(Of Object)
			Get
				If EnumJson Is Nothing Then
					Return Nothing
				End If
				Return Aggregate item In EnumJson Select ConvertJTokenToObject(item) Into ToArray
			End Get
			Set(value As IReadOnlyList(Of Object))
				If value Is Nothing Then
					EnumJson = Nothing
					Return
				End If

				EnumJson = Aggregate item In value Select New JValue(item) Into ToArray
			End Set
		End Property

		' 避免导出具体 JSON 库的类型
		Friend Property EnumJson As IReadOnlyList(Of JValue)

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
