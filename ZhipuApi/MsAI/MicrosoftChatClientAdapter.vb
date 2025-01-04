Imports System.Runtime.CompilerServices
Imports System.Threading
Imports Microsoft.Extensions.AI
Imports Nukepayload2.AI.Providers.Zhipu.Models

Public Module ClientAdapterExtensions
    <Extension>
    Public Function AsChatClient(client As Chat, modelId As String) As IChatClient
        Return New MicrosoftChatClientAdapter(client, modelId)
    End Function
End Module

Public Class MicrosoftChatClientAdapter
    Implements IChatClient

    Private disposedValue As Boolean

    Sub New(client As Chat, modelId As String)
        If client Is Nothing Then
            Throw New ArgumentNullException(NameOf(client))
        End If
        If String.IsNullOrEmpty(modelId) Then
            Throw New ArgumentException($"""{NameOf(modelId)}""不能为 null 或空。", NameOf(modelId))
        End If

        Me.Client = client
        Metadata = New ChatClientMetadata("ZhipuV4",
                    New Uri("https://open.bigmodel.cn/api/paas/v4/"),
                    modelId)
    End Sub

    Public ReadOnly Property Client As Chat

    Public ReadOnly Property Metadata As ChatClientMetadata Implements IChatClient.Metadata

    Public Async Function CompleteAsync(chatMessages As IList(Of ChatMessage), Optional options As ChatOptions = Nothing, Optional cancellationToken As CancellationToken = Nothing) As Task(Of ChatCompletion) Implements IChatClient.CompleteAsync
        Dim request As New TextRequestBase With {
            .Model = Metadata.ModelId,
            .Messages = (From msg In chatMessages
                         Select New MessageItem(msg.Role.Value, msg.Text)).ToArray,
            .Temperature = ToDoubleWithRounding(options.Temperature),
            .TopP = ToDoubleWithRounding(options.TopP),
            .ToolChoice = ConvertToolChoice(options.Tools, options.ToolMode),
            .Tools = ConvertTools(options.Tools)
        }

        Dim response = Await Client.CompleteAsync(request, cancellationToken)
        ThrowForNonSuccessResponse(response)
        Return New ChatCompletion(
            (From choice In response.Choices
             Let msg = choice.Message
             Select New ChatMessage(New ChatRole(msg.Role), msg.Content)
            ).ToArray)
    End Function

    Private Shared Sub ThrowForNonSuccessResponse(response As ResponseBase)
        If response.Error IsNot Nothing AndAlso response.Error.Count > 0 Then
            Throw New InvalidOperationException($"错误 {If(response.Error!code, "???")}: {If(response.Error!message, "未指定的错误")}")
        End If
    End Sub

    Private Function ToDoubleWithRounding(value As Single?) As Double?
        If value Is Nothing Then
            Return Nothing
        End If
        Return Math.Round(CDbl(value.Value), 2)
    End Function

    Private Function ConvertTools(tools As IList(Of AITool)) As FunctionTool()
        Return Nothing
    End Function

    Private Function ConvertToolChoice(tools As IList(Of AITool), toolMode As ChatToolMode) As String
        If tools Is Nothing Then
            Return Nothing
        End If
        If toolMode Is ChatToolMode.Auto Then
            Return "auto"
        End If
        Return Nothing
    End Function

    Public Function CompleteStreamingAsync(chatMessages As IList(Of ChatMessage),
                                           Optional options As ChatOptions = Nothing,
                                           Optional cancellationToken As CancellationToken = Nothing
                                           ) As IAsyncEnumerable(Of StreamingChatCompletionUpdate) Implements IChatClient.CompleteStreamingAsync
        Dim builder As New AsyncEnumerableAdapter(Of StreamingChatCompletionUpdate).Builder With {
            .ReturnAsync = Function(enumerator) Client.StreamAsync(
                New TextRequestBase With {
                    .Model = Metadata.ModelId,
                    .Messages = (From msg In chatMessages
                                 Select New MessageItem(msg.Role.Value, msg.Text)).ToArray,
                    .Temperature = ToDoubleWithRounding(options.Temperature),
                    .TopP = ToDoubleWithRounding(options.TopP),
                    .ToolChoice = ConvertToolChoice(options.Tools, options.ToolMode),
                    .Tools = ConvertTools(options.Tools),
                    .Stream = True
                },
                Sub(resp)
                    ThrowForNonSuccessResponse(resp)
                    Dim respMessage = resp.Choices?.FirstOrDefault?.Delta?.Content
                    If respMessage <> Nothing Then
                        Dim converted As New StreamingChatCompletionUpdate With {.Text = respMessage}
                        enumerator.YieldValue(converted)
                    End If
                End Sub,
                cancellationToken
            )
        }
        Return builder.Build()
    End Function

    Public Function GetService901(serviceType As Type, Optional serviceKey As Object = Nothing) As Object
        Return Nothing
    End Function

    Public Function GetService900(Of TService As Class)(Optional key As Object = Nothing) As TService Implements IChatClient.GetService
        Return Nothing
    End Function

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: 释放托管状态(托管对象)
            End If

            ' TODO: 释放未托管的资源(未托管的对象)并重写终结器
            ' TODO: 将大型字段设置为 null
            disposedValue = True
        End If
    End Sub

    ' ' TODO: 仅当“Dispose(disposing As Boolean)”拥有用于释放未托管资源的代码时才替代终结器
    ' Protected Overrides Sub Finalize()
    '     ' 不要更改此代码。请将清理代码放入“Dispose(disposing As Boolean)”方法中
    '     Dispose(disposing:=False)
    '     MyBase.Finalize()
    ' End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' 不要更改此代码。请将清理代码放入“Dispose(disposing As Boolean)”方法中
        Dispose(disposing:=True)
        GC.SuppressFinalize(Me)
    End Sub

End Class
