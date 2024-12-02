# ZhipuApiVBSdk
VB-friendly .NET bindings for the ZhipuApi v4. It's part of the Nukepayload2 VB AI SDK as a model provider.

It has the same capabilities as [zhipuai-sdk-csharp-v4](https://github.com/MetaGLM/zhipuai-sdk-csharp-v4), with additional VB-friendly benefits:

- [x] No deadlock in synchronization context threads.
- [x] Enumerate result asynchronously with the syntax introduced in VB 11.
- [x] No reflection required. Support AOT compilation on .NET 8 or later.
- [x] VB flavor of API design

Code examples: [CodeExamples.vb](https://github.com/Nukepayload2/ZhipuApiVBSdk/blob/master/ZhipuApiExamples/CodeExamples.vb)

NuGet package name: `Nukepayload2.AI.Providers.Zhipu`

## Target Frameworks
- .NET Standard 2.0
- .NET 8 or later

## Progress
It's currently in beta stage. Do not use it in production environment unless you've tested it carefully.

- [x] Text completion (tested)
- [x] Text streaming (tested)
- [x] Tool call (tested)
- [ ] Image generation (implemented but untested)
- [ ] Image recognition (implemented but untested)
- [ ] Embedding (implemented but untested)