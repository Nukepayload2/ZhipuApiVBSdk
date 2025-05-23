' <auto-generated/>
' This source file is part of the JSON serialization code and is not intended to be modified by you.
' Re-run the source generator will overwrite your changes in this file.
' Generated by: Nukepayload2.IO.Json.Serialization.NewtonsoftJson
Option Strict On
Option Infer On
Option Explicit On
Option Compare Binary
Imports Nukepayload2.AI.Providers.Zhipu.Models
Imports System.IO
Imports Newtonsoft.Json.Linq
Imports Nukepayload2.IO.Json.Serialization.NewtonsoftJson

Namespace Serialization
    Partial Class FileResponseReader
        ''' <summary>
        ''' Reads <see cref="FileItem"/> from JsonReader.
        ''' </summary>
        Public Shared Function ReadFileItem(reader As Global.Newtonsoft.Json.JsonReader, readState As JsonReadErrorHandler) As FileItem
            If reader.TokenType = Global.Newtonsoft.Json.JsonToken.None Then
                reader.Read()
            End If
        
            If reader.TokenType <> Global.Newtonsoft.Json.JsonToken.StartObject Then
                readState.OnConflictingTokenType("FileItem", JsonReadErrorHandler.Positions.StartObject, reader)
                Return Nothing
            End If
        
            Dim entity As New FileItem
        
            Dim startDepth As Integer = reader.Depth
            If Not reader.Read() Then
                Throw readState.OnUnrecoverableError("Error reading from JsonReader. File was truncated.")
            End If
        
            Do
                Select Case reader.TokenType
                    Case Global.Newtonsoft.Json.JsonToken.PropertyName
                        Dim name As String = CType(reader.Value, String)
                        Select Case name
                            Case "id"
                                If Not reader.Read() Then
                                    Throw readState.OnUnrecoverableError("Error reading from JsonReader. File was truncated.")
                                End If
                                If reader.TokenType = Global.Newtonsoft.Json.JsonToken.Null Then
                                    entity.Id = Nothing
                                ElseIf reader.TokenType = Global.Newtonsoft.Json.JsonToken.StartObject Or reader.TokenType = Global.Newtonsoft.Json.JsonToken.StartArray Then
                                    readState.OnConflictingTokenType("FileItem", name, reader)
                                Else
                                    entity.Id = Convert.ToString(reader.Value)
                                End If
                            Case "object"
                                If Not reader.Read() Then
                                    Throw readState.OnUnrecoverableError("Error reading from JsonReader. File was truncated.")
                                End If
                                If reader.TokenType = Global.Newtonsoft.Json.JsonToken.Null Then
                                    entity.TypeName = Nothing
                                ElseIf reader.TokenType = Global.Newtonsoft.Json.JsonToken.StartObject Or reader.TokenType = Global.Newtonsoft.Json.JsonToken.StartArray Then
                                    readState.OnConflictingTokenType("FileItem", name, reader)
                                Else
                                    entity.TypeName = Convert.ToString(reader.Value)
                                End If
                            Case "bytes"
                                If Not reader.Read() Then
                                    Throw readState.OnUnrecoverableError("Error reading from JsonReader. File was truncated.")
                                End If
                                If reader.TokenType = Global.Newtonsoft.Json.JsonToken.Null Then
                                    entity.Bytes = Nothing
                                ElseIf reader.TokenType = Global.Newtonsoft.Json.JsonToken.StartObject Or reader.TokenType = Global.Newtonsoft.Json.JsonToken.StartArray Then
                                    readState.OnConflictingTokenType("FileItem", name, reader)
                                Else
                                    entity.Bytes = Convert.ToInt64(reader.Value)
                                End If
                            Case "created_at"
                                If Not reader.Read() Then
                                    Throw readState.OnUnrecoverableError("Error reading from JsonReader. File was truncated.")
                                End If
                                If reader.TokenType = Global.Newtonsoft.Json.JsonToken.Null Then
                                    entity.CreatedAt = Nothing
                                ElseIf reader.TokenType = Global.Newtonsoft.Json.JsonToken.StartObject Or reader.TokenType = Global.Newtonsoft.Json.JsonToken.StartArray Then
                                    readState.OnConflictingTokenType("FileItem", name, reader)
                                Else
                                    entity.CreatedAt = Convert.ToInt64(reader.Value)
                                End If
                            Case "filename"
                                If Not reader.Read() Then
                                    Throw readState.OnUnrecoverableError("Error reading from JsonReader. File was truncated.")
                                End If
                                If reader.TokenType = Global.Newtonsoft.Json.JsonToken.Null Then
                                    entity.Filename = Nothing
                                ElseIf reader.TokenType = Global.Newtonsoft.Json.JsonToken.StartObject Or reader.TokenType = Global.Newtonsoft.Json.JsonToken.StartArray Then
                                    readState.OnConflictingTokenType("FileItem", name, reader)
                                Else
                                    entity.Filename = Convert.ToString(reader.Value)
                                End If
                            Case "purpose"
                                If Not reader.Read() Then
                                    Throw readState.OnUnrecoverableError("Error reading from JsonReader. File was truncated.")
                                End If
                                If reader.TokenType = Global.Newtonsoft.Json.JsonToken.Null Then
                                    entity.Purpose = Nothing
                                ElseIf reader.TokenType = Global.Newtonsoft.Json.JsonToken.StartObject Or reader.TokenType = Global.Newtonsoft.Json.JsonToken.StartArray Then
                                    readState.OnConflictingTokenType("FileItem", name, reader)
                                Else
                                    entity.Purpose = Convert.ToString(reader.Value)
                                End If
                            Case "status"
                                If Not reader.Read() Then
                                    Throw readState.OnUnrecoverableError("Error reading from JsonReader. File was truncated.")
                                End If
                                If reader.TokenType = Global.Newtonsoft.Json.JsonToken.Null Then
                                    entity.Status = Nothing
                                ElseIf reader.TokenType = Global.Newtonsoft.Json.JsonToken.StartObject Or reader.TokenType = Global.Newtonsoft.Json.JsonToken.StartArray Then
                                    readState.OnConflictingTokenType("FileItem", name, reader)
                                Else
                                    entity.Status = Convert.ToString(reader.Value)
                                End If
                            Case "status_details"
                                If Not reader.Read() Then
                                    Throw readState.OnUnrecoverableError("Error reading from JsonReader. File was truncated.")
                                End If
                                If reader.TokenType = Global.Newtonsoft.Json.JsonToken.Null Then
                                    entity.StatusDetails = Nothing
                                ElseIf reader.TokenType = Global.Newtonsoft.Json.JsonToken.StartObject Or reader.TokenType = Global.Newtonsoft.Json.JsonToken.StartArray Then
                                    readState.OnConflictingTokenType("FileItem", name, reader)
                                Else
                                    entity.StatusDetails = Convert.ToString(reader.Value)
                                End If
                            Case Else
                                readState.OnMissingProperty("FileItem", name, reader)
                        End Select
                    Case Global.Newtonsoft.Json.JsonToken.EndObject
                        Exit Do
                    Case Else
                        Throw readState.OnUnrecoverableError("Unexpected token while loading JObject. The reader is at unexpected position.")
                End Select
            Loop While reader.Read()
        
            Dim endDepth As Integer = reader.Depth
            If endDepth <> startDepth Then
                Throw readState.OnUnrecoverableError("Error reading from JsonReader. The reader is at unexpected position.")
            End If
        
            Return entity
        End Function ' ReadFileItem
        ''' <summary>
        ''' Reads <see cref="FileDeleteResponse"/> from JsonReader.
        ''' </summary>
        Public Shared Function ReadFileDeleteResponse(reader As Global.Newtonsoft.Json.JsonReader, readState As JsonReadErrorHandler) As FileDeleteResponse
            If reader.TokenType = Global.Newtonsoft.Json.JsonToken.None Then
                reader.Read()
            End If
        
            If reader.TokenType <> Global.Newtonsoft.Json.JsonToken.StartObject Then
                readState.OnConflictingTokenType("FileDeleteResponse", JsonReadErrorHandler.Positions.StartObject, reader)
                Return Nothing
            End If
        
            Dim entity As New FileDeleteResponse
        
            Dim startDepth As Integer = reader.Depth
            If Not reader.Read() Then
                Throw readState.OnUnrecoverableError("Error reading from JsonReader. File was truncated.")
            End If
        
            Do
                Select Case reader.TokenType
                    Case Global.Newtonsoft.Json.JsonToken.PropertyName
                        Dim name As String = CType(reader.Value, String)
                        Select Case name
                            Case "id"
                                If Not reader.Read() Then
                                    Throw readState.OnUnrecoverableError("Error reading from JsonReader. File was truncated.")
                                End If
                                If reader.TokenType = Global.Newtonsoft.Json.JsonToken.Null Then
                                    entity.Id = Nothing
                                ElseIf reader.TokenType = Global.Newtonsoft.Json.JsonToken.StartObject Or reader.TokenType = Global.Newtonsoft.Json.JsonToken.StartArray Then
                                    readState.OnConflictingTokenType("FileDeleteResponse", name, reader)
                                Else
                                    entity.Id = Convert.ToString(reader.Value)
                                End If
                            Case "deleted"
                                If Not reader.Read() Then
                                    Throw readState.OnUnrecoverableError("Error reading from JsonReader. File was truncated.")
                                End If
                                If reader.TokenType = Global.Newtonsoft.Json.JsonToken.Null Then
                                    entity.Deleted = Nothing
                                ElseIf reader.TokenType = Global.Newtonsoft.Json.JsonToken.StartObject Or reader.TokenType = Global.Newtonsoft.Json.JsonToken.StartArray Then
                                    readState.OnConflictingTokenType("FileDeleteResponse", name, reader)
                                Else
                                    entity.Deleted = Convert.ToBoolean(reader.Value)
                                End If
                            Case "object"
                                If Not reader.Read() Then
                                    Throw readState.OnUnrecoverableError("Error reading from JsonReader. File was truncated.")
                                End If
                                If reader.TokenType = Global.Newtonsoft.Json.JsonToken.Null Then
                                    entity.TypeName = Nothing
                                ElseIf reader.TokenType = Global.Newtonsoft.Json.JsonToken.StartObject Or reader.TokenType = Global.Newtonsoft.Json.JsonToken.StartArray Then
                                    readState.OnConflictingTokenType("FileDeleteResponse", name, reader)
                                Else
                                    entity.TypeName = Convert.ToString(reader.Value)
                                End If
                            Case Else
                                readState.OnMissingProperty("FileDeleteResponse", name, reader)
                        End Select
                    Case Global.Newtonsoft.Json.JsonToken.EndObject
                        Exit Do
                    Case Else
                        Throw readState.OnUnrecoverableError("Unexpected token while loading JObject. The reader is at unexpected position.")
                End Select
            Loop While reader.Read()
        
            Dim endDepth As Integer = reader.Depth
            If endDepth <> startDepth Then
                Throw readState.OnUnrecoverableError("Error reading from JsonReader. The reader is at unexpected position.")
            End If
        
            Return entity
        End Function ' ReadFileDeleteResponse
        ''' <summary>
        ''' Reads <see cref="FilesListResponse"/> from JsonReader.
        ''' </summary>
        Public Shared Function ReadFilesListResponse(reader As Global.Newtonsoft.Json.JsonReader, readState As JsonReadErrorHandler) As FilesListResponse
            If reader.TokenType = Global.Newtonsoft.Json.JsonToken.None Then
                reader.Read()
            End If
        
            If reader.TokenType <> Global.Newtonsoft.Json.JsonToken.StartObject Then
                readState.OnConflictingTokenType("FilesListResponse", JsonReadErrorHandler.Positions.StartObject, reader)
                Return Nothing
            End If
        
            Dim entity As New FilesListResponse
        
            Dim startDepth As Integer = reader.Depth
            If Not reader.Read() Then
                Throw readState.OnUnrecoverableError("Error reading from JsonReader. File was truncated.")
            End If
        
            Do
                Select Case reader.TokenType
                    Case Global.Newtonsoft.Json.JsonToken.PropertyName
                        Dim name As String = CType(reader.Value, String)
                        Select Case name
                            Case "object"
                                If Not reader.Read() Then
                                    Throw readState.OnUnrecoverableError("Error reading from JsonReader. File was truncated.")
                                End If
                                If reader.TokenType = Global.Newtonsoft.Json.JsonToken.Null Then
                                    entity.TypeName = Nothing
                                ElseIf reader.TokenType = Global.Newtonsoft.Json.JsonToken.StartObject Or reader.TokenType = Global.Newtonsoft.Json.JsonToken.StartArray Then
                                    readState.OnConflictingTokenType("FilesListResponse", name, reader)
                                Else
                                    entity.TypeName = Convert.ToString(reader.Value)
                                End If
                            Case "data"
                                If Not reader.Read() Then
                                    Throw readState.OnUnrecoverableError("Error reading from JsonReader. File was truncated.")
                                End If
                                If reader.TokenType = Global.Newtonsoft.Json.JsonToken.Null Then
                                    entity.Data = Nothing
                                Else
                                    entity.Data = ReadIReadOnlyListOfFileItem(reader, readState)
                                End If
                            Case "next_page"
                                If Not reader.Read() Then
                                    Throw readState.OnUnrecoverableError("Error reading from JsonReader. File was truncated.")
                                End If
                                If reader.TokenType = Global.Newtonsoft.Json.JsonToken.Null Then
                                    entity.NextPage = Nothing
                                ElseIf reader.TokenType = Global.Newtonsoft.Json.JsonToken.StartObject Or reader.TokenType = Global.Newtonsoft.Json.JsonToken.StartArray Then
                                    readState.OnConflictingTokenType("FilesListResponse", name, reader)
                                Else
                                    entity.NextPage = Convert.ToString(reader.Value)
                                End If
                            Case Else
                                readState.OnMissingProperty("FilesListResponse", name, reader)
                        End Select
                    Case Global.Newtonsoft.Json.JsonToken.EndObject
                        Exit Do
                    Case Else
                        Throw readState.OnUnrecoverableError("Unexpected token while loading JObject. The reader is at unexpected position.")
                End Select
            Loop While reader.Read()
        
            Dim endDepth As Integer = reader.Depth
            If endDepth <> startDepth Then
                Throw readState.OnUnrecoverableError("Error reading from JsonReader. The reader is at unexpected position.")
            End If
        
            Return entity
        End Function ' ReadFilesListResponse
        ''' <summary>
        ''' Reads array of <see cref="FileItem"/> from JsonReader.
        ''' </summary>
        Private Shared Function ReadIReadOnlyListOfFileItem(reader As Global.Newtonsoft.Json.JsonReader, readState As JsonReadErrorHandler) As IReadOnlyList(Of FileItem)
            If reader.TokenType = Global.Newtonsoft.Json.JsonToken.None Then
                If Not reader.Read() Then
                    Throw readState.OnUnrecoverableError("Error reading JArray from JsonReader.")
                End If
            End If
        
            If reader.TokenType <> Global.Newtonsoft.Json.JsonToken.StartArray Then
                readState.OnConflictingTokenType("IReadOnlyListOfFileItem", JsonReadErrorHandler.Positions.StartArray, reader)
                Return Nothing
            End If
        
            Dim entityList As New List(Of FileItem)
            Dim startDepth As Integer = reader.Depth
        
            If Not reader.Read() Then
                Throw readState.OnUnrecoverableError("Error reading from JsonReader. File was truncated.")
            End If
        
            Do
                Dim item As FileItem
                Select Case reader.TokenType
                    Case Global.Newtonsoft.Json.JsonToken.StartObject
                        item = ReadFileItem(reader, readState)
                    Case Global.Newtonsoft.Json.JsonToken.Null
                        item = Nothing
                    Case Global.Newtonsoft.Json.JsonToken.EndArray
                        Exit Do
                    Case Else
                        readState.OnConflictingTokenType("IReadOnlyListOfFileItem", JsonReadErrorHandler.Positions.ArrayElement, reader)
                        item = Nothing
                End Select
                entityList.Add(item)
            Loop While reader.Read()
        
            Dim endDepth As Integer = reader.Depth
        
            If endDepth <> startDepth Then
                Throw readState.OnUnrecoverableError("Error reading from JsonReader. The reader is at unexpected position.")
            End If
        
            Return entityList
        End Function

    End Class ' FileResponseReader
End Namespace
