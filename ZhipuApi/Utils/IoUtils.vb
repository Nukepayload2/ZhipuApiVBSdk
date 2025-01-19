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
    End Class
End Namespace
