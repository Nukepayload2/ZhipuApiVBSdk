# ZhipuApiVBSdk
VB-friendly .NET bindings for the ZhipuApi v4. It's part of the Nukepayload2 VB AI SDK as a model provider.

It covers the capabilities of [zhipuai-sdk-csharp-v4](https://github.com/MetaGLM/zhipuai-sdk-csharp-v4), with additional VB-friendly benefits:

- [x] No deadlock in synchronization context threads.
- [x] Enumerate result asynchronously with the syntax introduced in VB 11.
- [x] AOT compatible on .NET 8 or later.
- [x] VB flavor of API design
- [x] Microsoft.Extensions.AI integration

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
- [x] [Errors as HttpRequestException](https://bigmodel.cn/dev/api/error-code/service-error)
- [x] [Image generation](https://bigmodel.cn/dev/api/image-model/cogview)
- [x] [Image recognition](https://bigmodel.cn/dev/api/normal-model/glm-4v)
- [x] [Embedding](https://bigmodel.cn/dev/api/vector/embedding)
- [x] [File management](https://bigmodel.cn/dev/api/knowlage-manage/queryfile)
- [x] [Batch](https://bigmodel.cn/dev/api/batch-api/batch)

### Tested Manually
- [x] Text completion
- [x] Text streaming
- [x] Tool call in completion
- [x] Tool call in streaming
- [x] Errors as HttpRequestException
- [x] Image generation
- [x] Image recognition
- [x] Embedding
- [x] File management
- [x] Batch

### Microsoft.Extensions.AI 9.0.5
- [x] Chat completion
- [x] Chat streaming
- [x] Tool call in completion
- [x] Tool call in streaming
- [ ] Image generation
- [ ] Image recognition
- [ ] Embedding

### Tested Manually with Microsoft.Extensions.AI
- [x] Text completion
- [x] Text streaming
- [x] Tool call in completion
- [x] Tool call in streaming
- [ ] Image generation
- [ ] Image recognition
- [ ] Embedding

### Planned
#### P1
- [ ] [Web search](https://bigmodel.cn/dev/api/search-tool/web-search) <!-- ��Ҫ JSON schema ���ɣ����� MCP (websearch.json) -->
- [ ] [Video generation](https://bigmodel.cn/dev/api/videomodel/cogvideox) <!-- ��Ҫ JSON schema ���� (video-gen.json)-->
- [ ] [Voice recognition](https://bigmodel.cn/dev/api/rtav/glm-asr) <!-- ��Ҫ JSON schema ���� (asr.json) -->

#### P2
- [ ] [Input audio in chat](https://bigmodel.cn/dev/api/rtav/GLM-4-Voice) <!-- ��ǿ chat/completions ֧�֣�content "type": "input_audio" -->
- [ ] [Web search in chat](https://bigmodel.cn/dev/api/search-tool/websearch-in-chat) <!-- ��ǿ chat/completions ֧�֣����� type �� web_search����Ӧ�� web_search -->
- [ ] [Tokenizer](https://bigmodel.cn/dev/api/tokenizer) <!-- ��Ҫ JSON schema ���� (tokenizer.json) -->
- [ ] [Rerank](https://bigmodel.cn/dev/api/knowlage-manage/rerank) <!-- ��Ҫ JSON schema ���� (rerank.json) -->
- [ ] [Async completion](https://bigmodel.cn/dev/api/normal-model/glm-4) <!-- ��Ҫ JSON schema ���� (async-response.json) -->
- [ ] [Text extraction](https://bigmodel.cn/dev/api/knowlage-manage/queryextract) <!-- ��ǿ files ֧�֣�purpose="file-extract" -->

#### P3
- [ ] [Document management](https://bigmodel.cn/dev/api/knowlage-manage/queryfile) <!-- ��Ҫ JSON schema ���� (files.json) -->
- [ ] Knowledge management (undocumented)
- [ ] [Realtime video chat](https://bigmodel.cn/dev/api/rtav/GLM-Realtime) <!-- Ҫʹ�� wss��������Ҫ JSON schema ���� (realtime.json)��������רע�ض����򣬵����ȼ��� -->
- [ ] [Agent: AllTools](https://bigmodel.cn/dev/api/intelligent-agent-model/glm-4-alltools) <!-- ��ǿ chat/completions ֧�֣�tools �� tool_calls ������� code_interpreter, drawing_tool, web_browser  -->
- [ ] [Agent: Application](https://bigmodel.cn/dev/api/Agent_Platform/newagent) <!-- ��Ҫ JSON schema ���ɣ����� MCP �ܸ��Ǵ������������ȼ��� -->
- [ ] [Agent: Assistant](https://bigmodel.cn/dev/api/intelligent-agent-model/assistantapi) <!-- ��Ҫ JSON schema ���� (assistant.other.json) -->
- [ ] [Agent: Code completion](https://bigmodel.cn/dev/api/code-model/codegeex-4) <!-- ��ǿ chat/completions ֧�֣�extra �����Ǵ��벹ȫ��صĲ��� -->
- [ ] [Agent: Emohaa](https://bigmodel.cn/dev/api/super-humanoid/emohaa) <!-- ��ǿ chat/completions ֧�֣������и� meta -->
- [ ] [Agent: Financial](https://bigmodel.cn/dev/api/Agent_Platform/FinAgent) <!-- ��Ҫ JSON schema ���ɣ�������רע�ض����򣬵����ȼ��� -->
- [ ] [Agent: Search](https://bigmodel.cn/dev/api/search-tool/agent-search) <!-- ��Ҫ JSON schema ���� (assistant.search.json) -->
- [ ] [Fine tuning](https://bigmodel.cn/dev/api/model-fine-tuning/fine-tuning) <!-- ��Ҫ JSON schema ���ɣ�������רע�ض����򣬵����ȼ��� -->
