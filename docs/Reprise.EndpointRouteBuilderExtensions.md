### [Reprise](Reprise.md 'Reprise')

## EndpointRouteBuilderExtensions Class

Extension methods for mapping API endpoints.

```csharp
public static class EndpointRouteBuilderExtensions
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; EndpointRouteBuilderExtensions
### Methods

<a name='Reprise.EndpointRouteBuilderExtensions.MapEndpoints(thisMicrosoft.AspNetCore.Routing.IEndpointRouteBuilder)'></a>

## EndpointRouteBuilderExtensions.MapEndpoints(this IEndpointRouteBuilder) Method

Maps all API endpoints discovered by 
[ConfigureServices(this WebApplicationBuilder)](Reprise.WebApplicationBuilderExtensions.md#Reprise.WebApplicationBuilderExtensions.ConfigureServices(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder) 'Reprise.WebApplicationBuilderExtensions.ConfigureServices(this Microsoft.AspNetCore.Builder.WebApplicationBuilder)')
with the default [EndpointOptions](Reprise.EndpointOptions.md 'Reprise.EndpointOptions').

```csharp
public static Microsoft.AspNetCore.Routing.IEndpointRouteBuilder MapEndpoints(this Microsoft.AspNetCore.Routing.IEndpointRouteBuilder app);
```
#### Parameters

<a name='Reprise.EndpointRouteBuilderExtensions.MapEndpoints(thisMicrosoft.AspNetCore.Routing.IEndpointRouteBuilder).app'></a>

`app` [Microsoft.AspNetCore.Routing.IEndpointRouteBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.AspNetCore.Routing.IEndpointRouteBuilder 'Microsoft.AspNetCore.Routing.IEndpointRouteBuilder')

#### Returns
[Microsoft.AspNetCore.Routing.IEndpointRouteBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.AspNetCore.Routing.IEndpointRouteBuilder 'Microsoft.AspNetCore.Routing.IEndpointRouteBuilder')

#### Exceptions

[System.ArgumentNullException](https://docs.microsoft.com/en-us/dotnet/api/System.ArgumentNullException 'System.ArgumentNullException')

[System.InvalidOperationException](https://docs.microsoft.com/en-us/dotnet/api/System.InvalidOperationException 'System.InvalidOperationException')

<a name='Reprise.EndpointRouteBuilderExtensions.MapEndpoints(thisMicrosoft.AspNetCore.Routing.IEndpointRouteBuilder,System.Action_Reprise.EndpointOptions_)'></a>

## EndpointRouteBuilderExtensions.MapEndpoints(this IEndpointRouteBuilder, Action<EndpointOptions>) Method

Maps all API endpoints discovered by 
[ConfigureServices(this WebApplicationBuilder)](Reprise.WebApplicationBuilderExtensions.md#Reprise.WebApplicationBuilderExtensions.ConfigureServices(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder) 'Reprise.WebApplicationBuilderExtensions.ConfigureServices(this Microsoft.AspNetCore.Builder.WebApplicationBuilder)')
with custom [EndpointOptions](Reprise.EndpointOptions.md 'Reprise.EndpointOptions').

```csharp
public static Microsoft.AspNetCore.Routing.IEndpointRouteBuilder MapEndpoints(this Microsoft.AspNetCore.Routing.IEndpointRouteBuilder app, System.Action<Reprise.EndpointOptions> configure);
```
#### Parameters

<a name='Reprise.EndpointRouteBuilderExtensions.MapEndpoints(thisMicrosoft.AspNetCore.Routing.IEndpointRouteBuilder,System.Action_Reprise.EndpointOptions_).app'></a>

`app` [Microsoft.AspNetCore.Routing.IEndpointRouteBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.AspNetCore.Routing.IEndpointRouteBuilder 'Microsoft.AspNetCore.Routing.IEndpointRouteBuilder')

<a name='Reprise.EndpointRouteBuilderExtensions.MapEndpoints(thisMicrosoft.AspNetCore.Routing.IEndpointRouteBuilder,System.Action_Reprise.EndpointOptions_).configure'></a>

`configure` [System.Action&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Action-1 'System.Action`1')[EndpointOptions](Reprise.EndpointOptions.md 'Reprise.EndpointOptions')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Action-1 'System.Action`1')

#### Returns
[Microsoft.AspNetCore.Routing.IEndpointRouteBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.AspNetCore.Routing.IEndpointRouteBuilder 'Microsoft.AspNetCore.Routing.IEndpointRouteBuilder')

#### Exceptions

[System.ArgumentNullException](https://docs.microsoft.com/en-us/dotnet/api/System.ArgumentNullException 'System.ArgumentNullException')

[System.InvalidOperationException](https://docs.microsoft.com/en-us/dotnet/api/System.InvalidOperationException 'System.InvalidOperationException')