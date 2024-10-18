Imports System
Imports System.IdentityModel.Tokens.Jwt
Imports System.Text
Imports Microsoft.IdentityModel.Tokens

Namespace ZhipuApi.Utils
	' Token: 0x02000003 RID: 3
	Public Class AuthenticationUtils
		' Token: 0x06000008 RID: 8 RVA: 0x000020BC File Offset: 0x000002BC
		Public Shared Function GenerateToken(apiKey As String, expSeconds As Integer) As String
			Dim parts As String() = apiKey.Split("."c, StringSplitOptions.None)
			Dim flag As Boolean = parts.Length <> 2
			If flag Then
				Throw New ArgumentException("Invalid API key format.")
			End If
			Dim id As String = parts(0)
			Dim secret As String = parts(1)
			Dim keyBytes As Byte() = Encoding.UTF8.GetBytes(secret)
			Dim flag2 As Boolean = keyBytes.Length < 32
			If flag2 Then
				Array.Resize(Of Byte)(keyBytes, 32)
			End If
			Dim securityKey As SymmetricSecurityKey = New SymmetricSecurityKey(keyBytes)
			Dim credentials As SigningCredentials = New SigningCredentials(securityKey, "HS256")
			Dim jwtPayload As JwtPayload = New JwtPayload()
			jwtPayload.Add("api_key", id)
			jwtPayload.Add("exp", DateTimeOffset.UtcNow.ToUnixTimeSeconds() + CLng(expSeconds))
			jwtPayload.Add("timestamp", DateTimeOffset.UtcNow.ToUnixTimeSeconds())
			Dim payload As JwtPayload = jwtPayload
			Dim header As JwtHeader = New JwtHeader(credentials)
			header.Add("sign_type", "SIGN")
			Dim token As JwtSecurityToken = New JwtSecurityToken(header, payload)
			Return New JwtSecurityTokenHandler().WriteToken(token)
		End Function
	End Class
End Namespace
