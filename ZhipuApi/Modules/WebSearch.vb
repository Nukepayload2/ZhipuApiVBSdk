Imports System.Net.Http
Imports System.Threading
Imports Nukepayload2.AI.Providers.Zhipu.Models

''' <summary>
''' 专给大模型用的搜索引擎，在传统搜索引擎网页抓取、排序的能力基础上，增强了意图识别能力，返回更适合大模型处理的结果（网页标题、网页URL、网页摘要、网站名称、网站图标等）。
''' </summary>
Public Class WebSearch
    Inherits ClientFeatureBase
    Public Sub New(apiKey As String)
        MyBase.New(apiKey)
    End Sub

    Sub New(apiKey As String, client As HttpClient)
        MyBase.New(apiKey, client)
    End Sub

    Protected Overridable ReadOnly Property RequestUrl As String = "https://open.bigmodel.cn/api/paas/v4/web_search"

    ''' <summary>
    ''' 执行搜索请求
    ''' </summary>
    ''' <param name="request">搜索请求参数</param>
    ''' <param name="cancellationToken">用于取消请求</param>
    ''' <returns>搜索结果</returns>
    ''' <exception cref="ArgumentException">参数不正确</exception>
    ''' <exception cref="HttpRequestException">请求发生错误</exception>
    ''' <exception cref="ZhipuHttpRequestException">请求发生错误，并且错误详情可解析</exception>
    Public Async Function SearchAsync(request As WebSearchRequest, Optional cancellationToken As CancellationToken = Nothing) As Task(Of WebSearchResponse)
        Dim content = request.ToJsonUtf8
        Dim response = Await PostAsync(RequestUrl, content, cancellationToken)
        Return WebSearchResponse.FromJson(response)
    End Function
End Class
