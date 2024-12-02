Imports System.IO
Imports Newtonsoft.Json
Imports ZhipuApi.Models.ResponseModels.ToolModels

Namespace ZhipuApi.Models.ResponseModels
    Public Class ResponseBase
        Public Property Id As String

        Public Property RequestId As String

        Public Property Created As Long

        Public Property Model As String

        Public Property Usage As Dictionary(Of String, Integer)

        Public Property Choices As ResponseChoiceItem()

        Public Property [Error] As Dictionary(Of String, String)

        Public Shared Function FromJson(json As Stream) As ResponseBase
            Dim jsonReader As New JsonTextReader(New StreamReader(json))
            Dim response As New ResponseBase
            response = ReadResponse(jsonReader, response)
            Return response
        End Function

        Public Shared Function FromJson(json As String) As ResponseBase
            Dim jsonReader As New JsonTextReader(New StringReader(json))
            Dim response As New ResponseBase
            response = ReadResponse(jsonReader, response)
            Return response
        End Function

        Private Shared Function ReadResponse(jsonReader As JsonTextReader, response As ResponseBase) As ResponseBase

            ' None -> FirstToken
            If jsonReader.TokenType = JsonToken.None Then jsonReader.Read()

            If jsonReader.TokenType = JsonToken.StartObject Then
                While jsonReader.Read()
                    If jsonReader.TokenType = JsonToken.PropertyName Then
                        Dim propertyName As String = jsonReader.Value.ToString()

                        jsonReader.Read()

                        Select Case jsonReader.TokenType
                            Case JsonToken.String
                                Select Case propertyName
                                    Case "id"
                                        response.Id = jsonReader.Value.ToString()
                                    Case "request_id"
                                        response.RequestId = jsonReader.Value.ToString()
                                    Case "model"
                                        response.Model = jsonReader.Value.ToString()
                                    Case Else
                                        Throw New InvalidDataException($"Unexpected property: {propertyName}")
                                End Select
                            Case JsonToken.Integer
                                Select Case propertyName
                                    Case "created"
                                        response.Created = CLng(jsonReader.Value)
                                    Case Else
                                        Throw New InvalidDataException($"Unexpected property: {propertyName}")
                                End Select
                            Case JsonToken.StartObject
                                Select Case propertyName
                                    Case "usage"
                                        response.Usage = DeserializeStringIntegerDictionary(jsonReader)
                                    Case "error"
                                        response.Error = DeserializeStringDictionary(jsonReader)
                                    Case Else
                                        Throw New InvalidDataException($"Unexpected property: {propertyName}")
                                End Select
                            Case JsonToken.StartArray
                                Select Case propertyName
                                    Case "choices"
                                        response.Choices = DeserializeChoices(jsonReader)
                                    Case Else
                                        Throw New InvalidDataException($"Unexpected property: {propertyName}")
                                End Select
                            Case Else
                                Throw New InvalidDataException($"Unexpected token type")
                        End Select
                    ElseIf jsonReader.TokenType = JsonToken.EndObject Then
                        Exit While
                    Else
                        Throw New InvalidDataException($"Unexpected token type")
                    End If
                End While
            Else
                Throw New InvalidDataException($"Unexpected token type")
            End If

            Return response
        End Function

        Private Shared Function DeserializeStringDictionary(jsonReader As JsonTextReader) As Dictionary(Of String, String)
            Dim dict As New Dictionary(Of String, String)

            While jsonReader.Read()
                If jsonReader.TokenType = JsonToken.EndObject Then
                    Exit While
                End If

                If jsonReader.TokenType = JsonToken.PropertyName Then
                    Dim key As String = jsonReader.Value.ToString()

                    jsonReader.Read()
                    If jsonReader.TokenType = JsonToken.String Then
                        dict.Add(key, jsonReader.Value.ToString())
                    Else
                        Throw New InvalidDataException("Expected string value")
                    End If
                Else
                    Throw New InvalidDataException($"Unexpected tonen: {jsonReader.TokenType}")
                End If
            End While

            Return dict
        End Function

        Private Shared Function DeserializeStringIntegerDictionary(jsonReader As JsonTextReader) As Dictionary(Of String, Integer)
            Dim dict As New Dictionary(Of String, Integer)()

            While jsonReader.Read()
                If jsonReader.TokenType = JsonToken.EndObject Then
                    Exit While
                End If

                If jsonReader.TokenType = JsonToken.PropertyName Then
                    Dim key As String = jsonReader.Value.ToString()

                    jsonReader.Read()
                    If jsonReader.TokenType = JsonToken.Integer Then
                        dict.Add(key, CInt(jsonReader.Value))
                    Else
                        Throw New InvalidDataException("Expected integer value")
                    End If
                Else
                    Throw New InvalidDataException($"Unexpected tonen: {jsonReader.TokenType}")
                End If
            End While

            Return dict
        End Function
        Private Shared Function DeserializeChoices(jsonReader As JsonTextReader) As ResponseChoiceItem()
            Dim choices As New List(Of ResponseChoiceItem)()

            While jsonReader.Read()
                If jsonReader.TokenType = JsonToken.EndArray Then
                    Exit While
                End If

                If jsonReader.TokenType = JsonToken.StartObject Then
                    Dim choice As New ResponseChoiceItem()

                    While jsonReader.Read()
                        If jsonReader.TokenType = JsonToken.EndObject Then
                            Exit While
                        End If

                        If jsonReader.TokenType = JsonToken.PropertyName Then
                            Dim propertyName As String = jsonReader.Value.ToString()

                            jsonReader.Read()

                            Select Case propertyName
                                Case "finish_reason"
                                    choice.FinishReason = jsonReader.Value.ToString()
                                Case "index"
                                    choice.Index = CInt(jsonReader.Value)
                                Case "message"
                                    choice.Message = DeserializeResponseChoiceDelta(jsonReader)
                                Case "delta"
                                    choice.Delta = DeserializeResponseChoiceDelta(jsonReader)
                                Case Else
                                    Throw New InvalidDataException($"Unexpected property: {propertyName}")
                            End Select
                        Else
                            Throw New InvalidDataException($"Unexpected tonen: {jsonReader.TokenType}")
                        End If
                    End While

                    choices.Add(choice)
                Else
                    Throw New InvalidDataException($"Unexpected tonen: {jsonReader.TokenType}")
                End If
            End While

            Return choices.ToArray()
        End Function

        Private Shared Function DeserializeResponseChoiceDelta(jsonReader As JsonTextReader) As ResponseChoiceDelta
            Dim delta As New ResponseChoiceDelta()

            While jsonReader.Read()
                If jsonReader.TokenType = JsonToken.EndObject Then
                    Exit While
                End If

                If jsonReader.TokenType = JsonToken.PropertyName Then
                    Dim propertyName As String = jsonReader.Value.ToString()

                    jsonReader.Read()

                    Select Case propertyName
                        Case "role"
                            delta.Role = jsonReader.Value.ToString()
                        Case "content"
                            delta.Content = jsonReader.Value.ToString()
                        Case "tool_calls"
                            delta.ToolCalls = DeserializeToolCalls(jsonReader)
                        Case Else
                            Throw New InvalidDataException($"Unexpected property: {propertyName}")
                    End Select
                Else
                    Throw New InvalidDataException($"Unexpected tonen: {jsonReader.TokenType}")
                End If
            End While

            Return delta
        End Function

        Private Shared Function DeserializeToolCalls(jsonReader As JsonTextReader) As ToolCallItem()
            Dim toolCalls As New List(Of ToolCallItem)()

            While jsonReader.Read()
                If jsonReader.TokenType = JsonToken.EndArray Then
                    Exit While
                End If

                If jsonReader.TokenType = JsonToken.StartObject Then
                    Dim toolCall As New ToolCallItem()

                    While jsonReader.Read()
                        If jsonReader.TokenType = JsonToken.EndObject Then
                            Exit While
                        End If

                        If jsonReader.TokenType = JsonToken.PropertyName Then
                            Dim propertyName As String = jsonReader.Value.ToString()

                            jsonReader.Read()

                            Select Case propertyName
                                Case "id"
                                    toolCall.Id = jsonReader.Value.ToString()
                                Case "function"
                                    toolCall.Function = DeserializeFunctionDescriptor(jsonReader)
                                Case "index"
                                    toolCall.Index = CInt(jsonReader.Value)
                                Case "type"
                                    toolCall.Type = jsonReader.Value.ToString()
                                Case Else
                                    Throw New InvalidDataException($"Unexpected property: {propertyName}")
                            End Select
                        Else
                            Throw New InvalidDataException($"Unexpected tonen: {jsonReader.TokenType}")
                        End If
                    End While
                    toolCalls.Add(toolCall)
                Else
                    Throw New InvalidDataException($"Unexpected tonen: {jsonReader.TokenType}")
                End If
            End While

            Return toolCalls.ToArray()
        End Function

        Private Shared Function DeserializeFunctionDescriptor(jsonReader As JsonTextReader) As FunctionDescriptor
            Dim functionDescriptor As New FunctionDescriptor

            While jsonReader.Read()
                If jsonReader.TokenType = JsonToken.EndObject Then
                    Exit While
                End If

                If jsonReader.TokenType = JsonToken.PropertyName Then
                    Dim propertyName As String = jsonReader.Value.ToString()

                    jsonReader.Read()

                    Select Case propertyName
                        Case "name"
                            functionDescriptor.Name = jsonReader.Value.ToString()
                        Case "arguments"
                            functionDescriptor.Arguments = jsonReader.Value.ToString()
                        Case Else
                            Throw New InvalidDataException($"Unexpected property: {propertyName}")
                    End Select
                Else
                    Throw New InvalidDataException($"Unexpected tonen: {jsonReader.TokenType}")
                End If
            End While

            Return functionDescriptor
        End Function
    End Class
End Namespace
