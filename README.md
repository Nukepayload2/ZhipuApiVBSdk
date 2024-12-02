# ZhipuApiVBSdk
A VB friendly variant of https://github.com/MetaGLM/zhipuai-sdk-csharp-v4

- [x] No deadlock in STA thread apps.
- [x] Enumerate result asynchronously with the syntax introduced in VB 11.
- [x] No reflection required. Support AOT compilation on .NET 8 or later.
- [x] VB flavor of API usage

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