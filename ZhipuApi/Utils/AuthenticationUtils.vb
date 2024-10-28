Imports System.IdentityModel.Tokens.Jwt
Imports System.Text
Imports Microsoft.IdentityModel.Tokens

Namespace ZhipuApi.Utils
	Public Class AuthenticationUtils
		Public Shared Function GenerateToken(apiKey As String, expSeconds As Integer) As String
			Dim parts As String() = apiKey.Split("."c, StringSplitOptions.None)
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
