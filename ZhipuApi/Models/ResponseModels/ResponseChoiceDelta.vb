Imports ZhipuApi.Models.ResponseModels.ToolModels

Namespace ZhipuApi.Models.ResponseModels
	Public Class ResponseChoiceDelta
		Public Property Role As String

		Public Property Content As String

		Public Property ToolCalls As ToolCallItem()
	End Class
End Namespace
