Imports Newtonsoft.Json
Imports Nukepayload2.AI.Providers.Zhipu.Serialization
Imports Nukepayload2.AI.Providers.Zhipu.Utils
Imports System.IO

Namespace Models
    ''' <summary>
    ''' For GLM models
    ''' </summary>
    Public Class TextRequestBase
        Public Property RequestId As String

        Public Property Model As String

        Public Property Messages As IReadOnlyList(Of MessageItem)

        Public Property Tools As IReadOnlyList(Of FunctionTool)

        Public Property ToolChoice As String

        Public Property TopP As Double?

        Public Property Temperature As Double?

        Public Property MaxResponseTokens As Integer?

        Public Property UserId As String

        Public Property DoSample As Boolean?

        Public Property ResponseFormat As ResponseFormat

        Public Property StopWords As IReadOnlyList(Of String)

        Public Property Stream As Boolean?

        Public Function ToJsonUtf8() As MemoryStream
            Dim ms As New MemoryStream
            Using sw As New StreamWriter(ms, IoUtils.UTF8NoBOM, 8192, True), jsonWriter As New JsonTextWriter(sw)
                TextRequestWriter.WriteTextRequestBase(jsonWriter, Me)
            End Using
            ms.Position = 0
            Return ms
        End Function

        Public Function ToJson() As String
            Using stringWriter = New StringWriter, jsonWriter = New JsonTextWriter(stringWriter)
                TextRequestWriter.WriteTextRequestBase(jsonWriter, Me)
                Return stringWriter.ToString()
            End Using
        End Function

    End Class

End Namespace
