## Breaking changes of 1.1 Beta
### MicrosoftChatClientAdapter has changed the usage

#### Reason for this change
Migrated to Microsoft.Extensions.AI.Abstractions 9.0.3 Preview, which is the version that LlamaSharp is using.

#### Breaking change type
- Binary breaking change: some interface implementations could not be found at runtime.
- Source breaking change: some members are changed at compile time.

#### Version introduced
1.1.0-beta12

## Breaking changes of 1.0 Beta
### All value type properties are nullable
#### Reason for this change
After this change, we can distinguish whether a `Double`/`Integer`/`Boolean` is absent in JSON.

#### Breaking change type
- Binary breaking change: some properties could not be found at runtime.
- Source breaking change: some property types are changed at compile time.

#### Suggested action
Use nullable value types to fix compilation errors.

#### Version introduced
1.0.1-beta10

### Dictionaries are only used when there're unknown property names
#### Reason for this change
After this change, the API design is more complaint with OOP.

#### Breaking change type
- Binary breaking change: some properties and types could not be found at runtime.
- Source breaking change: some property types are changed at compile time.

#### Suggested action
Use specific classes instead of dictionaries to fix compilation errors.
- `FunctionTool.Function` property

#### Version introduced
1.0.1-beta10

### More places use `IReadOnlyList(Of T)` instead of arrays
See "`TextRequestBase` uses `IReadOnlyList(Of T)` instead of arrays" for more information.
#### Version introduced
1.0.1-beta10

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
