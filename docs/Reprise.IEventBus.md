### [Reprise](Reprise.md 'Reprise')

## IEventBus Interface

Defines an event bus to encapsulate publishing of events.

```csharp
public interface IEventBus
```
### Methods

<a name='Reprise.IEventBus.Publish(Reprise.IEvent)'></a>

## IEventBus.Publish(IEvent) Method

Publishes an event.

```csharp
void Publish(Reprise.IEvent payload);
```
#### Parameters

<a name='Reprise.IEventBus.Publish(Reprise.IEvent).payload'></a>

`payload` [IEvent](Reprise.IEvent.md 'Reprise.IEvent')

#### Exceptions

[System.ArgumentNullException](https://docs.microsoft.com/en-us/dotnet/api/System.ArgumentNullException 'System.ArgumentNullException')

### Remarks
The method returns immediately. Under the hood, a new [Microsoft.Extensions.DependencyInjection.IServiceScope](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.DependencyInjection.IServiceScope 'Microsoft.Extensions.DependencyInjection.IServiceScope') is created,   
all matching event handlers are resolved, and are executed in parallel.   
  
If a handler throws, the exception is logged.