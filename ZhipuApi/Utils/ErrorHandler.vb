Imports System.IO
Imports System.Net.Http
Imports System.Text
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

        Public Shared Sub ThrowErrorWithDetailedMessage(result As MemoryStream)
            Dim exMessage As String = "Error in SSE response: "
            Dim details As RequestError = Nothing
            If result.Length > 0 Then
                exMessage &= Encoding.UTF8.GetString(result.ToArray)
                result.Position = 0
                Try
                    details = ErrorResponse.FromJson(result).Error
                Catch ex As Exception
                End Try
            Else
                exMessage &= "Unspecified Error"
            End If
            Throw New ZhipuHttpRequestException(exMessage, details)
        End Sub
    End Class

End Namespace
