Imports System.Runtime.InteropServices
Imports Newtonsoft.Json.Linq

Namespace Utils
    Friend Class JsonReadOnlyStringDictionary
        Implements IReadOnlyDictionary(Of String, String)

        Private ReadOnly _jsonObj As JObject

        Protected Sub New(jsonObj As JObject)
            _jsonObj = jsonObj
        End Sub

        Public Shared Function ToJsonObject(dictionary As IReadOnlyDictionary(Of String, String)) As JObject
            If dictionary Is Nothing Then
                Return Nothing
            End If
            Dim jsonObj As New JObject
            For Each p In dictionary
                jsonObj.Add(p.Key, WrapJsonPrimitive(p.Value))
            Next
            Return jsonObj
        End Function

        Private Shared Function UnwrapJsonPrimitive(token As JToken) As String
            Return token?.Value(Of String)
        End Function

        Private Shared Function WrapJsonPrimitive(token As String) As JValue
            If token Is Nothing Then
                Return JValue.CreateNull
            End If
            Return New JValue(token)
        End Function

        Default Public Property Item(key As String) As String Implements IReadOnlyDictionary(Of String, String).Item
            Get
                Dim propValue = _jsonObj(key)
                Return UnwrapJsonPrimitive(propValue)
            End Get
            Set(value As String)
                Dim primitive = WrapJsonPrimitive(value)
                _jsonObj(key) = primitive
            End Set
        End Property

        Public ReadOnly Property Keys As IEnumerable(Of String) Implements IReadOnlyDictionary(Of String, String).Keys
            Get
                Return _jsonObj.Properties.Select(Function(prop) prop.Name)
            End Get
        End Property

        Public ReadOnly Property Values As IEnumerable(Of String) Implements IReadOnlyDictionary(Of String, String).Values
            Get
                Return _jsonObj.Values.Select(AddressOf UnwrapJsonPrimitive)
            End Get
        End Property

        Public ReadOnly Property Count As Integer Implements IReadOnlyCollection(Of KeyValuePair(Of String, String)).Count
            Get
                Return _jsonObj.Count
            End Get
        End Property

        Public Iterator Function GetEnumerator() As IEnumerator(Of KeyValuePair(Of String, String)) Implements IEnumerable(Of KeyValuePair(Of String, String)).GetEnumerator
            For Each prop In _jsonObj.Properties
                Yield New KeyValuePair(Of String, String)(prop.Name, UnwrapJsonPrimitive(prop.Value))
            Next
        End Function

        Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return GetEnumerator()
        End Function

        Public Function ContainsKey(key As String) As Boolean Implements IReadOnlyDictionary(Of String, String).ContainsKey
            Return _jsonObj.ContainsKey(key)
        End Function

        Public Function TryGetValue(key As String, <Out> ByRef value As String) As Boolean Implements IReadOnlyDictionary(Of String, String).TryGetValue
            Dim tokenValue As JToken = Nothing
            Dim hasValue = _jsonObj.TryGetValue(key, tokenValue)
            If hasValue Then
                value = UnwrapJsonPrimitive(tokenValue)
            Else
                value = Nothing
            End If
            Return hasValue
        End Function

        Public Shared Function Wrap(jsonObj As JObject) As IReadOnlyDictionary(Of String, String)
            If jsonObj Is Nothing Then
                Return Nothing
            End If
            Return New JsonReadOnlyStringDictionary(jsonObj)
        End Function
    End Class

End Namespace
