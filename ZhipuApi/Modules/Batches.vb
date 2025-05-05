Imports System.Net.Http
Imports System.Threading
Imports Nukepayload2.AI.Providers.Zhipu.Models
Imports Nukepayload2.AI.Providers.Zhipu.Utils

Public Class Batches
    Inherits ClientFeatureBase

    Public Sub New(apiKey As String)
        MyBase.New(apiKey)
    End Sub

    Sub New(apiKey As String, client As HttpClient)
        MyBase.New(apiKey, client)
    End Sub

    Protected Overridable ReadOnly Property RequestUrl As String = "https://open.bigmodel.cn/api/paas/v4/batches"

    ''' <summary>
    ''' 创建批处理任务
    ''' </summary>
    ''' <param name="request">要创建的批处理</param>
    ''' <param name="cancellationToken">用于取消请求</param>
    ''' <returns>批处理详情</returns>
    ''' <exception cref="ArgumentException">参数不正确</exception>
    ''' <exception cref="HttpRequestException">请求发生错误</exception>
    ''' <exception cref="ZhipuHttpRequestException">请求发生错误，并且错误详情可解析</exception>
    Public Async Function CreateAsync(request As BatchCreateRequest, Optional cancellationToken As CancellationToken = Nothing) As Task(Of BatchStatus)
        Dim content = request.ToJsonUtf8
        Dim response = Await PostAsync(RequestUrl, content, cancellationToken)
        Return BatchStatus.FromJson(response)
    End Function

    ''' <summary>
    ''' 获取指定ID的批处理信息
    ''' </summary>
    ''' <param name="batchId">要获取的批处理任务状态</param>
    ''' <param name="cancellationToken">用于取消请求</param>
    ''' <returns>批处理详情</returns>
    ''' <exception cref="ArgumentException">参数不正确</exception>
    ''' <exception cref="HttpRequestException">请求发生错误</exception>
    ''' <exception cref="ZhipuHttpRequestException">请求发生错误，并且错误详情可解析</exception>
    Public Async Function GetStatusAsync(batchId As String, Optional cancellationToken As CancellationToken = Nothing) As Task(Of BatchStatus)
        Dim url = $"{RequestUrl}/{batchId}"
        Dim response = Await GetAsync(url, cancellationToken)
        Return BatchStatus.FromJson(response)
    End Function

    ''' <summary>
    ''' 获取批处理列表（分页）
    ''' </summary>
    ''' <param name="listRequest">列出文件的请求参数</param>
    ''' <param name="cancellationToken">用于取消请求</param>
    ''' <returns>多个批处理详情</returns>
    ''' <exception cref="ArgumentException">参数不正确</exception>
    ''' <exception cref="HttpRequestException">请求发生错误</exception>
    ''' <exception cref="ZhipuHttpRequestException">请求发生错误，并且错误详情可解析</exception>
    Public Async Function ListAsync(listRequest As BatchListRequest,
                                    Optional cancellationToken As CancellationToken = Nothing) As Task(Of BatchPage)
        Dim queryBuilder As New QueryBuilder(RequestUrl)

        queryBuilder.Add("after", listRequest.After)
        queryBuilder.Add("limit", listRequest.Limit)

        Dim url = queryBuilder.ToString
        Dim response = Await GetAsync(url, cancellationToken)
        Return BatchPage.FromJson(response)
    End Function

    ''' <summary>
    ''' 取消指定ID的批处理
    ''' </summary>
    ''' <param name="batchId">取消的任务 ID</param>
    ''' <param name="cancellationToken">用于取消请求</param>
    ''' <returns>取消的批处理状态</returns>
    ''' <exception cref="ArgumentException">参数不正确</exception>
    ''' <exception cref="HttpRequestException">请求发生错误</exception>
    ''' <exception cref="ZhipuHttpRequestException">请求发生错误，并且错误详情可解析</exception>
    Public Async Function CancelAsync(batchId As String, Optional cancellationToken As CancellationToken = Nothing) As Task(Of BatchStatus)
        Dim url = $"{RequestUrl}/{batchId}/cancel"
        Dim response = Await PostAsync(url, Nothing, cancellationToken)
        Return BatchStatus.FromJson(response)
    End Function
End Class
