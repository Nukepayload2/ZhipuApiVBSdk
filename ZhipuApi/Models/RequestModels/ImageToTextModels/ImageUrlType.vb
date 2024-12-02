Namespace Models
	Public Class ImageUrlType
		Public Property Url As String
		Sub New()

		End Sub

		Sub New(url As String)
			Me.Url = url
		End Sub

		Public Shared Widening Operator CType(url As String) As ImageUrlType
			Return New ImageUrlType(url)
		End Operator
	End Class
End Namespace
