Imports System
Imports ZhipuApi.Models.RequestModels.FunctionModels

Namespace ZhipuApi.Models.RequestModels
	' Token: 0x02000015 RID: 21
	Public Class TextRequestBase
		' Token: 0x1700002A RID: 42
		' (get) Token: 0x06000078 RID: 120 RVA: 0x000028BC File Offset: 0x00000ABC
		' (set) Token: 0x06000079 RID: 121 RVA: 0x000028C4 File Offset: 0x00000AC4
		Public Property request_id As String

		' Token: 0x1700002B RID: 43
		' (get) Token: 0x0600007A RID: 122 RVA: 0x000028CD File Offset: 0x00000ACD
		' (set) Token: 0x0600007B RID: 123 RVA: 0x000028D5 File Offset: 0x00000AD5
		Public Property model As String

		' Token: 0x1700002C RID: 44
		' (get) Token: 0x0600007C RID: 124 RVA: 0x000028DE File Offset: 0x00000ADE
		' (set) Token: 0x0600007D RID: 125 RVA: 0x000028E6 File Offset: 0x00000AE6
		Public Property messages As MessageItem()

		' Token: 0x1700002D RID: 45
		' (get) Token: 0x0600007E RID: 126 RVA: 0x000028EF File Offset: 0x00000AEF
		' (set) Token: 0x0600007F RID: 127 RVA: 0x000028F7 File Offset: 0x00000AF7
		Public Property tools As FunctionTool()

		' Token: 0x1700002E RID: 46
		' (get) Token: 0x06000080 RID: 128 RVA: 0x00002900 File Offset: 0x00000B00
		' (set) Token: 0x06000081 RID: 129 RVA: 0x00002908 File Offset: 0x00000B08
		Public Property tool_choice As String

		' Token: 0x1700002F RID: 47
		' (get) Token: 0x06000082 RID: 130 RVA: 0x00002911 File Offset: 0x00000B11
		' (set) Token: 0x06000083 RID: 131 RVA: 0x00002919 File Offset: 0x00000B19
		Public Property top_p As Double

		' Token: 0x17000030 RID: 48
		' (get) Token: 0x06000084 RID: 132 RVA: 0x00002922 File Offset: 0x00000B22
		' (set) Token: 0x06000085 RID: 133 RVA: 0x0000292A File Offset: 0x00000B2A
		Public Property temperature As Double

		' Token: 0x17000031 RID: 49
		' (get) Token: 0x06000086 RID: 134 RVA: 0x00002933 File Offset: 0x00000B33
		' (set) Token: 0x06000087 RID: 135 RVA: 0x0000293B File Offset: 0x00000B3B
		Public Property stream As Boolean

		' Token: 0x06000088 RID: 136 RVA: 0x00002944 File Offset: 0x00000B44
		Public Sub New()
			Me.stream = True
		End Sub

		' Token: 0x06000089 RID: 137 RVA: 0x00002958 File Offset: 0x00000B58
		Public Function SetRequestId(requestId As String) As TextRequestBase
			Me.request_id = requestId
			Return Me
		End Function

		' Token: 0x0600008A RID: 138 RVA: 0x00002974 File Offset: 0x00000B74
		Public Function SetModel(model As String) As TextRequestBase
			Me.model = model
			Return Me
		End Function

		' Token: 0x0600008B RID: 139 RVA: 0x00002990 File Offset: 0x00000B90
		Public Function SetMessages(messages As MessageItem()) As TextRequestBase
			Me.messages = messages
			Return Me
		End Function

		' Token: 0x0600008C RID: 140 RVA: 0x000029AC File Offset: 0x00000BAC
		Public Function SetTools(tools As FunctionTool()) As TextRequestBase
			Me.tools = tools
			Return Me
		End Function

		' Token: 0x0600008D RID: 141 RVA: 0x000029C8 File Offset: 0x00000BC8
		Public Function SetToolChoice(toolChoice As String) As TextRequestBase
			Me.tool_choice = toolChoice
			Return Me
		End Function

		' Token: 0x0600008E RID: 142 RVA: 0x000029E4 File Offset: 0x00000BE4
		Public Function SetTopP(topP As Double) As TextRequestBase
			Me.top_p = topP
			Return Me
		End Function

		' Token: 0x0600008F RID: 143 RVA: 0x00002A00 File Offset: 0x00000C00
		Public Function SetTemperature(temperature As Double) As TextRequestBase
			Me.temperature = temperature
			Return Me
		End Function
	End Class
End Namespace
