Imports System
Imports System.Reflection
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Serialization
Imports ZhipuApi.Models.RequestModels
Imports ZhipuApi.Models.RequestModels.ImageToTextModels

Namespace ZhipuApi.Utils.JsonResolver
	Public Class JsonResolver
		Inherits DefaultContractResolver

		Protected Overrides Function CreateProperty(ByVal member As MemberInfo, ByVal memberSerialization As MemberSerialization) As JsonProperty
			Dim [property] As JsonProperty = MyBase.CreateProperty(member, memberSerialization)

			If [property].DeclaringType Is GetType(MessageItem) AndAlso [property].PropertyName = "content" Then
				Dim shouldSerialize As Predicate(Of Object) =
					Function(instance)
						If TypeOf instance Is ImageToTextMessageItem Then
							Return False
						End If
						Return True
					End Function
				[property].ShouldSerialize = shouldSerialize
			End If

			Return [property]
		End Function
	End Class

End Namespace
