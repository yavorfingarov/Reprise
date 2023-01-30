### [Reprise](Reprise.md 'Reprise')

## IEventBus Interface

Defines an event bus to encapsulate publishing of events.

```csharp
public interface IEventBus
```
### Methods

<a name='Reprise.IEventBus.Publish(Reprise.IEvent)'></a>

## IEventBus.Publish(IEvent) Method

Publishes an event without waiting for handlers to finish execution.

```csharp
void Publish(Reprise.IEvent payload);
```
#### Parameters

<a name='Reprise.IEventBus.Publish(Reprise.IEvent).payload'></a>

`payload` [IEvent](Reprise.IEvent.md 'Reprise.IEvent')

#### Exceptions

[System.ArgumentNullException](https://docs.microsoft.com/en-us/dotnet/api/System.ArgumentNullException 'System.ArgumentNullException')

### Remarks

All matching event handlers are resolved in a new [Microsoft.Extensions.DependencyInjection.IServiceScope](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.DependencyInjection.IServiceScope 'Microsoft.Extensions.DependencyInjection.IServiceScope'), and are executed in parallel.

[Microsoft.Extensions.Hosting.IHostApplicationLifetime.ApplicationStopping](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.Hosting.IHostApplicationLifetime.ApplicationStopping 'Microsoft.Extensions.Hosting.IHostApplicationLifetime.ApplicationStopping') is passed to the handlers as a cancellation token.

If a handler throws, the exception is logged.

<a name='Reprise.IEventBus.PublishAndWait(Reprise.IEvent,System.Threading.CancellationToken)'></a>

## IEventBus.PublishAndWait(IEvent, CancellationToken) Method

Publishes an event and waits for all handlers to finish execution.

```csharp
System.Threading.Tasks.Task PublishAndWait(Reprise.IEvent payload, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='Reprise.IEventBus.PublishAndWait(Reprise.IEvent,System.Threading.CancellationToken).payload'></a>

`payload` [IEvent](Reprise.IEvent.md 'Reprise.IEvent')

<a name='Reprise.IEventBus.PublishAndWait(Reprise.IEvent,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System.Threading.CancellationToken](https://docs.microsoft.com/en-us/dotnet/api/System.Threading.CancellationToken 'System.Threading.CancellationToken')

#### Returns
[System.Threading.Tasks.Task](https://docs.microsoft.com/en-us/dotnet/api/System.Threading.Tasks.Task 'System.Threading.Tasks.Task')

#### Exceptions

[System.ArgumentNullException](https://docs.microsoft.com/en-us/dotnet/api/System.ArgumentNullException 'System.ArgumentNullException')

[System.AggregateException](https://docs.microsoft.com/en-us/dotnet/api/System.AggregateException 'System.AggregateException')

### Remarks

All matching event handlers are resolved in the current [Microsoft.Extensions.DependencyInjection.IServiceScope](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.DependencyInjection.IServiceScope 'Microsoft.Extensions.DependencyInjection.IServiceScope'), and are executed in parallel.

If one or more handlers throw, all exceptions are packed into an [System.AggregateException](https://docs.microsoft.com/en-us/dotnet/api/System.AggregateException 'System.AggregateException').