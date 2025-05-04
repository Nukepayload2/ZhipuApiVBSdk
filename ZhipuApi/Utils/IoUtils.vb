Imports System.IO
Imports System.Net.Http
Imports System.Text
Imports System.Threading

Namespace Utils
    Friend Class IoUtils
        Private Shared _UTF8NoBOM As Encoding

        Public Shared ReadOnly Property UTF8NoBOM As Encoding
            Get
                If Volatile.Read(_UTF8NoBOM) Is Nothing Then
                    ' No need for double lock - we just want to avoid extra allocations in the common case.
                    Dim noBOM As New UTF8Encoding(False, True)
                    Thread.MemoryBarrier()
                    Volatile.Write(_UTF8NoBOM, noBOM)
                End If
                Return Volatile.Read(_UTF8NoBOM)
            End Get
        End Property

        Public Shared Async Function CopyToMemoryStreamAsync(response As HttpResponseMessage, cancellation As CancellationToken) As Task(Of MemoryStream)
#If NET6_0_OR_GREATER Then
            Dim stream = Await response.Content.ReadAsStreamAsync(cancellation)
#Else
            Dim stream = Await response.Content.ReadAsStreamAsync()
#End If
            Dim result As New MemoryStream
            Await stream.CopyToAsync(result, 8192, cancellation)
            result.Position = 0
            Return result
        End Function
    End Class
End Namespace
