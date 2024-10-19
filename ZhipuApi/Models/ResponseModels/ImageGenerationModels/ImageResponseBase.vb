Imports Newtonsoft.Json
Imports System.IO

Namespace ZhipuApi.Models.ResponseModels.ImageGenerationModels
	Public Class ImageResponseBase
		Public Property created As Long

		Public Property data As List(Of ImageResponseDataItem)

        Public Property [error] As Dictionary(Of String, String)

        Public Shared Function FromJson(json As String) As ImageResponseBase
			Using reader As New JsonTextReader(New StringReader(json))
				Return ReadImageResponseBase(reader)
			End Using
		End Function

        Private Shared Function ReadImageResponseBase(reader As JsonTextReader) As ImageResponseBase
            Dim response As New ImageResponseBase

            While reader.Read()
                If reader.TokenType = JsonToken.StartObject Then
                    While reader.Read()
                        If reader.TokenType = JsonToken.EndObject Then
                            Exit While
                        End If

                        If reader.TokenType = JsonToken.PropertyName Then
                            Dim propertyName As String = reader.Value.ToString()

                            reader.Read()

                            Select Case propertyName
                                Case "created"
                                    response.created = CLng(reader.Value)
                                Case "data"
                                    response.data = ReadImageDataList(reader)
                                Case "error"
                                    response.error = ReadErrorDictionary(reader)
                                Case Else
                                    Throw New InvalidDataException($"Unexpected property: {propertyName}")
                            End Select
                        Else
                            Throw New InvalidDataException($"Unexpected token type: {reader.TokenType}")
                        End If
                    End While
                Else
                    Throw New InvalidDataException($"Unexpected token type: {reader.TokenType}")
                End If
            End While

            Return response
        End Function

        Private Shared Function ReadImageDataList(reader As JsonTextReader) As List(Of ImageResponseDataItem)
            Dim dataList As New List(Of ImageResponseDataItem)

            While reader.Read()
                If reader.TokenType = JsonToken.StartObject Then
                    dataList.Add(ReadImageDataItem(reader))
                ElseIf reader.TokenType = JsonToken.EndArray Then
                    Exit While
                Else
                    Throw New InvalidDataException($"Unexpected token type: {reader.TokenType}")
                End If
            End While

            Return dataList
        End Function

        Private Shared Function ReadImageDataItem(reader As JsonTextReader) As ImageResponseDataItem
            Dim dataItem As New ImageResponseDataItem

            While reader.Read()
                If reader.TokenType = JsonToken.EndObject Then
                    Exit While
                End If

                If reader.TokenType = JsonToken.PropertyName Then
                    Dim propertyName As String = reader.Value.ToString()

                    reader.Read()

                    Select Case propertyName
                        Case "url"
                            dataItem.url = reader.Value.ToString()
                        Case Else
                            Throw New InvalidDataException($"Unexpected property: {propertyName}")
                    End Select
                Else
                    Throw New InvalidDataException($"Unexpected token type: {reader.TokenType}")
                End If
            End While

            Return dataItem
        End Function

        Private Shared Function ReadErrorDictionary(reader As JsonTextReader) As Dictionary(Of String, String)
            Dim errorDict As New Dictionary(Of String, String)

            While reader.Read()
                If reader.TokenType = JsonToken.EndObject Then
                    Exit While
                End If

                If reader.TokenType = JsonToken.PropertyName Then
                    Dim propertyName As String = reader.Value.ToString()

                    reader.Read()

                    errorDict.Add(propertyName, reader.Value.ToString())
                Else
                    Throw New InvalidDataException($"Unexpected token type: {reader.TokenType}")
                End If
            End While

            Return errorDict
        End Function
    End Class
End Namespace
