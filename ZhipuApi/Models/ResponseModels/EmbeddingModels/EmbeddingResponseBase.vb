Imports Newtonsoft.Json
Imports System.IO

Namespace ZhipuApi.Models.ResponseModels.EmbeddingModels
    Public Class EmbeddingResponseBase
        Public Property Model As String
        Public Property [Object] As String

        Public Property Usage As Dictionary(Of String, Integer)

        Public Property Data As EmbeddingDataItem()

        Public Property [Error] As Dictionary(Of String, String)

        Public Shared Function FromJson(json As String) As EmbeddingResponseBase
            Using reader As New JsonTextReader(New StringReader(json))
                Return ReadEmbeddingResponseBase(reader)
            End Using
        End Function

        Private Shared Function ReadEmbeddingResponseBase(reader As JsonTextReader) As EmbeddingResponseBase
            Dim response As New EmbeddingResponseBase

            ' None -> FirstToken
            If reader.TokenType = JsonToken.None Then reader.Read()

            If reader.TokenType = JsonToken.StartObject Then
                While reader.Read()
                    If reader.TokenType = JsonToken.EndObject Then
                        Exit While
                    End If

                    If reader.TokenType = JsonToken.PropertyName Then
                        Dim propertyName As String = reader.Value.ToString()

                        reader.Read()

                        Select Case propertyName
                            Case "model"
                                response.Model = reader.Value.ToString()
                            Case "object"
                                response.Object = reader.Value.ToString()
                            Case "usage"
                                response.Usage = ReadUsageDictionary(reader)
                            Case "data"
                                response.Data = ReadEmbeddingDataItems(reader)
                            Case "error"
                                response.Error = ReadErrorDictionary(reader)
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

            Return response
        End Function

        Private Shared Function ReadUsageDictionary(reader As JsonTextReader) As Dictionary(Of String, Integer)
            Dim usageDict As New Dictionary(Of String, Integer)

            While reader.Read()
                If reader.TokenType = JsonToken.EndObject Then
                    Exit While
                End If

                If reader.TokenType = JsonToken.PropertyName Then
                    Dim propertyName As String = reader.Value.ToString()

                    reader.Read()

                    usageDict.Add(propertyName, CInt(reader.Value))
                Else
                    Throw New InvalidDataException($"Unexpected token type: {reader.TokenType}")
                End If
            End While

            Return usageDict
        End Function

        Private Shared Function ReadEmbeddingDataItems(reader As JsonTextReader) As EmbeddingDataItem()
            If reader.TokenType <> JsonToken.StartArray Then
                Throw New JsonReaderException($"Unexpected token type {reader.TokenType} at start array")
            End If
            Dim array As New List(Of EmbeddingDataItem)
            While reader.Read() AndAlso reader.TokenType <> JsonToken.EndArray
                If reader.TokenType = JsonToken.StartObject Then
                    array.Add(ReadEmbeddingDataItem(reader))
                Else
                    Throw New JsonReaderException("Expected double value")
                End If
            End While
            Return array.ToArray()
        End Function

        Private Shared Function ReadEmbeddingDataItem(reader As JsonTextReader) As EmbeddingDataItem
            If reader.TokenType <> JsonToken.StartObject Then
                Throw New JsonReaderException($"Unexpected token type {reader.TokenType} at start object")
            End If

            Dim dataItem As New EmbeddingDataItem

            While reader.Read()
                If reader.TokenType = JsonToken.EndObject Then
                    Exit While
                End If

                If reader.TokenType = JsonToken.PropertyName Then
                    Dim propertyName As String = reader.Value.ToString()

                    reader.Read()

                    Select Case propertyName
                        Case "index"
                            dataItem.Index = CInt(reader.Value)
                        Case "object"
                            dataItem.Object = reader.Value.ToString()
                        Case "embedding"
                            dataItem.Embedding = ReadEmbeddingArray(reader)
                        Case Else
                            Throw New InvalidDataException($"Unexpected property: {propertyName}")
                    End Select
                Else
                    Throw New InvalidDataException($"Unexpected token type: {reader.TokenType}")
                End If
            End While

            Return dataItem
        End Function

        Private Shared Function ReadEmbeddingArray(reader As JsonTextReader) As Double()
            If reader.TokenType <> JsonToken.StartArray Then
                Throw New JsonReaderException($"Unexpected token type {reader.TokenType} at start array")
            End If
            Dim array As New List(Of Double)
            While reader.Read() AndAlso reader.TokenType <> JsonToken.EndArray
                If reader.TokenType = JsonToken.Float OrElse reader.TokenType = JsonToken.Integer Then
                    array.Add(CDbl(reader.Value))
                Else
                    Throw New JsonReaderException("Expected double value")
                End If
            End While
            Return array.ToArray()
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

                    errorDict.Add(propertyName, reader.Value?.ToString())
                Else
                    Throw New InvalidDataException($"Unexpected token type: {reader.TokenType}")
                End If
            End While

            Return errorDict
        End Function
    End Class
End Namespace
