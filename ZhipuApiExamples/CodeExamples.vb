Imports System.Text
Imports Microsoft.Extensions.AI
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Nukepayload2.AI.Providers.Zhipu
Imports Nukepayload2.AI.Providers.Zhipu.Models

<TestClass>
Public Class CodeExamples
    Private Shared ReadOnly Property ApiKey As String
        Get
            Dim key = Environment.GetEnvironmentVariable("ZHIPU_API_KEY")
            If key = Nothing Then
                Throw New InvalidOperationException("Please set API key to %ZHIPU_API_KEY% and try again.")
            End If
            Return key
        End Get
    End Property

    <TestMethod>
    Async Function TestCompletionAsync() As Task
        Dim clientV4 As New ClientV4(ApiKey)
        Dim request As New TextRequestBase With {
            .Model = "glm-4-flash",
            .Messages = {New MessageItem("user", "你好，你是谁？")},
            .Temperature = 0.7,
            .TopP = 0.7
        }
        Dim response = Await clientV4.Chat.CompleteAsync(request)

        Dim respMessage = response.Choices?.FirstOrDefault?.Message?.Content
        Console.WriteLine(respMessage)
        Assert.IsNotNull(respMessage)
    End Function

    <TestMethod>
    Async Function TestMicrosoftAICompletionAsync() As Task
        Dim clientV4 As New ClientV4(ApiKey)
        Dim response = Await clientV4.Chat.AsChatClient("glm-4-flash").CompleteAsync(
            {New ChatMessage(ChatRole.User, "你好，你是谁？")},
             New ChatOptions With {.Temperature = 0.7F, .TopP = 0.7F}
        )

        Dim respMessage = response.Choices?.FirstOrDefault?.Text
        Console.WriteLine(respMessage)
        Assert.IsNotNull(respMessage)
    End Function

    <TestMethod>
    Async Function TestStreamAsync() As Task
        Dim clientV4 As New ClientV4(ApiKey)
        Dim sb As New StringBuilder
        Await clientV4.Chat.StreamAsync(
            New TextRequestBase With {
                .Model = "glm-4-flash",
                .Messages = {New MessageItem("user", "1+1等于多少"),
                              New MessageItem("assistant", "1+1等于2。"),
                              New MessageItem("user", "再加2呢？")},
                .Temperature = 0.7,
                .TopP = 0.7,
                .Stream = True
            },
            Sub(resp)
                Dim respMessage = resp.Choices?.FirstOrDefault?.Delta?.Content
                If respMessage <> Nothing Then
                    sb.AppendLine($"{Environment.TickCount}: {respMessage}")
                End If
            End Sub)

        Console.WriteLine(sb.ToString)
        Assert.AreNotEqual(0, sb.Length)
    End Function

    <TestMethod>
    Async Function TestMicrosoftAIStreamAsync() As Task
        Dim clientV4 As New ClientV4(ApiKey)
        Dim messages = {New ChatMessage(ChatRole.User, "1+1等于多少"),
                        New ChatMessage(ChatRole.Assistant, "1+1等于2。"),
                        New ChatMessage(ChatRole.User, "再加2呢？")}
        Dim options As New ChatOptions With {.Temperature = 0.7F, .TopP = 0.7F}
        Dim respAsyncEnumerate = clientV4.Chat.AsChatClient("glm-4-flash").CompleteStreamingAsync(messages, options)
        ' 在 VB 支持简化的 IAsyncEnumerable 调用 (Await Each 语句) 之前可以用 System.Linq.Async 读取服务端回复的数据。
        Dim sb As New StringBuilder
        Await respAsyncEnumerate.ForEachAsync(
        Sub(update)
            Dim respMessage = update.Text
            If respMessage <> Nothing Then
                sb.AppendLine($"{Environment.TickCount}: {respMessage}")
            End If
        End Sub)

        Console.WriteLine(sb.ToString)
        Assert.AreNotEqual(0, sb.Length)
    End Function

    <TestMethod>
    Async Function TestToolCallAsync() As Task
        Dim clientV4 As New ClientV4(ApiKey)
        Dim response = Await clientV4.Chat.CompleteAsync(
            New TextRequestBase With {
                .Model = "glm-4-flash",
                .Messages = {New MessageItem("user", "北京今天的天气如何？")},
                .Tools = {
                    New FunctionTool With {
                        .Name = "get_weather",
                        .Description = "根据提供的城市名称，提供未来的天气数据",
                        .Parameters = New FunctionParameters With {
                            .Required = {"city"}
                        }.
                        AddParameter("city", ParameterType.String, "搜索的城市名称").
                        AddParameter("days", ParameterType.Integer, "要查询的未来的天数，默认为0")
                    }
                },
                .ToolChoice = "auto",
                .Temperature = 0.7,
                .TopP = 0.7
            }
        )

        Dim respMessage = response.Choices?.FirstOrDefault?.Message?.ToolCalls?.FirstOrDefault.Function
        Assert.AreEqual("get_weather", respMessage?.Name)
        Assert.AreEqual("{""city"":""北京"",""days"":0}", respMessage?.Arguments)
    End Function

    <TestMethod>
    Async Function TestImageGenerationAsync() As Task
        Dim clientV4 As New ClientV4(ApiKey)
        Dim response = Await clientV4.Images.GenerateAsync(New ImageRequestBase With {
            .Model = "cogview",
            .Prompt = "一只可爱的科幻风格小猫咪"
        })
        Dim respMessage = response.Data?.FirstOrDefault?.Url
        Assert.IsNotNull(respMessage)
    End Function

    <TestMethod>
    Public Async Function TestImageRecognitionAsync() As Task
        Dim clientV4 As New ClientV4(ApiKey)
        Dim response = Await clientV4.Chat.CompleteAsync(New TextRequestBase With {
            .Model = "cogvlm_28b",
            .Messages = {
                New ImageToTextMessageItem("user") With {
                    .Text = "这是什么",
                    .ImageUrl = "<image_url>"
                }
            },
            .Temperature = 0.7,
            .TopP = 0.7
        })

        Dim respMessage = response.Choices?.FirstOrDefault?.Message?.Content
        Assert.IsNotNull(respMessage)
    End Function

    <TestMethod>
    Public Async Function TestImageRecognitionStreamAsync() As Task
        Dim sb As New StringBuilder
        Dim clientV4 As New ClientV4(ApiKey)
        Await clientV4.Chat.StreamAsync(
            New TextRequestBase With {
                .Model = "cogvlm_28b",
                .Messages = {
                    New ImageToTextMessageItem("user") With {
                        .Text = "这是什么",
                        .ImageUrl = "<image_url>"
                    }
                },
                .Temperature = 0.7,
                .TopP = 0.7,
                .Stream = True
            },
            Sub(resp)
                Dim respMessage = resp.Choices?.FirstOrDefault?.Delta?.Content
                If respMessage <> Nothing Then
                    sb.Append(respMessage)
                End If
            End Sub)

        Assert.AreNotEqual(0, sb.Length)
    End Function

    <TestMethod>
    Public Async Function TestEmbeddingAsync() As Task
        Dim clientV4 As New ClientV4(ApiKey)
        Dim response = Await clientV4.Embeddings.ProcessAsync(New EmbeddingRequestBase With {
            .Model = "embedding-2",
            .Input = "一只可爱的科幻风格小猫咪"
        })

        Dim firstEmbedding = response.Data?.FirstOrDefault?.Embedding
        Assert.IsNotNull(firstEmbedding)
    End Function
End Class
