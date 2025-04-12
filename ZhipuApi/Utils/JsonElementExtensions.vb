Imports System.Runtime.CompilerServices
Imports System.Text.Json

Module JsonElementExtensions

    <Extension>
    Function GetNullableString(element As JsonElement, key As String) As String
        Dim subElement As JsonElement = Nothing
        If element.ValueKind <> JsonValueKind.Object Then
            Return Nothing
        ElseIf element.TryGetProperty(key, subElement) AndAlso subElement.ValueKind = JsonValueKind.String Then
            Return subElement.GetString()
        End If
        Return Nothing
    End Function

    <Extension>
    Function GetNullableValue(element As JsonElement, key As String) As Object
        Dim subElement As JsonElement = Nothing
        If element.ValueKind <> JsonValueKind.Object Then
            Return Nothing
        ElseIf element.TryGetProperty(key, subElement) Then
            Return subElement.GetNullableValue
        End If
        Return Nothing
    End Function

    <Extension>
    Function GetNullableValue(subElement As JsonElement) As Object
        Select Case subElement.ValueKind
            Case JsonValueKind.Number
                Return subElement.GetDouble()
            Case JsonValueKind.String
                Return subElement.GetString()
            Case JsonValueKind.True
                Return True
            Case JsonValueKind.False
                Return False
            Case Else
                Return Nothing
        End Select
    End Function

    <Extension>
    Function GetNullableArray(element As JsonElement, key As String) As JsonElement?
        Dim subElement As JsonElement = Nothing
        If element.ValueKind <> JsonValueKind.Object Then
            Return Nothing
        ElseIf element.TryGetProperty(key, subElement) AndAlso subElement.ValueKind = JsonValueKind.Array Then
            Return subElement
        End If
        Return Nothing
    End Function

    <Extension>
    Function GetNullableObject(element As JsonElement, key As String) As JsonElement?
        Dim subElement As JsonElement = Nothing
        If element.ValueKind <> JsonValueKind.Object Then
            Return Nothing
        ElseIf element.TryGetProperty(key, subElement) AndAlso subElement.ValueKind = JsonValueKind.Object Then
            Return subElement
        End If
        Return Nothing
    End Function
End Module
