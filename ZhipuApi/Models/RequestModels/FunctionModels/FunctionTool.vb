Namespace Models
    Public Class FunctionTool
        Public Const TypeFunction = "function"
        Public Const TypeRetrieval = "retrieval"
        Public Const TypeWebSearch = "web_search"
        ''' <summary>
        ''' function、retrieval、web_search
        ''' </summary>
        Public Property Type As String = TypeFunction

        Public Property [Function] As FunctionMetadata

        Private Sub EnsureFunction()
            If [Function] Is Nothing Then
                [Function] = New FunctionMetadata
            End If
        End Sub

        ''' <summary>
        ''' The name of the function.
        ''' </summary>
        ''' <remarks>
        ''' This is a migration compatibility member for the official C# SDK. For more advanced cases, use <see cref="[Function]"/> property.
        ''' </remarks>
        Public Property Name As String
            Get
                Return [Function]?.Name
            End Get
            Set(name As String)
                If name Is Nothing AndAlso [Function] Is Nothing Then Return
                EnsureFunction()
                [Function].Name = name
            End Set
        End Property

        ''' <summary>
        ''' The description of the function.
        ''' </summary>
        ''' <remarks>
        ''' This is a migration compatibility member for the official C# SDK. For more advanced cases, use <see cref="[Function]"/> property.
        ''' </remarks>
        Public Property Description As String
            Get
                Return [Function]?.Description
            End Get
            Set(desc As String)
                If Name Is Nothing AndAlso [Function] Is Nothing Then Return
                EnsureFunction()
                [Function].Description = desc
            End Set
        End Property

        ''' <summary>
        ''' The parameters of the function.
        ''' </summary>
        ''' <remarks>
        ''' This is a migration compatibility member for the official C# SDK. For more advanced cases, use <see cref="[Function]"/> property.
        ''' </remarks>
        Public Property Parameters As FunctionParameters
            Get
                Return [Function]?.Parameters
            End Get
            Set(param As FunctionParameters)
                If Name Is Nothing AndAlso [Function] Is Nothing Then Return
                EnsureFunction()
                [Function].Parameters = param
            End Set
        End Property

        Public Property Retrieval As RetrievalMetadata
        Public Property WebSearch As WebSearchMetadata
    End Class

End Namespace
