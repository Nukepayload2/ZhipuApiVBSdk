Namespace Models
    Public Class ResponseFormat
        Public Sub New(type As String)
            Me.Type = type
        End Sub

        Public Shared ReadOnly Property Text As New ResponseFormat("text")
        Public Shared ReadOnly Property Json As New ResponseFormat("json_object")

        Public ReadOnly Property Type As String
    End Class
End Namespace
