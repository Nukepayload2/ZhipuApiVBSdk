Namespace Models
	Public Class ResponseChoiceDelta
		Public Property Role As String

		Public Property Content As String

		Public Property ToolCalls As IReadOnlyList(Of ToolCallItem)
	End Class
End Namespace
