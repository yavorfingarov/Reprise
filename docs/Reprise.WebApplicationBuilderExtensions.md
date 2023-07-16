### [Reprise](Reprise.md 'Reprise')

## WebApplicationBuilderExtensions Class

Extension methods for configuring services at application startup.

```csharp
public static class WebApplicationBuilderExtensions
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; WebApplicationBuilderExtensions
### Methods

<a name='Reprise.WebApplicationBuilderExtensions.ConfigureServices(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder)'></a>

## WebApplicationBuilderExtensions.ConfigureServices(this WebApplicationBuilder) Method

Sets up the DI container by performing an assembly scan to:
- Discover all API endpoints (types decorated with [EndpointAttribute](Reprise.EndpointAttribute.md 'Reprise.EndpointAttribute')).
- Call all [ConfigureServices(WebApplicationBuilder)](Reprise.IServiceConfigurator.md#Reprise.IServiceConfigurator.ConfigureServices(Microsoft.AspNetCore.Builder.WebApplicationBuilder) 'Reprise.IServiceConfigurator.ConfigureServices(Microsoft.AspNetCore.Builder.WebApplicationBuilder)') 
  implementations.
- Bind all configurations (types decorated with [ConfigurationAttribute](Reprise.ConfigurationAttribute.md 'Reprise.ConfigurationAttribute'))
  and add them with a [Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton 'Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton').
- Add all [FluentValidation.IValidator&lt;&gt;](https://docs.microsoft.com/en-us/dotnet/api/FluentValidation.IValidator-1 'FluentValidation.IValidator`1') implementations with a [Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton 'Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton').
- Add all [IMapper&lt;T1,T2&gt;](Reprise.IMapper_T1,T2_.md 'Reprise.IMapper<T1,T2>') implementations with a [Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton 'Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton').
- Add an [IExceptionLogger](Reprise.IExceptionLogger.md 'Reprise.IExceptionLogger') implementation or the default one if none is found.
- Add an [IErrorResponseFactory](Reprise.IErrorResponseFactory.md 'Reprise.IErrorResponseFactory') implementation or the default one if none is found.
- Add the [IEventBus](Reprise.IEventBus.md 'Reprise.IEventBus').
- Add all [IEventHandler&lt;T&gt;](Reprise.IEventHandler_T_.md 'Reprise.IEventHandler<T>') implementations with a [Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped 'Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped').
- Add all [IJob](Reprise.IJob.md 'Reprise.IJob') implementations with a [Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped 'Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped').

```csharp
public static Microsoft.AspNetCore.Builder.WebApplicationBuilder ConfigureServices(this Microsoft.AspNetCore.Builder.WebApplicationBuilder builder);
```
#### Parameters

<a name='Reprise.WebApplicationBuilderExtensions.ConfigureServices(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder).builder'></a>

`builder` [Microsoft.AspNetCore.Builder.WebApplicationBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.AspNetCore.Builder.WebApplicationBuilder 'Microsoft.AspNetCore.Builder.WebApplicationBuilder')

#### Returns
[Microsoft.AspNetCore.Builder.WebApplicationBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.AspNetCore.Builder.WebApplicationBuilder 'Microsoft.AspNetCore.Builder.WebApplicationBuilder')

#### Exceptions

[System.ArgumentNullException](https://docs.microsoft.com/en-us/dotnet/api/System.ArgumentNullException 'System.ArgumentNullException')

[System.InvalidOperationException](https://docs.microsoft.com/en-us/dotnet/api/System.InvalidOperationException 'System.InvalidOperationException')

<a name='Reprise.WebApplicationBuilderExtensions.ConfigureServices(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder,System.Reflection.Assembly)'></a>

## WebApplicationBuilderExtensions.ConfigureServices(this WebApplicationBuilder, Assembly) Method

Sets up the DI container by performing an assembly scan to:
- Discover all API endpoints (types decorated with [EndpointAttribute](Reprise.EndpointAttribute.md 'Reprise.EndpointAttribute')).
- Call all [ConfigureServices(WebApplicationBuilder)](Reprise.IServiceConfigurator.md#Reprise.IServiceConfigurator.ConfigureServices(Microsoft.AspNetCore.Builder.WebApplicationBuilder) 'Reprise.IServiceConfigurator.ConfigureServices(Microsoft.AspNetCore.Builder.WebApplicationBuilder)') 
  implementations.
- Bind all configurations (types decorated with [ConfigurationAttribute](Reprise.ConfigurationAttribute.md 'Reprise.ConfigurationAttribute'))
  and add them with a [Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton 'Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton').
- Add all [FluentValidation.IValidator&lt;&gt;](https://docs.microsoft.com/en-us/dotnet/api/FluentValidation.IValidator-1 'FluentValidation.IValidator`1') implementations with a [Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton 'Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton').
- Add all [IMapper&lt;T1,T2&gt;](Reprise.IMapper_T1,T2_.md 'Reprise.IMapper<T1,T2>') implementations with a [Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton 'Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton').
- Add an [IExceptionLogger](Reprise.IExceptionLogger.md 'Reprise.IExceptionLogger') implementation or the default one if none is found.
- Add an [IErrorResponseFactory](Reprise.IErrorResponseFactory.md 'Reprise.IErrorResponseFactory') implementation or the default one if none is found.
- Add the [IEventBus](Reprise.IEventBus.md 'Reprise.IEventBus').
- Add all [IEventHandler&lt;T&gt;](Reprise.IEventHandler_T_.md 'Reprise.IEventHandler<T>') implementations with a [Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped 'Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped').
- Add all [IJob](Reprise.IJob.md 'Reprise.IJob') implementations with a [Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped 'Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped').

```csharp
public static Microsoft.AspNetCore.Builder.WebApplicationBuilder ConfigureServices(this Microsoft.AspNetCore.Builder.WebApplicationBuilder builder, System.Reflection.Assembly assembly);
```
#### Parameters

<a name='Reprise.WebApplicationBuilderExtensions.ConfigureServices(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder,System.Reflection.Assembly).builder'></a>

`builder` [Microsoft.AspNetCore.Builder.WebApplicationBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.AspNetCore.Builder.WebApplicationBuilder 'Microsoft.AspNetCore.Builder.WebApplicationBuilder')

<a name='Reprise.WebApplicationBuilderExtensions.ConfigureServices(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder,System.Reflection.Assembly).assembly'></a>

`assembly` [System.Reflection.Assembly](https://docs.microsoft.com/en-us/dotnet/api/System.Reflection.Assembly 'System.Reflection.Assembly')

#### Returns
[Microsoft.AspNetCore.Builder.WebApplicationBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.AspNetCore.Builder.WebApplicationBuilder 'Microsoft.AspNetCore.Builder.WebApplicationBuilder')

#### Exceptions

[System.ArgumentNullException](https://docs.microsoft.com/en-us/dotnet/api/System.ArgumentNullException 'System.ArgumentNullException')

[System.InvalidOperationException](https://docs.microsoft.com/en-us/dotnet/api/System.InvalidOperationException 'System.InvalidOperationException')