Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Threading
Imports Nukepayload2.AI.Providers.Zhipu.Models

Public Class Files
    Inherits ClientFeatureBase

    Public Sub New(apiKey As String)
        MyBase.New(apiKey)
    End Sub

    Sub New(apiKey As String, client As HttpClient)
        MyBase.New(apiKey, client)
    End Sub

    Protected Overridable ReadOnly Property RequestUrl As String = "https://open.bigmodel.cn/api/paas/v4/files"

    ''' <summary>
    ''' 上传文件
    ''' </summary>
    ''' <param name="request">指定上传的文件</param>
    ''' <param name="cancellationToken">用于取消请求</param>
    ''' <returns>
    ''' 上传的文件信息
    ''' </returns>
    ''' <exception cref="ArgumentException">参数不正确</exception>
    ''' <exception cref="HttpRequestException">请求发生错误</exception>
    ''' <exception cref="ZhipuHttpRequestException">请求发生错误，并且错误详情可解析</exception>
    Public Async Function UploadAsync(request As FileUploadRequest, Optional cancellationToken As CancellationToken = Nothing) As Task(Of FileItem)
        If request Is Nothing Then
            Throw New ArgumentNullException(NameOf(request))
        End If
        If request.FileContent Is Nothing Then
            Throw New ArgumentException("FileContent is required.")
        End If
        If request.FileName = Nothing Then
            Throw New ArgumentException("FileName is required.")
        End If
        Dim formData As New MultipartFormDataContent
        Dim fileContent As New StreamContent(request.FileContent)
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream")
        formData.Add(fileContent, "file", request.FileName)
        If request.Purpose <> Nothing Then
            formData.Add(New StringContent(request.Purpose), "purpose")
        End If
        If request.KnowledgeId <> Nothing Then
            formData.Add(New StringContent(request.KnowledgeId), "knowledge_id")
        End If
        Dim response = Await PostRawAsync(RequestUrl, formData, cancellationToken)
        Dim jsonStream = Await ReadAndCheckErrorAsync(response, cancellationToken)
        Dim result = FileItem.FromJson(jsonStream)
        Return result
    End Function

    ''' <summary>
    ''' 删除指定的文件
    ''' </summary>
    ''' <param name="request">指定哪个文件要删除</param>
    ''' <param name="cancellationToken">用于取消请求</param>
    ''' <returns>删除操作的结果</returns>
    ''' <exception cref="ArgumentException">参数不正确</exception>
    ''' <exception cref="HttpRequestException">请求发生错误</exception>
    ''' <exception cref="ZhipuHttpRequestException">请求发生错误，并且错误详情可解析</exception>
    Public Overloads Async Function DeleteAsync(request As FileDeleteRequest, Optional cancellationToken As CancellationToken = Nothing) As Task(Of FileDeleteResponse)
        If request?.FileId = Nothing Then
            Throw New ArgumentException("FileId is required.")
        End If
        Dim requestUrl = Me.RequestUrl & "/" & request.FileId
        Dim jsonStream = Await DeleteAsync(requestUrl, cancellationToken)
        Dim result = FileDeleteResponse.FromJson(jsonStream)
        Return result
    End Function

    ''' <summary>
    ''' 获取文件列表
    ''' </summary>
    ''' <param name="request">指定文件列表的参数</param>
    ''' <param name="cancellationToken">用于取消请求</param>
    ''' <returns>获取的文件列表</returns>
    ''' <exception cref="ArgumentException">参数不正确</exception>
    ''' <exception cref="HttpRequestException">请求发生错误</exception>
    ''' <exception cref="ZhipuHttpRequestException">请求发生错误，并且错误详情可解析</exception>
    Public Async Function ListAsync(request As FilesListRequest, Optional cancellationToken As CancellationToken = Nothing) As Task(Of FilesListResponse)
        If request Is Nothing Then
            Throw New ArgumentNullException(NameOf(request))
        End If

        ' 参数集合用于构建查询字符串
        Dim queryParams As New List(Of String)

        ' 1. 验证 Purpose 为必填
        If String.IsNullOrEmpty(request.Purpose) Then
            Throw New ArgumentException("Purpose is required.")
        End If

        queryParams.Add($"purpose={Uri.EscapeDataString(request.Purpose)}")

        ' 2. 知识库ID - 可选，非空才添加
        If request.KnowledgeId <> Nothing Then
            queryParams.Add($"knowledge_id={Uri.EscapeDataString(request.KnowledgeId)}")
        End If

        ' 3. Page（Long?），默认值为 1
        If request.Page IsNot Nothing Then
            queryParams.Add($"page={request.Page.Value}")
        End If

        ' 4. Limit（Long?），默认值为 10
        If request.Limit IsNot Nothing Then
            queryParams.Add($"limit={request.Limit.Value}")
        End If

        ' 5. After
        If request.After <> Nothing Then
            queryParams.Add($"after={Uri.EscapeDataString(request.After)}")
        End If

        ' 6. Order，默认为 "desc"
        If request.Order <> Nothing Then
            queryParams.Add($"order={Uri.EscapeDataString(request.Order)}")
        End If

        ' 构建完整的 URL（基础路径 + 查询参数）
        Dim queryString As String = If(queryParams.Count > 0, "?" & String.Join("&", queryParams), "")

        Dim finalUrl As New Uri(RequestUrl + queryString)
        Dim response = Await GetAsync(RequestUrl, cancellationToken)
        Dim result = FilesListResponse.FromJson(response)
        Return result
    End Function
End Class
