Imports Newtonsoft.Json
Imports System.IO
Imports ZhipuApi.Models.RequestModels.FunctionModels
Namespace ZhipuApi.Models.RequestModels
    Public Class TextRequestBase
        Public Property request_id As String
        Public Property model As String

        Public Property messages As MessageItem()

		Public Property tools As FunctionTool()

		Public Property tool_choice As String

        Public Property top_p As Double?

        Public Property temperature As Double?

        Public Property stream As Boolean = True

		Public Function ToJson() As String
            Dim stringWriter = New StringWriter
            Dim jsonWriter = New JsonTextWriter(stringWriter) With {
                .Formatting = Formatting.None,
                .Indentation = 0
            }
            jsonWriter.WriteStartObject()

            If request_id IsNot Nothing Then
                jsonWriter.WritePropertyName("request_id")
                jsonWriter.WriteValue(request_id)
            End If

            If model IsNot Nothing Then
                jsonWriter.WritePropertyName("model")
                jsonWriter.WriteValue(model)
            End If

            If messages IsNot Nothing Then
                jsonWriter.WritePropertyName("messages")
                jsonWriter.WriteStartArray()
                For Each message In messages
                    If message IsNot Nothing Then
                        jsonWriter.WriteStartObject()
                        If message.role IsNot Nothing Then
                            jsonWriter.WritePropertyName("role")
                            jsonWriter.WriteValue(message.role)
                        End If
                        If message.content IsNot Nothing Then
                            jsonWriter.WritePropertyName("content")
                            If message.content.StringValue IsNot Nothing Then
                                jsonWriter.WriteValue(message.content.StringValue)
                            ElseIf message.content.ArrayValue IsNot Nothing Then
                                jsonWriter.WriteStartArray()
                                For Each item In message.content.ArrayValue
                                    jsonWriter.WriteValue(item)
                                Next
                                jsonWriter.WriteEndArray()
                            End If
                        End If
                        jsonWriter.WriteEndObject()
                    End If
                Next
                jsonWriter.WriteEndArray()
            End If

            If tools IsNot Nothing Then
                jsonWriter.WritePropertyName("tools")
                jsonWriter.WriteStartArray()
                For Each tool In tools
                    If tool IsNot Nothing Then
                        jsonWriter.WriteStartObject()
                        If tool.type IsNot Nothing Then
                            jsonWriter.WritePropertyName("type")
                            jsonWriter.WriteValue(tool.type)
                        End If
                        If tool.[function] IsNot Nothing Then
                            jsonWriter.WritePropertyName("function")
                            jsonWriter.WriteStartObject()
                            For Each kv In tool.[function]
                                jsonWriter.WritePropertyName(kv.Key)
                                If kv.Value.StringValue IsNot Nothing Then
                                    jsonWriter.WriteValue(kv.Value.StringValue)
                                ElseIf kv.Value.ObjectValue IsNot Nothing Then
                                    jsonWriter.WriteStartObject()
                                    For Each param In kv.Value.ObjectValue.properties
                                        jsonWriter.WritePropertyName(param.Key)
                                        jsonWriter.WriteValue(param.Value.description)
                                    Next
                                    jsonWriter.WriteEndObject()
                                End If
                            Next
                            jsonWriter.WriteEndObject()
                        End If
                        jsonWriter.WriteEndObject()
                    End If
                Next
                jsonWriter.WriteEndArray()
            End If

            If tool_choice IsNot Nothing Then
                jsonWriter.WritePropertyName("tool_choice")
                jsonWriter.WriteValue(tool_choice)
            End If

            If top_p.HasValue Then
                jsonWriter.WritePropertyName("top_p")
                jsonWriter.WriteValue(top_p)
            End If

            If temperature.HasValue Then
                jsonWriter.WritePropertyName("temperature")
                jsonWriter.WriteValue(temperature)
            End If

            jsonWriter.WritePropertyName("stream")
            jsonWriter.WriteValue(stream)

            jsonWriter.WriteEndObject()

            Return stringWriter.ToString()
        End Function
	End Class
End Namespace
