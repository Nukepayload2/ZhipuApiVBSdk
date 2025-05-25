Imports System.Text
Imports System.Threading
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
        Try
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
                Assert.Fail("抛异常了就不应该走这里了")
            End Sub)
            Assert.Fail("应该抛出异常才对")
        Catch ex As ZhipuHttpRequestException
            Dim errObj = ex.Details
            Assert.AreEqual("1211", errObj.Code)
            Assert.IsTrue(errObj.Message.Contains("模型不存在"))
        End Try
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
            .Model = "cogview-3-flash",
            .Prompt = "一只可爱的科幻风格小猫咪"
        })
        Dim respMessage = response.Data?.FirstOrDefault?.Url
        Assert.IsNotNull(respMessage)
    End Function

    <TestMethod>
    Public Async Function TestImageRecognitionAsync() As Task
        Dim clientV4 As New ClientV4(ApiKey)
        Dim response = Await clientV4.Chat.CompleteAsync(New TextRequestBase With {
            .Model = "glm-4v-flash",
            .Messages = {
                New ImageToTextMessageItem("user") With {
                    .Text = "这是什么",
                    .ImageUrl = "https://www.zhipuai.cn/assets/images/logo_icon.jpeg"
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
                .Model = "glm-4v-flash",
                .Messages = {
                    New ImageToTextMessageItem("user") With {
                        .Text = "这是什么",
                        .ImageUrl = "https://www.zhipuai.cn/assets/images/logo_icon.jpeg"
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
            .Model = "embedding-3",
            .Input = "一只可爱的科幻风格小猫咪",
            .Dimensions = 1024
        })

        Dim firstEmbedding = response.Data?.FirstOrDefault?.Embedding
        Assert.IsNotNull(firstEmbedding)
    End Function

    <TestMethod, Ignore("涉及到长任务，统一测试时不执行")>
    Async Function TestCreateChatBatchAsync() As Task
        ' 创建客户端
        Dim clientV4 As New ClientV4(ApiKey)
        ' 创建批量对话请求项
        Dim requests As New List(Of BatchChatRequestItem) From {
            New BatchChatRequestItem With {
                .CustomId = "request1",
                .Body = New TextRequestBase With {
                    .Model = "glm-4-flash",
                    .Messages = {New MessageItem("user", "你好，你是谁？")},
                    .Temperature = 0.7,
                    .TopP = 0.7
                }
            },
            New BatchChatRequestItem With {
                .CustomId = "request2",
                .Body = New TextRequestBase With {
                    .Model = "glm-4-flash",
                    .Messages = {New MessageItem("user", "你能做什么？")},
                    .Temperature = 0.7,
                    .TopP = 0.7
                }
            }
        }
        ' 创建批量对话任务
        Dim chatBatch As ChatBatch = Nothing
        Try
            chatBatch = Await clientV4.CreateChatBatchAsync(requests)
        Catch ex As Exception
            Assert.Fail($"创建批量对话任务失败: {ex.Message}")
        End Try
        ' 检查 batch 存在
        Assert.AreNotEqual(TaskStatus.Uninitialized, chatBatch.Status)
        Assert.IsTrue(chatBatch.Id.Length > 6)
    End Function

    <TestMethod, Ignore("涉及到长任务，统一测试时不执行")>
    Public Async Function TestListBatchAsync() As Task
        ' 创建客户端
        Dim clientV4 As New ClientV4(ApiKey)
        ' 列出批处理任务
        Dim listResult = Await clientV4.Batches.ListAsync(New BatchListRequest With {.Limit = 10})
        Assert.AreNotEqual(0, listResult.Data.Count)
    End Function

    <TestMethod, Ignore("涉及到长任务，统一测试时不执行")>
    Async Function TestWaitChatBatchAsync() As Task
        ' 创建客户端
        Dim clientV4 As New ClientV4(ApiKey)
        ' TODO: 请替换为实际 batch_id。上面的 TestListBatchAsync 可以查询批量任务列表，获取 batch_id
        Dim batchStatus = Await clientV4.Batches.GetStatusAsync("batch_1926150685276246016")
        ' 创建批量对话任务
        Dim chatBatch As New ChatBatch(clientV4, batchStatus)

        ' 等待时间老长了
        Do Until chatBatch.IsCompleted
            Await Task.Delay(3000)
            Await chatBatch.UpdateStatusAsync()
        Loop

        ' 必须是正常完成状态，否则数据没什么意义
        Assert.AreEqual(TaskStatus.Completed, chatBatch.Status)

        ' 获取批量任务结果
        Dim results = Await chatBatch.GetResultAsync()

        ' 验证结果
        Assert.IsNotNull(results)
        Assert.AreEqual(2, results.Count)
        For Each result In results
            Console.WriteLine($"CustomId: {result.CustomId}, Content: {result.Response.Body.Choices?.FirstOrDefault.Message.Content}")
        Next
    End Function

    <TestMethod>
    Public Async Function TestWebSearchAsync() As Task
        Dim clientV4 As New ClientV4(ApiKey)

        Dim request As New WebSearchRequest With {
            .SearchEngine = "search_std",
            .SearchQuery = $"红警HBK08在{Today}发布的视频"
        }

        Dim response = Await clientV4.WebSearch.SearchAsync(request)

        ' 处理响应数据
        Dim searchResults = response.SearchResult
        Assert.IsNotNull(searchResults)

    End Function

End Class
