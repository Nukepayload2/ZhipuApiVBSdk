Imports System.IdentityModel.Tokens.Jwt
Imports System.Text
Imports System.Threading
Imports Microsoft.IdentityModel.Tokens

Namespace Utils
    Friend Class AuthenticationUtils
        Public Shared Function GenerateToken(apiKey As String, expSeconds As Integer) As String
#If NET6_0_OR_GREATER Then
            Dim parts As String() = apiKey.Split("."c, StringSplitOptions.None)
#Else
			Dim parts As String() = apiKey.Split({"."c}, StringSplitOptions.None)
#End If
            Dim flag As Boolean = parts.Length <> 2
            If flag Then
                Throw New ArgumentException("Invalid API key format.")
            End If
            Dim id As String = parts(0)
            Dim secret As String = parts(1)
            Dim keyBytes As Byte() = Encoding.UTF8.GetBytes(secret)
            If keyBytes.Length < 32 Then
                Array.Resize(keyBytes, 32)
            End If
            Dim securityKey As New SymmetricSecurityKey(keyBytes)
            Dim credentials As New SigningCredentials(securityKey, "HS256")
            Dim jwtPayload As New JwtPayload From {
                {"api_key", id},
                {"exp", DateTimeOffset.UtcNow.ToUnixTimeSeconds() + CLng(expSeconds)},
                {"timestamp", DateTimeOffset.UtcNow.ToUnixTimeSeconds()}
            }
            Dim payload As JwtPayload = jwtPayload
            Dim header As New JwtHeader(credentials) From {
                {"sign_type", "SIGN"}
            }
            Dim token As New JwtSecurityToken(header, payload)
            Return New JwtSecurityTokenHandler().WriteToken(token)
        End Function
    End Class
End Namespace
