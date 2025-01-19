# ZhipuApiVBSdk
VB-friendly .NET bindings for the ZhipuApi v4. It's part of the Nukepayload2 VB AI SDK as a model provider.

It has the same capabilities as [zhipuai-sdk-csharp-v4](https://github.com/MetaGLM/zhipuai-sdk-csharp-v4), with additional VB-friendly benefits:

- [x] No deadlock in synchronization context threads.
- [x] Enumerate result asynchronously with the syntax introduced in VB 11.
- [x] No reflection required. We support AOT compilation on .NET 8 or later.
- [x] VB flavor of API design

Code examples: [CodeExamples.vb](https://github.com/Nukepayload2/ZhipuApiVBSdk/blob/master/ZhipuApiExamples/CodeExamples.vb)

Get on NuGet: [Nukepayload2.AI.Providers.Zhipu](https://www.nuget.org/packages/Nukepayload2.AI.Providers.Zhipu)

## Target Frameworks
- .NET Standard 2.0
- .NET 8 or later

## Progress
It's currently in beta stage. 
We reserve the rights of **making breaking changes**.

Do not use it in production environment unless you've tested it carefully.

### Implementation
- [x] Text completion
- [x] Text streaming
- [x] Tool call in completion
- [x] Tool call in streaming
- [x] Image generation
- [x] Image recognition
- [x] Embedding

### Tested Manually
- [x] Text completion
- [x] Text streaming
- [x] Tool call in completion
- [x] Tool call in streaming
- [ ] Image generation
- [ ] Image recognition
- [ ] Embedding

#### Microsoft.Extension.AI 9.0.0 Preview
- [x] Chat completion
- [x] Chat streaming
- [x] Tool call in completion
- [x] Tool call in streaming
- [ ] Image generation
- [ ] Image recognition
- [ ] Embedding

### Tested Manually
- [x] Text completion
- [x] Text streaming
- [x] Tool call in completion
- [x] Tool call in streaming
- [ ] Image generation
- [ ] Image recognition
- [ ] Embedding

## Breaking changes of 1.0 Beta
### `TextRequestBase` uses `IReadOnlyList(Of T)` instead of arrays
#### Reason for this change
Enable the use of `List(Of T)` and `T()` at the same time. `List(Of T)` is very useful when handling tool calls. But in other cases (e.g. LINQ result or hard-coded parameters), arrays are enough.

#### Breaking change type
- Binary breaking change: some property setters could not be found at runtime.

#### Suggested action
Recompile your program and components with the same version of this SDK to resolve runtime type conflicts.

#### Version introduced
1.0.0-beta9

### Removed useless `SetXxx` methods
#### Reason for this change
These methods are designed for Java. VB has `With` statement for object initialization.

#### Breaking change type
- Binary breaking change: some `SetXxx` methods could not be found at runtime.
- Source breaking change: some `SetXxx` methods could not be found at compile time.

#### Suggested action
Use [`With` statement](https://learn.microsoft.com/en-us/dotnet/visual-basic/programming-guide/language-features/objects-and-classes/how-to-declare-an-object-by-using-an-object-initializer) for object initialization.

#### Version introduced
1.0.0-beta2 and other 1.0 beta versions
