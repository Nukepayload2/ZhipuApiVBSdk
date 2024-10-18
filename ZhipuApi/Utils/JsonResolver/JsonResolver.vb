Imports System
Imports System.Reflection
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Serialization
Imports ZhipuApi.Models.RequestModels
Imports ZhipuApi.Models.RequestModels.ImageToTextModels

Namespace ZhipuApi.Utils.JsonResolver
	' Token: 0x02000004 RID: 4
	Public Class JsonResolver
		Inherits DefaultContractResolver

		' Token: 0x0600000A RID: 10 RVA: 0x000021C8 File Offset: 0x000003C8
		Protected Overrides Function CreateProperty(member As MemberInfo, memberSerialization As MemberSerialization) As JsonProperty
			Dim [property] As JsonProperty = MyBase.CreateProperty(member, memberSerialization)
			Dim flag As Boolean = [property].DeclaringType Is GetType(MessageItem) AndAlso [property].PropertyName = "content"
			If flag Then
				Dim <>9__0_ As Predicate(Of Object) = JsonResolver.<>c.<>9__0_0
				Dim predicate As Predicate(Of Object) = <>9__0_
				If <>9__0_ Is Nothing Then
					Dim predicate2 As Predicate(Of Object) = Function(instance As Object)
						Dim flag2 As Boolean = TypeOf instance Is ImageToTextMessageItem
						Return Not flag2
					End Function
					predicate = predicate2
					JsonResolver.<>c.<>9__0_0 = predicate2
				End If
				Dim shouldSerialize As Predicate(Of Object) = predicate
				[property].ShouldSerialize = shouldSerialize
			End If
			Return [property]
		End Function
	End Class
End Namespace
