Imports System.Collections.Concurrent
Imports System.Threading

' Provides more detailed control of IAsyncEnumerable
Friend Class AsyncEnumerableAdapter(Of T)
    Implements IAsyncEnumerable(Of T)

    Private ReadOnly _returnAsync As Func(Of AsyncEnumerator, Task)

    Public Sub New(returnAsync As Func(Of AsyncEnumerator, Task))
        _returnAsync = returnAsync
    End Sub

    Public Function GetAsyncEnumerator(Optional cancellationToken As CancellationToken = Nothing) As IAsyncEnumerator(Of T) Implements IAsyncEnumerable(Of T).GetAsyncEnumerator
        Return New AsyncEnumerator(_returnAsync, cancellationToken)
    End Function

    Class Builder
        Public Property ReturnAsync As Func(Of AsyncEnumerator, Task)

        Function Build() As AsyncEnumerableAdapter(Of T)
            Return New AsyncEnumerableAdapter(Of T)(ReturnAsync)
        End Function
    End Class

    Class AsyncEnumerator
        Implements IAsyncEnumerator(Of T)

        Private ReadOnly _remainingItems As New ConcurrentQueue(Of T)
        Private ReadOnly _returnAsync As Func(Of AsyncEnumerator, Task)
        Private ReadOnly _cancellationToken As CancellationToken

        Private _state As State
        Private _iterationTask As Task
        Private _onNextItem As New TaskCompletionSource(Of Object)

        Sub New(returnAsync As Func(Of AsyncEnumerator, Task), cancellationToken As CancellationToken)
            _returnAsync = returnAsync
            _cancellationToken = cancellationToken
        End Sub

        Public Sub YieldValue(value As T)
            _remainingItems.Enqueue(value)
            Volatile.Read(_onNextItem).TrySetResult(Nothing)
        End Sub

        Public ReadOnly Property Current As T Implements IAsyncEnumerator(Of T).Current

        Public Function MoveNextAsync() As ValueTask(Of Boolean) Implements IAsyncEnumerator(Of T).MoveNextAsync
            Select Case _state
                Case State.NotStarted
                    _iterationTask = _returnAsync(Me)
                    _state = State.Running
                    Return MoveNextAsync()
                Case State.Running
                    _cancellationToken.ThrowIfCancellationRequested()
                    Return New ValueTask(Of Boolean)(OnRunningMoveNext)
                Case State.TaskEnded
                    Dim cur As T = Nothing
                    Dim result = _remainingItems.TryDequeue(cur)
                    If result Then
                        _Current = cur
                    End If
#If NET8_0_OR_GREATER Then
                    Return ValueTask.FromResult(result)
#Else
                    Return New ValueTask(Of Boolean)(Task.FromResult(result))
#End If
                Case Else
                    ' Faulted  
                    Throw New InvalidOperationException
            End Select
        End Function

        Private Async Function OnRunningMoveNext() As Task(Of Boolean)
            ' Fast path, item already cached.
            Dim cur As T = Nothing
            If _remainingItems.TryDequeue(cur) Then
                If _remainingItems.IsEmpty Then
                    ' Async lock the _remainingItems
                    Volatile.Write(_onNextItem, New TaskCompletionSource(Of Object))
                End If
                _Current = cur
                Return True
            End If
            ' Async wait until next item arrived or iteration task completed
            Await Task.WhenAny(_iterationTask, Volatile.Read(_onNextItem).Task)
            If _iterationTask.IsCanceled OrElse _iterationTask.IsFaulted Then
                _state = State.Faulted
                ' Throw error
                Await _iterationTask
                ' Never executed. For control flow analysis only.
                Throw _iterationTask.Exception
            ElseIf _iterationTask.IsCompleted Then
                _state = State.TaskEnded
            End If
            If _remainingItems.TryDequeue(cur) Then
                If _remainingItems.IsEmpty Then
                    ' Async lock the _remainingItems
                    Volatile.Write(_onNextItem, New TaskCompletionSource(Of Object))
                End If
                _Current = cur
                Return True
            ElseIf _state = State.TaskEnded Then
                ' Iteration task completed and _remainingItems is empty
                Return False
            Else
                ' _onNextItem has been released, but _remainingItems doesn't have value. This should never happen.
                Throw New InvalidOperationException("Concurrent modification detected: lock state of _remainingItems is inconsistent")
            End If
        End Function

        Public Function DisposeAsync() As ValueTask Implements IAsyncDisposable.DisposeAsync
            GC.SuppressFinalize(Me)
#If NET8_0_OR_GREATER Then
            Return ValueTask.CompletedTask
#Else
            Return New ValueTask(Task.CompletedTask)
#End If
        End Function

        Enum State
            NotStarted
            Running
            TaskEnded
            Faulted
        End Enum
    End Class
End Class
