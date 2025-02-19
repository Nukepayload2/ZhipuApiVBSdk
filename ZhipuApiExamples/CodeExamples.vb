Imports System.Text
Imports System.Threading
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
    Async Function TestMicrosoftAICompletionNoOptionsAsync() As Task
        Dim clientV4 As New ClientV4(ApiKey)
        Dim response = Await clientV4.Chat.AsChatClient("glm-4-flash").CompleteAsync(
            {New ChatMessage(ChatRole.User, "你好，你是谁？")}
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
    Async Function TestStreamErrorHandlingAsync() As Task
        Dim clientV4 As New ClientV4(ApiKey)
        Dim sb As New StringBuilder
        Await clientV4.Chat.StreamAsync(
            New TextRequestBase With {
                .Model = "glm-wrong-model-name",
                .Messages = {New MessageItem("user", "1+1等于多少"),
                              New MessageItem("assistant", "1+1等于2。"),
                              New MessageItem("user", "再加2呢？")},
                .Temperature = 0.7,
                .TopP = 0.7,
                .Stream = True
            },
            Sub(resp)
                If resp.Error IsNot Nothing Then
                    sb.Append(String.Join(vbCrLf, From err In resp.Error Select $"{err.Key}: {err.Value}"))
                    Return
                End If
                Dim respMessage = resp.Choices?.FirstOrDefault?.Delta?.Content
                If respMessage <> Nothing Then
                    sb.AppendLine($"{Environment.TickCount}: {respMessage}")
                End If
            End Sub)

        Dim errMsg = sb.ToString
        Console.WriteLine(errMsg)
        Assert.IsTrue(errMsg.Contains("模型不存在"))
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
    Async Function TestMicrosoftAIStreamNoOptionsAsync() As Task
        Dim clientV4 As New ClientV4(ApiKey)
        Dim messages = {New ChatMessage(ChatRole.User, "1+1等于多少"),
                        New ChatMessage(ChatRole.Assistant, "1+1等于2。"),
                        New ChatMessage(ChatRole.User, "再加2呢？")}
        Dim respAsyncEnumerate = clientV4.Chat.AsChatClient("glm-4-flash").CompleteStreamingAsync(messages)
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
                .Messages = {
                    New MessageItem("system", "不要假设或猜测传入函数的参数值。如果用户的描述不明确，请要求用户提供必要信息"),
                    New MessageItem("user", "能帮我查天气吗？"),
                    New MessageItem("assistant", "好的，请告诉我您所在的城市名称。"),
                    New MessageItem("user", "北京"),
                    New MessageItem("assistant", "您需要查询未来几天的天气呢？"),
                    New MessageItem("user", "就今天一天的")
                },
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
        ' 这个模型工具调用比较蠢，有时候不会返回工具调用信息，得手动多试几次。
        Dim respMessage = response.Choices?.FirstOrDefault?.Message?.ToolCalls?.FirstOrDefault.Function
        Assert.AreEqual("get_weather", respMessage?.Name)
        Assert.AreEqual("{""city"": ""北京"", ""days"": 1}", respMessage?.Arguments)
    End Function

    Private Class AIGetWeather
        Inherits AIFunction

        Public Overrides ReadOnly Property Metadata As New AIFunctionMetadata("get_weather") With {
            .Description = "根据提供的城市名称，提供未来的天气数据",
            .Parameters = {
                New AIFunctionParameterMetadata("city") With {
                    .IsRequired = True, .Description = "搜索的城市名称", .ParameterType = GetType(String)
                },
                New AIFunctionParameterMetadata("days") With {
                    .Description = "要查询的未来的天数，默认为0",
                    .DefaultValue = 0, .HasDefaultValue = True,
                    .ParameterType = GetType(Integer)
                }
            }
        }

        Public ReadOnly Property CallLogForTest As New List(Of IEnumerable(Of KeyValuePair(Of String, Object)))

        Protected Overrides Function InvokeCoreAsync(arguments As IEnumerable(Of KeyValuePair(Of String, Object)), cancellationToken As Threading.CancellationToken) As Task(Of Object)
            ' 假的实现，用于测试
            CallLogForTest.Add(arguments)
            Return Task.FromResult(CObj("晴天，30 摄氏度。"))
        End Function
    End Class

    <TestMethod>
    Async Function TestMicrosoftAIToolCallAsync() As Task
        Dim clientV4 As New ClientV4(ApiKey)
        Dim tool As New AIGetWeather
        Dim options As New ChatOptions With {
            .Temperature = 0.7F, .TopP = 0.7F, .ToolMode = ChatToolMode.Auto,
            .Tools = {tool}
        }
        ' 注意：工具调用情况下，聊天记录必须是可修改的，因为要插入工具调用返回的值。
        Dim response = Await clientV4.Chat.AsChatClient("glm-4-flash").CompleteAsync(
            New List(Of ChatMessage) From {
                New ChatMessage(ChatRole.System, "不要假设或猜测传入函数的参数值。如果用户的描述不明确，请要求用户提供必要信息"),
                New ChatMessage(ChatRole.User, "能帮我查天气吗？"),
                New ChatMessage(ChatRole.Assistant, "好的，请告诉我您所在的城市名称。"),
                New ChatMessage(ChatRole.User, "北京"),
                New ChatMessage(ChatRole.Assistant, "您需要查询未来几天的天气呢？"),
                New ChatMessage(ChatRole.User, "就今天一天的")
            }, options
        )

        Dim respMessage = response.Choices?.FirstOrDefault?.Text
        Console.WriteLine(respMessage)
        Assert.IsTrue(respMessage.Contains("晴天"))
        Assert.IsTrue(respMessage.Contains("30"))

        ' 这个模型工具调用比较蠢，有时候不会返回工具调用信息，有时候又会调用好几次。
        ' .NET 9 可以省略 ToDictionary 参数
        Dim firstCall = tool.CallLogForTest(0).ToDictionary(Function(it) it.Key, Function(it) it.Value)
        Dim city = CStr(firstCall!city)
        Dim days = CInt(firstCall!days)
        Assert.AreEqual("北京", city)
        Assert.AreEqual(1, days)
    End Function

    <TestMethod>
    Async Function TestMicrosoftAIToolCallStreamingAsync() As Task
        Dim clientV4 As New ClientV4(ApiKey)
        Dim tool As New AIGetWeather
        Dim options As New ChatOptions With {
            .Temperature = 0.7F, .TopP = 0.7F, .ToolMode = ChatToolMode.Auto,
            .Tools = {tool}
        }
        ' 注意：工具调用情况下，聊天记录必须是可修改的，因为要插入工具调用返回的值。
        Dim respAsyncEnumerate = clientV4.Chat.AsChatClient("glm-4-flash").CompleteStreamingAsync(
            New List(Of ChatMessage) From {
                New ChatMessage(ChatRole.System, "不要假设或猜测传入函数的参数值。如果用户的描述不明确，请要求用户提供必要信息"),
                New ChatMessage(ChatRole.User, "能帮我查天气吗？"),
                New ChatMessage(ChatRole.Assistant, "好的，请告诉我您所在的城市名称。"),
                New ChatMessage(ChatRole.User, "北京"),
                New ChatMessage(ChatRole.Assistant, "您需要查询未来几天的天气呢？"),
                New ChatMessage(ChatRole.User, "就今天一天的")
            }, options
        )

        ' 在 VB 支持简化的 IAsyncEnumerable 调用 (Await Each 语句) 之前可以用 System.Linq.Async 读取服务端回复的数据。
        Dim respLog As New StringBuilder
        Dim respText As New StringBuilder
        Await respAsyncEnumerate.ForEachAsync(
        Sub(update)
            Dim resp = update.Text
            If resp <> Nothing Then
                respLog.AppendLine($"{Environment.TickCount}: {resp}")
                respText.AppendLine(resp)
            End If
        End Sub)

        Console.WriteLine(respLog.ToString)
        Dim respMessage = respText.ToString
        Assert.IsTrue(respMessage.Contains("晴天"))
        Assert.IsTrue(respMessage.Contains("30"))
    End Function

    <TestMethod>
    Async Function TestToolCallStreamingAsync() As Task
        Dim clientV4 As New ClientV4(ApiKey)
        Dim sb As New StringBuilder
        Dim messages As New List(Of MessageItem) From {
            New MessageItem("system", "不要假设或猜测传入函数的参数值。如果用户的描述不明确，请要求用户提供必要信息"),
            New MessageItem("user", "能帮我查天气吗？"),
            New MessageItem("assistant", "好的，请告诉我您所在的城市名称。"),
            New MessageItem("user", "北京"),
            New MessageItem("assistant", "您需要查询未来几天的天气呢？"),
            New MessageItem("user", "就今天一天的")
        }
        Dim lastToolCall As ToolCallItem = Nothing
        Dim onResponse =
            Sub(resp As ResponseBase)
                Dim toolCall = resp.Choices?.FirstOrDefault?.Delta?.ToolCalls?.FirstOrDefault
                If toolCall IsNot Nothing Then
                    lastToolCall = toolCall
                    Console.WriteLine("触发工具调用")
                    Return
                End If
                Dim respMessage = resp.Choices?.FirstOrDefault?.Delta?.Content
                If respMessage <> Nothing Then
                    sb.AppendLine($"{Environment.TickCount}: {respMessage}")
                End If
            End Sub
        Dim requestParams As New TextRequestBase With {
            .Model = "glm-4-flash",
            .Messages = messages,
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
            .Temperature = 0.3,
            .TopP = 0.7,
            .Stream = True
        }

        ' 这个模型有时候会需要多次工具调用才给你回答，这里我们重试最多十次。
        Await clientV4.Chat.StreamAsync(requestParams, onResponse)
        Dim retry = 0
        Do While lastToolCall IsNot Nothing AndAlso Interlocked.Increment(retry) <= 10
            Dim lastToolCallFunc = lastToolCall.Function
            If lastToolCallFunc IsNot Nothing Then
                ' 在这里返回了示例数据，实际应用中应当进行异步查询请求，并返回真实数据。
                If lastToolCallFunc.Name = "get_weather" AndAlso lastToolCallFunc.Arguments?.Contains("北京") Then
                    Dim callResult = "晴天，30 摄氏度。"
                    messages.Add(New MessageItem("tool", callResult) With {.ToolCallId = lastToolCall.Id})
                    lastToolCall = Nothing
                    Await clientV4.Chat.StreamAsync(requestParams, onResponse)
                End If
            End If
        Loop

        Dim finalResult = sb.ToString
        Console.WriteLine(finalResult)
        Assert.IsTrue(finalResult.Contains("晴"c))
        Assert.IsTrue(finalResult.Contains("30"))
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
