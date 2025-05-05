# ZhipuApiVBSdk
VB-friendly .NET bindings for the ZhipuApi v4. It's part of the Nukepayload2 VB AI SDK as a model provider.

It has the same capabilities as [zhipuai-sdk-csharp-v4](https://github.com/MetaGLM/zhipuai-sdk-csharp-v4), with additional VB-friendly benefits:

- [x] No deadlock in synchronization context threads.
- [x] Enumerate result asynchronously with the syntax introduced in VB 11.
- [x] No reflection required. We support AOT compilation on .NET 8 or later.
- [x] VB flavor of API design

Code examples: 
- [CodeExamples.vb](https://github.com/Nukepayload2/ZhipuApiVBSdk/blob/master/ZhipuApiExamples/CodeExamples.vb)
- [MsAICodeExamples.vb](https://github.com/Nukepayload2/ZhipuApiVBSdk/blob/master/ZhipuApiExamples/MsAICodeExamples.vb)

Get on NuGet: [Nukepayload2.AI.Providers.Zhipu](https://www.nuget.org/packages/Nukepayload2.AI.Providers.Zhipu)

## Target Frameworks
- .NET Standard 2.0
- .NET 8.0

## Progress
It's currently in beta stage. 
We reserve the rights of **making breaking changes**.

Do not use it in production environment unless you've tested it carefully.

### Implementation
- [x] [Text completion](https://bigmodel.cn/dev/api/normal-model/glm-4)
- [x] [Text streaming](https://bigmodel.cn/dev/api/normal-model/glm-4)
- [x] [Tool call in completion](https://bigmodel.cn/dev/api/normal-model/glm-4)
- [x] [Tool call in streaming](https://bigmodel.cn/dev/api/normal-model/glm-4)
- [x] [Image generation](https://bigmodel.cn/dev/api/image-model/cogview)
- [x] [Image recognition](https://bigmodel.cn/dev/api/normal-model/glm-4v)
- [x] [Embedding](https://bigmodel.cn/dev/api/vector/embedding)
- [x] [Errors as HttpRequestException](https://bigmodel.cn/dev/api/error-code/service-error)

### Tested Manually
- [x] Text completion
- [x] Text streaming
- [x] Tool call in completion
- [x] Tool call in streaming
- [x] Errors as HttpRequestException
- [x] Image generation
- [ ] Image recognition
- [x] Embedding

### Microsoft.Extension.AI 9.0.3 Preview
- [x] Chat completion
- [x] Chat streaming
- [x] Tool call in completion
- [x] Tool call in streaming
- [ ] Image generation
- [ ] Image recognition
- [ ] Embedding

### Tested Manually with Microsoft.Extension.AI
- [x] Text completion
- [x] Text streaming
- [x] Tool call in completion
- [x] Tool call in streaming
- [ ] Image generation
- [ ] Image recognition
- [ ] Embedding

### Planned
#### P1
- [ ] [File management](https://bigmodel.cn/dev/api/knowlage-manage/queryfile) <!-- 需要 JSON schema 生成 (files.json) -->
    - [ ] [Text extraction](https://bigmodel.cn/dev/api/knowlage-manage/queryextract) <!-- 增强 files 支持，purpose="file-extract" -->
- [ ] [Batch](https://bigmodel.cn/dev/api/batch-api/batch) <!-- 需要 JSON schema 生成 (batch.json) -->
- [ ] [Web search](https://bigmodel.cn/dev/api/search-tool/web-search) <!-- 需要 JSON schema 生成，不做 MCP (websearch.json) -->
- [ ] [Video generation](https://bigmodel.cn/dev/api/videomodel/cogvideox) <!-- 需要 JSON schema 生成 (video-gen.json)-->
- [ ] [Voice recognition](https://bigmodel.cn/dev/api/rtav/glm-asr) <!-- 需要 JSON schema 生成 (asr.json) -->

#### P2
- [ ] [Input audio in chat](https://bigmodel.cn/dev/api/rtav/GLM-4-Voice) <!-- 增强 chat/completions 支持，content "type": "input_audio" -->
- [ ] [Web search in chat](https://bigmodel.cn/dev/api/search-tool/websearch-in-chat) <!-- 增强 chat/completions 支持，请求 type 是 web_search，响应有 web_search -->
- [ ] [Tokenizer](https://bigmodel.cn/dev/api/tokenizer) <!-- 需要 JSON schema 生成 (tokenizer.json) -->
- [ ] [Rerank](https://bigmodel.cn/dev/api/knowlage-manage/rerank) <!-- 需要 JSON schema 生成 (rerank.json) -->
- [ ] [Async completion](https://bigmodel.cn/dev/api/normal-model/glm-4) <!-- 需要 JSON schema 生成 (async-response.json) -->

#### P3
- [ ] [Document management](https://bigmodel.cn/dev/api/knowlage-manage/queryfile) <!-- 需要 JSON schema 生成 (files.json) -->
- [ ] Knowledge management (undocumented)
- [ ] [Realtime video chat](https://bigmodel.cn/dev/api/rtav/GLM-Realtime) <!-- 要使用 wss，并且需要 JSON schema 生成 (realtime.json)，由于它专注特定领域，低优先级。 -->
- [ ] [Agent: AllTools](https://bigmodel.cn/dev/api/intelligent-agent-model/glm-4-alltools) <!-- 增强 chat/completions 支持，tools 和 tool_calls 里面加上 code_interpreter, drawing_tool, web_browser  -->
- [ ] [Agent: Application](https://bigmodel.cn/dev/api/Agent_Platform/newagent) <!-- 需要 JSON schema 生成，由于 MCP 能覆盖此用例，低优先级。 -->
- [ ] [Agent: Assistant](https://bigmodel.cn/dev/api/intelligent-agent-model/assistantapi) <!-- 需要 JSON schema 生成 (assistant.other.json) -->
- [ ] [Agent: Code completion](https://bigmodel.cn/dev/api/code-model/codegeex-4) <!-- 增强 chat/completions 支持，extra 里面是代码补全相关的参数 -->
- [ ] [Agent: Emohaa](https://bigmodel.cn/dev/api/super-humanoid/emohaa) <!-- 增强 chat/completions 支持，里面有个 meta -->
- [ ] [Agent: Financial](https://bigmodel.cn/dev/api/Agent_Platform/FinAgent) <!-- 需要 JSON schema 生成，由于它专注特定领域，低优先级。 -->
- [ ] [Agent: Search](https://bigmodel.cn/dev/api/search-tool/agent-search) <!-- 需要 JSON schema 生成 (assistant.search.json) -->
- [ ] [Fine tuning](https://bigmodel.cn/dev/api/model-fine-tuning/fine-tuning) <!-- 需要 JSON schema 生成，由于它专注特定领域，低优先级。 -->
