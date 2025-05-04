Imports System.Net
Imports System.Net.Http
Imports Nukepayload2.AI.Providers.Zhipu.Models

Public Class ZhipuHttpRequestException
    Inherits HttpRequestException

    Public ReadOnly Property Details As RequestError

    Public Sub New(message As String, details As RequestError, statusCode As HttpStatusCode)
#If NET6_0_OR_GREATER Then
        MyBase.New(message, Nothing, statusCode)
#Else
        MyBase.New(message)
#End If
        Me.Details = details
    End Sub
End Class
