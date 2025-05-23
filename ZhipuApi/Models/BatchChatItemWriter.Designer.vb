' <auto-generated/>
' This source file is part of the JSON serialization code and is not intended to be modified by you.
' Re-run the source generator will overwrite your changes in this file.
' Generated by: Nukepayload2.IO.Json.Serialization.NewtonsoftJson
Option Strict On
Option Infer On
Option Explicit On
Option Compare Binary
Imports Nukepayload2.AI.Providers.Zhipu.Models
Imports Newtonsoft.Json.Linq

Namespace Serialization
    Partial Class BatchChatItemWriter

        ''' <summary>
        ''' Writes <see cref="BatchChatRequestItem"/> to JsonWriter.
        ''' </summary>
        Public Shared Sub WriteBatchChatItem(writer As Global.Newtonsoft.Json.JsonWriter, value As BatchChatRequestItem)
            If value Is Nothing Then
                writer.WriteNull()
                Return
            End If

            writer.WriteStartObject()
            If value.CustomId IsNot Nothing Then
                writer.WritePropertyName("custom_id")
                writer.WriteValue(value.CustomId)
            End If
            If value.Method IsNot Nothing Then
                writer.WritePropertyName("method")
                writer.WriteValue(value.Method)
            End If
            If value.Url IsNot Nothing Then
                writer.WritePropertyName("url")
                writer.WriteValue(value.Url)
            End If
            If value.Body IsNot Nothing Then
                writer.WritePropertyName("body")
                WriteTextRequestBase(writer, value.Body)
            End If

            writer.WriteEndObject()
        End Sub ' WriteBatchChatItem

        ''' <summary>
        ''' Writes <see cref="TextRequestBase"/> to JsonWriter.
        ''' </summary>
        Private Shared Sub WriteTextRequestBase(writer As Global.Newtonsoft.Json.JsonWriter, value As TextRequestBase)
            TextRequestWriter.WriteTextRequestBase(writer, value)
        End Sub ' WriteTextRequestBase

    End Class ' BatchChatItemWriter
End Namespace
