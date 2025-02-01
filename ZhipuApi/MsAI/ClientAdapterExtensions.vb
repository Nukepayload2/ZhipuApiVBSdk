Imports System.Runtime.CompilerServices
Imports Microsoft.Extensions.AI

Public Module ClientAdapterExtensions
    <Extension>
    Public Function AsChatClient(client As Chat, modelId As String) As IChatClient
        Return New MicrosoftChatClientAdapter(client, modelId)
    End Function
End Module
