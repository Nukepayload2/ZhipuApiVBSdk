Imports Newtonsoft.Json
Imports Nukepayload2.AI.Providers.Zhipu.Utils
Imports System.IO

Namespace Models
    Public Class TextRequestBase
        Public Property RequestId As String

        Public Property Model As String

        Public Property Messages As MessageItem()

        Public Property Tools As FunctionTool()

        Public Property ToolChoice As String

        Public Property TopP As Double?

        Public Property Temperature As Double?

        Public Property Stream As Boolean

        Public Function ToJsonUtf8() As MemoryStream
            Dim ms As New MemoryStream
            Using sw As New StreamWriter(ms, IoUtils.UTF8NoBOM, 8192, True), jsonWriter As New JsonTextWriter(sw)
                ToJsonInternal(jsonWriter)
            End Using
            ms.Position = 0
            Return ms
        End Function

        Public Function ToJson() As String
            Using stringWriter = New StringWriter, jsonWriter = New JsonTextWriter(stringWriter)
                ToJsonInternal(jsonWriter)
                Return stringWriter.ToString()
            End Using
        End Function

        Private Sub ToJsonInternal(jsonWriter As JsonTextWriter)
            jsonWriter.WriteStartObject()

            If RequestId IsNot Nothing Then
                jsonWriter.WritePropertyName("request_id")
                jsonWriter.WriteValue(RequestId)
            End If

            If Model IsNot Nothing Then
                jsonWriter.WritePropertyName("model")
                jsonWriter.WriteValue(Model)
            End If

            If Messages IsNot Nothing Then
                jsonWriter.WritePropertyName("messages")
                jsonWriter.WriteStartArray()
                For Each message In Messages
                    WriteMessage(jsonWriter, message)
                Next
                jsonWriter.WriteEndArray()
            End If

            If Tools IsNot Nothing Then
                jsonWriter.WritePropertyName("tools")
                jsonWriter.WriteStartArray()
                For Each tool In Tools
                    If tool IsNot Nothing Then
                        jsonWriter.WriteStartObject()
                        WriteTool(jsonWriter, tool)
                        jsonWriter.WriteEndObject()
                    End If
                Next
                jsonWriter.WriteEndArray()
            End If

            If ToolChoice IsNot Nothing Then
                jsonWriter.WritePropertyName("tool_choice")
                jsonWriter.WriteValue(ToolChoice)
            End If

            If TopP IsNot Nothing Then
                jsonWriter.WritePropertyName("top_p")
                jsonWriter.WriteValue(TopP)
            End If

            If Temperature IsNot Nothing Then
                jsonWriter.WritePropertyName("temperature")
                jsonWriter.WriteValue(Temperature)
            End If

            jsonWriter.WritePropertyName("stream")
            jsonWriter.WriteValue(Stream)

            jsonWriter.WriteEndObject()
        End Sub

        Private Shared Sub WriteTool(jsonWriter As JsonTextWriter, tool As FunctionTool)
            If tool.Type IsNot Nothing Then
                jsonWriter.WritePropertyName("type")
                jsonWriter.WriteValue(tool.Type)
            End If
            If tool.Function Is Nothing Then Return
            jsonWriter.WritePropertyName("function")
            jsonWriter.WriteStartObject()
            For Each kv In tool.Function
                jsonWriter.WritePropertyName(kv.Key)
                If kv.Value.StringValue IsNot Nothing Then
                    jsonWriter.WriteValue(kv.Value.StringValue)
                ElseIf kv.Value.ObjectValue IsNot Nothing Then
                    jsonWriter.WriteStartObject()
                    If kv.Value.ObjectValue.Type IsNot Nothing Then
                        jsonWriter.WritePropertyName("type")
                        jsonWriter.WriteValue(kv.Value.ObjectValue.Type)
                    End If
                    If kv.Value.ObjectValue.Required IsNot Nothing Then
                        jsonWriter.WritePropertyName("required")
                        jsonWriter.WriteStartArray()
                        For Each req In kv.Value.ObjectValue.Required
                            jsonWriter.WriteValue(req)
                        Next
                        jsonWriter.WriteEndArray()
                    End If
                    If kv.Value.ObjectValue.Properties IsNot Nothing Then
                        WriteFuncProps(jsonWriter, kv)
                    End If
                    jsonWriter.WriteEndObject()
                End If
            Next
            jsonWriter.WriteEndObject()
        End Sub

        Private Shared Sub WriteFuncProps(jsonWriter As JsonTextWriter, kv As KeyValuePair(Of String, StringOrObject(Of FunctionParameters)))
            jsonWriter.WritePropertyName("properties")
            jsonWriter.WriteStartObject()
            For Each param In kv.Value.ObjectValue.Properties
                jsonWriter.WritePropertyName(param.Key)
                Dim funcValue = param.Value
                If funcValue Is Nothing Then
                    jsonWriter.WriteNull()
                Else
                    jsonWriter.WriteStartObject()
                    If funcValue.Type IsNot Nothing Then
                        jsonWriter.WritePropertyName("type")
                        jsonWriter.WriteValue(funcValue.Type)
                    End If
                    If funcValue.Description IsNot Nothing Then
                        jsonWriter.WritePropertyName("description")
                        jsonWriter.WriteValue(funcValue.Description)
                    End If
                    jsonWriter.WriteEndObject()
                End If
            Next
            jsonWriter.WriteEndObject()
        End Sub

        Private Shared Sub WriteMessage(jsonWriter As JsonTextWriter, message As MessageItem)
            If message Is Nothing Then Return
            jsonWriter.WriteStartObject()
            If message.Role IsNot Nothing Then
                jsonWriter.WritePropertyName("role")
                jsonWriter.WriteValue(message.Role)
            End If
            If message.Content IsNot Nothing Then
                jsonWriter.WritePropertyName("content")
                If message.Content.StringValue IsNot Nothing Then
                    jsonWriter.WriteValue(message.Content.StringValue)
                ElseIf message.Content.ArrayValue IsNot Nothing Then
                    jsonWriter.WriteStartArray()
                    For Each item In message.Content.ArrayValue
                        If item IsNot Nothing Then
                            jsonWriter.WriteStartObject()
                            If item.Type IsNot Nothing Then
                                jsonWriter.WriteValue(item.Type)
                            End If
                            If item.Text IsNot Nothing Then
                                jsonWriter.WriteValue(item.Text)
                            End If
                            If item.ImageUrl IsNot Nothing Then
                                jsonWriter.WriteStartObject()
                                If item.ImageUrl.Url IsNot Nothing Then
                                    jsonWriter.WriteValue(item.ImageUrl.Url)
                                End If
                                jsonWriter.WriteEndObject()
                            End If
                            jsonWriter.WriteEndObject()
                        Else
                            jsonWriter.WriteNull()
                        End If
                    Next
                    jsonWriter.WriteEndArray()
                End If
            End If
            jsonWriter.WriteEndObject()
        End Sub
    End Class
End Namespace
