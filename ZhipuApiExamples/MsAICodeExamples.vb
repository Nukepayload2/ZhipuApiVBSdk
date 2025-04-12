Imports System.Text
Imports System.Text.Json
Imports Microsoft.Extensions.AI
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Nukepayload2.AI.Providers.Zhipu

<TestClass>
Public Class MsAICodeExamples
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
    Async Function TestMicrosoftAICompletionAsync() As Task
        Dim clientV4 As New ClientV4(ApiKey)
        Dim response = Await clientV4.Chat.AsChatClient("glm-4-flash").GetResponseAsync(
            {New ChatMessage(ChatRole.User, "你好，你是谁？")},
             New ChatOptions With {.Temperature = 0.7F, .TopP = 0.7F}
        )

        Dim respMessage = response.Messages?.FirstOrDefault?.Text
        Console.WriteLine(respMessage)
        Assert.IsNotNull(respMessage)
    End Function

    <TestMethod>
    Async Function TestMicrosoftAICompletionNoOptionsAsync() As Task
        Dim clientV4 As New ClientV4(ApiKey)
        Dim response = Await clientV4.Chat.AsChatClient("glm-4-flash").GetResponseAsync(
            {New ChatMessage(ChatRole.User, "你好，你是谁？")}
        )

        Dim respMessage = response.Messages?.FirstOrDefault?.Text
        Console.WriteLine(respMessage)
        Assert.IsNotNull(respMessage)
    End Function

    <TestMethod>
    Async Function TestMicrosoftAIStreamAsync() As Task
        Dim clientV4 As New ClientV4(ApiKey)
        Dim messages = {New ChatMessage(ChatRole.User, "1+1等于多少"),
                        New ChatMessage(ChatRole.Assistant, "1+1等于2。"),
                        New ChatMessage(ChatRole.User, "再加2呢？")}
        Dim options As New ChatOptions With {.Temperature = 0.7F, .TopP = 0.7F}
        Dim respAsyncEnumerate = clientV4.Chat.AsChatClient("glm-4-flash").GetStreamingResponseAsync(messages, options)
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
    Async Function TestMicrosoftAIStreamErrorHandlingAsync() As Task
        Dim clientV4 As New ClientV4(ApiKey)
        Dim messages = {New ChatMessage(ChatRole.User, "1+1等于多少"),
                        New ChatMessage(ChatRole.Assistant, "1+1等于2。"),
                        New ChatMessage(ChatRole.User, "再加2呢？")}
        Dim options As New ChatOptions With {.Temperature = 0.7F, .TopP = 0.7F}
        Dim sb As New StringBuilder

        Try
            Dim respAsyncEnumerate = clientV4.Chat.AsChatClient("glm-wrong-model-name").GetStreamingResponseAsync(messages, options)
            ' 在 VB 支持简化的 IAsyncEnumerable 调用 (Await Each 语句) 之前可以用 System.Linq.Async 读取服务端回复的数据。
            Await respAsyncEnumerate.ForEachAsync(
            Sub(update)
                Dim respMessage = update.Text
                If respMessage <> Nothing Then
                    sb.AppendLine($"{Environment.TickCount}: {respMessage}")
                End If
            End Sub)
            Assert.Fail("Exception expected")
        Catch ex As Exception
            Assert.IsTrue(ex.Message.Contains("模型不存在"))
        End Try
    End Function

    <TestMethod>
    Async Function TestMicrosoftAIStreamNoOptionsAsync() As Task
        Dim clientV4 As New ClientV4(ApiKey)
        Dim messages = {New ChatMessage(ChatRole.User, "1+1等于多少"),
                        New ChatMessage(ChatRole.Assistant, "1+1等于2。"),
                        New ChatMessage(ChatRole.User, "再加2呢？")}
        Dim respAsyncEnumerate = clientV4.Chat.AsChatClient("glm-4-flash").GetStreamingResponseAsync(messages)
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

    Private Class AIGetWeather
        Inherits AIFunction

        Private Shared ReadOnly Schema As String =
            "{
  ""title"": ""get_weather"",
  ""description"": ""根据提供的城市名称，提供未来的天气数据"",
  ""type"": ""object"",
  ""properties"": {
    ""city"": {
      ""type"": ""string"",
      ""description"": ""搜索的城市名称""
    },
    ""days"": {
      ""type"": ""integer"",
      ""description"": ""要查询的未来的天数，默认为0"",
      ""default"": 0
    }
  },
  ""required"": [""city""]
}"

        Public Overrides ReadOnly Property JsonSchema As JsonElement = JsonDocument.Parse(Schema).RootElement

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
        Dim response = Await clientV4.Chat.AsChatClient("glm-4-flash").GetResponseAsync(
            New List(Of ChatMessage) From {
                New ChatMessage(ChatRole.System, "不要假设或猜测传入函数的参数值。如果用户的描述不明确，请要求用户提供必要信息"),
                New ChatMessage(ChatRole.User, "能帮我查天气吗？"),
                New ChatMessage(ChatRole.Assistant, "好的，请告诉我您所在的城市名称。"),
                New ChatMessage(ChatRole.User, "北京"),
                New ChatMessage(ChatRole.Assistant, "您需要查询未来几天的天气呢？"),
                New ChatMessage(ChatRole.User, "就今天一天的")
            }, options
        )

        Dim respMessage = response.Messages?.FirstOrDefault?.Text
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
        Dim respAsyncEnumerate = clientV4.Chat.AsChatClient("glm-4-flash").GetStreamingResponseAsync(
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

End Class
