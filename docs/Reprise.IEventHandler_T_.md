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

Handles the [payload](Reprise.IEventHandler_T_.md#Reprise.IEventHandler_T_.Handle(T,System.Threading.CancellationToken).payload 'Reprise.IEventHandler<T>.Handle(T, System.Threading.CancellationToken).payload'). [Microsoft.Extensions.Hosting.IHostApplicationLifetime.ApplicationStopping](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.Hosting.IHostApplicationLifetime.ApplicationStopping 'Microsoft.Extensions.Hosting.IHostApplicationLifetime.ApplicationStopping') is passed   
as [stoppingToken](Reprise.IEventHandler_T_.md#Reprise.IEventHandler_T_.Handle(T,System.Threading.CancellationToken).stoppingToken 'Reprise.IEventHandler<T>.Handle(T, System.Threading.CancellationToken).stoppingToken').

```csharp
System.Threading.Tasks.Task Handle(T payload, System.Threading.CancellationToken stoppingToken);
```
#### Parameters

<a name='Reprise.IEventHandler_T_.Handle(T,System.Threading.CancellationToken).payload'></a>

`payload` [T](Reprise.IEventHandler_T_.md#Reprise.IEventHandler_T_.T 'Reprise.IEventHandler<T>.T')

<a name='Reprise.IEventHandler_T_.Handle(T,System.Threading.CancellationToken).stoppingToken'></a>

`stoppingToken` [System.Threading.CancellationToken](https://docs.microsoft.com/en-us/dotnet/api/System.Threading.CancellationToken 'System.Threading.CancellationToken')

#### Returns
[System.Threading.Tasks.Task](https://docs.microsoft.com/en-us/dotnet/api/System.Threading.Tasks.Task 'System.Threading.Tasks.Task')