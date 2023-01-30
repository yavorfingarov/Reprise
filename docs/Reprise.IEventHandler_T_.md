### [Reprise](Reprise.md 'Reprise')

## IEventHandler<T> Interface

Specifies the contract to handle events.

```csharp
public interface IEventHandler<T>
    where T : Reprise.IEvent
```
#### Type parameters

<a name='Reprise.IEventHandler_T_.T'></a>

`T`
### Methods

<a name='Reprise.IEventHandler_T_.Handle(T,System.Threading.CancellationToken)'></a>

## IEventHandler<T>.Handle(T, CancellationToken) Method

Handles an event.

```csharp
System.Threading.Tasks.Task Handle(T payload, System.Threading.CancellationToken cancellationToken);
```
#### Parameters

<a name='Reprise.IEventHandler_T_.Handle(T,System.Threading.CancellationToken).payload'></a>

`payload` [T](Reprise.IEventHandler_T_.md#Reprise.IEventHandler_T_.T 'Reprise.IEventHandler<T>.T')

<a name='Reprise.IEventHandler_T_.Handle(T,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System.Threading.CancellationToken](https://docs.microsoft.com/en-us/dotnet/api/System.Threading.CancellationToken 'System.Threading.CancellationToken')

#### Returns
[System.Threading.Tasks.Task](https://docs.microsoft.com/en-us/dotnet/api/System.Threading.Tasks.Task 'System.Threading.Tasks.Task')