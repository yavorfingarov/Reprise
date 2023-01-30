### [Reprise](Reprise.md 'Reprise')

## ApplicationBuilderExtensions Class

Extension methods for adding custom middleware.

```csharp
public static class ApplicationBuilderExtensions
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; ApplicationBuilderExtensions
### Methods

<a name='Reprise.ApplicationBuilderExtensions.UseExceptionHandling(thisMicrosoft.AspNetCore.Builder.IApplicationBuilder)'></a>

## ApplicationBuilderExtensions.UseExceptionHandling(this IApplicationBuilder) Method

Adds the Reprise exception handler. Its behavior can be customized by implementing 
[IExceptionLogger](Reprise.IExceptionLogger.md 'Reprise.IExceptionLogger') and/or [IErrorResponseFactory](Reprise.IErrorResponseFactory.md 'Reprise.IErrorResponseFactory').

```csharp
public static Microsoft.AspNetCore.Builder.IApplicationBuilder UseExceptionHandling(this Microsoft.AspNetCore.Builder.IApplicationBuilder app);
```
#### Parameters

<a name='Reprise.ApplicationBuilderExtensions.UseExceptionHandling(thisMicrosoft.AspNetCore.Builder.IApplicationBuilder).app'></a>

`app` [Microsoft.AspNetCore.Builder.IApplicationBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.AspNetCore.Builder.IApplicationBuilder 'Microsoft.AspNetCore.Builder.IApplicationBuilder')

#### Returns
[Microsoft.AspNetCore.Builder.IApplicationBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.AspNetCore.Builder.IApplicationBuilder 'Microsoft.AspNetCore.Builder.IApplicationBuilder')