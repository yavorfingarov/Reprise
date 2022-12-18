### [Reprise](Reprise.md 'Reprise')

## IExceptionLogger Interface

Specifies the contract for exception logging.

```csharp
public interface IExceptionLogger
```
### Methods

<a name='Reprise.IExceptionLogger.Log(Microsoft.Extensions.Logging.ILogger,Reprise.ErrorContext_FluentValidation.ValidationException_)'></a>

## IExceptionLogger.Log(ILogger, ErrorContext<ValidationException>) Method

Logs a [FluentValidation.ValidationException](https://docs.microsoft.com/en-us/dotnet/api/FluentValidation.ValidationException 'FluentValidation.ValidationException').

```csharp
void Log(Microsoft.Extensions.Logging.ILogger logger, Reprise.ErrorContext<FluentValidation.ValidationException> context);
```
#### Parameters

<a name='Reprise.IExceptionLogger.Log(Microsoft.Extensions.Logging.ILogger,Reprise.ErrorContext_FluentValidation.ValidationException_).logger'></a>

`logger` [Microsoft.Extensions.Logging.ILogger](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.Logging.ILogger 'Microsoft.Extensions.Logging.ILogger')

<a name='Reprise.IExceptionLogger.Log(Microsoft.Extensions.Logging.ILogger,Reprise.ErrorContext_FluentValidation.ValidationException_).context'></a>

`context` [Reprise.ErrorContext&lt;](Reprise.ErrorContext_TException_.md 'Reprise.ErrorContext<TException>')[FluentValidation.ValidationException](https://docs.microsoft.com/en-us/dotnet/api/FluentValidation.ValidationException 'FluentValidation.ValidationException')[&gt;](Reprise.ErrorContext_TException_.md 'Reprise.ErrorContext<TException>')

<a name='Reprise.IExceptionLogger.Log(Microsoft.Extensions.Logging.ILogger,Reprise.ErrorContext_Microsoft.AspNetCore.Http.BadHttpRequestException_)'></a>

## IExceptionLogger.Log(ILogger, ErrorContext<BadHttpRequestException>) Method

Logs a [Microsoft.AspNetCore.Http.BadHttpRequestException](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.AspNetCore.Http.BadHttpRequestException 'Microsoft.AspNetCore.Http.BadHttpRequestException').

```csharp
void Log(Microsoft.Extensions.Logging.ILogger logger, Reprise.ErrorContext<Microsoft.AspNetCore.Http.BadHttpRequestException> context);
```
#### Parameters

<a name='Reprise.IExceptionLogger.Log(Microsoft.Extensions.Logging.ILogger,Reprise.ErrorContext_Microsoft.AspNetCore.Http.BadHttpRequestException_).logger'></a>

`logger` [Microsoft.Extensions.Logging.ILogger](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.Logging.ILogger 'Microsoft.Extensions.Logging.ILogger')

<a name='Reprise.IExceptionLogger.Log(Microsoft.Extensions.Logging.ILogger,Reprise.ErrorContext_Microsoft.AspNetCore.Http.BadHttpRequestException_).context'></a>

`context` [Reprise.ErrorContext&lt;](Reprise.ErrorContext_TException_.md 'Reprise.ErrorContext<TException>')[Microsoft.AspNetCore.Http.BadHttpRequestException](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.AspNetCore.Http.BadHttpRequestException 'Microsoft.AspNetCore.Http.BadHttpRequestException')[&gt;](Reprise.ErrorContext_TException_.md 'Reprise.ErrorContext<TException>')

<a name='Reprise.IExceptionLogger.Log(Microsoft.Extensions.Logging.ILogger,Reprise.ErrorContext_System.Exception_)'></a>

## IExceptionLogger.Log(ILogger, ErrorContext<Exception>) Method

Logs an [System.Exception](https://docs.microsoft.com/en-us/dotnet/api/System.Exception 'System.Exception').

```csharp
void Log(Microsoft.Extensions.Logging.ILogger logger, Reprise.ErrorContext<System.Exception> context);
```
#### Parameters

<a name='Reprise.IExceptionLogger.Log(Microsoft.Extensions.Logging.ILogger,Reprise.ErrorContext_System.Exception_).logger'></a>

`logger` [Microsoft.Extensions.Logging.ILogger](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.Logging.ILogger 'Microsoft.Extensions.Logging.ILogger')

<a name='Reprise.IExceptionLogger.Log(Microsoft.Extensions.Logging.ILogger,Reprise.ErrorContext_System.Exception_).context'></a>

`context` [Reprise.ErrorContext&lt;](Reprise.ErrorContext_TException_.md 'Reprise.ErrorContext<TException>')[System.Exception](https://docs.microsoft.com/en-us/dotnet/api/System.Exception 'System.Exception')[&gt;](Reprise.ErrorContext_TException_.md 'Reprise.ErrorContext<TException>')