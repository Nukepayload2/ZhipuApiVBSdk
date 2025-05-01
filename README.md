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
- [ ] [File management](https://bigmodel.cn/dev/api/knowlage-manage/queryfile)
- [ ] [Batch](https://bigmodel.cn/dev/api/batch-api/batch)
- [ ] [Async completion](https://bigmodel.cn/dev/api/batch-api/batch)
- [ ] [Text extraction](https://bigmodel.cn/dev/api/knowlage-manage/queryextract)
- [ ] [Rerank](https://bigmodel.cn/dev/api/knowlage-manage/rerank)
- [ ] [Input audio in chat](https://bigmodel.cn/dev/api/rtav/GLM-4-Voice)
- [ ] [Realtime video chat](https://bigmodel.cn/dev/api/rtav/GLM-Realtime)
- [ ] [Voice recognition](https://bigmodel.cn/dev/api/rtav/glm-asr)
- [ ] [Web search](https://bigmodel.cn/dev/api/search-tool/web-search)
- [ ] [Web search in chat](https://bigmodel.cn/dev/api/search-tool/websearch-in-chat)
- [ ] [Search agent](https://bigmodel.cn/dev/api/search-tool/agent-search)
- [ ] [Video generation](https://bigmodel.cn/dev/api/videomodel/cogvideox)
- [ ] [Agent: AllTools](https://bigmodel.cn/dev/api/intelligent-agent-model/glm-4-alltools)
- [ ] [Agent: Assistant](https://bigmodel.cn/dev/api/intelligent-agent-model/assistantapi)
- [ ] [Agent: Code completion](https://bigmodel.cn/dev/api/code-model/codegeex-4)
- [ ] [Agent: Emohaa](https://bigmodel.cn/dev/api/super-humanoid/emohaa)
- [ ] [Agent: Financial](https://bigmodel.cn/dev/api/Agent_Platform/FinAgent)
- [ ] [Agent v2](https://bigmodel.cn/dev/api/Agent_Platform/newagent)
- [ ] [Fine tuning](https://bigmodel.cn/dev/api/model-fine-tuning/fine-tuning)
- [ ] [Tokenizer](https://bigmodel.cn/dev/api/tokenizer)

### Tested Manually
- [x] Text completion
- [x] Text streaming
- [x] Tool call in completion
- [x] Tool call in streaming
- [ ] Image generation
- [ ] Image recognition
- [ ] Embedding

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
