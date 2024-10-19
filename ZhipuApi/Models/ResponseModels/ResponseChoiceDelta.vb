Imports ZhipuApi.Models.ResponseModels.ToolModels

Namespace ZhipuApi.Models.ResponseModels
	Public Class ResponseChoiceDelta
		Public Property role As String

		Public Property content As String

		Public Property tool_calls As ToolCallItem()
	End Class
End Namespace
