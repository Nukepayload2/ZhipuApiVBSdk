Imports Newtonsoft.Json.Linq

Module JsonHelper

    Function ConvertJTokenToObject(jtoken As JToken) As Object
        If jtoken Is Nothing Then
            Return Nothing
        End If

        Try
            Select Case jtoken.Type
                Case JTokenType.String
                    Return jtoken.ToString()
                Case JTokenType.Integer
                    Return jtoken.ToObject(Of Integer)
                Case JTokenType.Float
                    Return jtoken.ToObject(Of Double)
                Case JTokenType.Boolean
                    Return jtoken.ToObject(Of Boolean)
                Case JTokenType.Date
                    Return jtoken.ToObject(Of Date)
                Case JTokenType.Null
                    Return Nothing
                Case JTokenType.Array
                    Dim array = DirectCast(jtoken, JArray)
                    Return Aggregate item In array Select ConvertJTokenToObject(item) Into ToArray
                Case Else
                    Debug.WriteLine($"不支持的 JToken 类型: {jtoken.Type}")
                    Return Nothing
            End Select
        Catch ex As Exception
            ' 如果转换失败，返回 Nothing 并输出错误信息
            Debug.WriteLine($"无法将 JToken 转换为对象: {ex.Message}")
            Return Nothing
        End Try
    End Function

End Module
