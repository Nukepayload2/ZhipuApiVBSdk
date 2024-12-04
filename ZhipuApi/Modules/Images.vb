Imports System.IO
Imports System.Net.Http
Imports System.Threading
Imports Nukepayload2.AI.Providers.Zhipu.Models

Public Class Images
    Inherits ClientFeatureBase

    Public Sub New(apiKey As String)
        MyBase.New(apiKey)
    End Sub

    Sub New(apiKey As String, client As HttpClient)
        MyBase.New(apiKey, client)
    End Sub

    Private Async Function GenerateInternalAsync(requestBody As ImageRequestBase, cancellation As CancellationToken) As Task(Of MemoryStream)
        Const RequestUrl = "https://open.bigmodel.cn/api/paas/v4/images/generations"
        Dim json = requestBody.ToJsonUtf8()
        Return Await PostAsync(RequestUrl, json, cancellation)
    End Function

    Public Async Function GenerateAsync(requestBody As ImageRequestBase,
                                        Optional cancellation As CancellationToken = Nothing) As Task(Of ImageResponseBase)
        Return ImageResponseBase.FromJson(Await GenerateInternalAsync(requestBody, cancellation))
    End Function

End Class
