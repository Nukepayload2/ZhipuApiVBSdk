Imports System.IO
Imports System.Net.Http
Imports System.Text
Imports System.Threading
Imports Nukepayload2.AI.Providers.Zhipu.Models

Namespace Utils
    Friend Class ErrorHandler

        Public Shared Sub ThrowForNonSuccess(response As HttpResponseMessage, result As MemoryStream)
            If response.StatusCode >= 400 Then
                ThrowErrorWithDetailedMessage(response, result)
            Else
                response.EnsureSuccessStatusCode()
            End If
        End Sub

        Public Shared Async Function ThrowForNonSuccessAsync(response As HttpResponseMessage, stream As Stream, cancellationToken As CancellationToken) As Task
            If response.StatusCode >= 400 Then
                Dim result = Await IoUtils.CopyToMemoryStreamAsync(response, cancellationToken)
                ThrowErrorWithDetailedMessage(response, result)
            Else
                response.EnsureSuccessStatusCode()
            End If
        End Function

        Private Shared Sub ThrowErrorWithDetailedMessage(response As HttpResponseMessage, result As MemoryStream)
            Dim exMessage As String = "HTTP " & response.StatusCode & ": "
            Dim details As RequestError = Nothing
            If result.Length > 0 Then
                exMessage &= Encoding.UTF8.GetString(result.ToArray)
                result.Position = 0
                Try
                    details = ErrorResponse.FromJson(result).Error
                Catch ex As Exception
                End Try
            Else
                exMessage &= response.ReasonPhrase
            End If
            Throw New ZhipuHttpRequestException(exMessage, details, response.StatusCode)
        End Sub
    End Class

End Namespace
