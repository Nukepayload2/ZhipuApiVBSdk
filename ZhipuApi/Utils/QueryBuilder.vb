Imports System.Text

Namespace Utils

    Public Class QueryBuilder

        Private ReadOnly _baseUrl As String
        Private ReadOnly _parameters As New List(Of String)

        ''' <summary>
        ''' 构造函数传入基础 URL
        ''' </summary>
        Sub New(baseUrl As String)
            _baseUrl = baseUrl
        End Sub

        ''' <summary>
        ''' 添加查询参数（字符串类型）
        ''' </summary>
        Public Sub Add(name As String, value As String)
            If value IsNot Nothing Then
                _parameters.Add($"{name}={Uri.EscapeDataString(value)}")
            End If
        End Sub

        ''' <summary>
        ''' 添加查询参数（可空长整型）
        ''' </summary>
        Public Sub Add(name As String, value As Long?)
            If value.HasValue Then
                _parameters.Add($"{name}={value.Value}")
            End If
        End Sub

        ''' <summary>
        ''' 返回完整的带查询字符串的 URL
        ''' </summary>
        Public Overrides Function ToString() As String
            If _parameters.Count = 0 Then
                Return _baseUrl
            End If

            Dim sb As New StringBuilder(_baseUrl)
            sb.Append("?"c)
#If NET8_0_OR_GREATER Then
            sb.AppendJoin("&", _parameters)
#Else
            For i = 0 To _parameters.Count - 1
                Dim param = _parameters(i)
                sb.Append(param).Append("&"c)
            Next
            sb.Remove(sb.Length - 1, 1)
#End If

            Return sb.ToString()
        End Function

    End Class

End Namespace
