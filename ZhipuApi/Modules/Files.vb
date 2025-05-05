Imports System.IO
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Text
Imports System.Threading
Imports Nukepayload2.AI.Providers.Zhipu.Models
Imports Nukepayload2.AI.Providers.Zhipu.Utils

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

        ' 1. 验证 Purpose 是否必填且非空字符串
        If String.IsNullOrEmpty(request.Purpose) Then
            Throw New ArgumentException("Purpose is required.")
        End If

        ' 创建 QueryBuilder 实例，传入基础 URL（假设 RequestUrl 是类属性）
        Dim queryBuilder As New QueryBuilder(RequestUrl)

        queryBuilder.Add("purpose", request.Purpose)

        ' 2. knowledge_id（可选，由 QueryBuilder 自动处理是否添加）
        queryBuilder.Add("knowledge_id", request.KnowledgeId)

        ' 3. page - Long?
        queryBuilder.Add("page", request.Page)

        ' 4. limit - Long?
        queryBuilder.Add("limit", request.Limit)

        ' 5. after（假设是可空字符串）
        queryBuilder.Add("after", request.After)

        ' 6. order（默认为 "desc"，非必填）
        queryBuilder.Add("order", request.Order)

        ' 构建完整 URL 并调用 API
        Dim url = queryBuilder.ToString()
        Dim response = Await GetAsync(url, cancellationToken)
        Dim result = FilesListResponse.FromJson(response)
        Return result
    End Function

    ''' <summary>
    ''' 完成批处理任务后，您可以通过使用 <see cref="BatchStatus.OutputFileId"/> 将输出文件下载到本地。
    ''' </summary>
    ''' <param name="fileId">要下载的文件</param>
    ''' <param name="cancellationToken">用于取消请求</param>
    ''' <returns>下载的文件流</returns>
    ''' <exception cref="ArgumentException">参数不正确</exception>
    ''' <exception cref="HttpRequestException">请求发生错误</exception>
    ''' <exception cref="ZhipuHttpRequestException">请求发生错误，并且错误详情可解析</exception>
    Public Async Function DownloadAsync(fileId As String, Optional cancellationToken As CancellationToken = Nothing) As Task(Of MemoryStream)
        Dim url = $"{RequestUrl}/{fileId}/content"
        Dim response = Await GetAsync(url, cancellationToken, "application/octet-stream")
        Return response
    End Function
End Class
