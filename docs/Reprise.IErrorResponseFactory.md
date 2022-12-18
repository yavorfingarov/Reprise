### [Reprise](Reprise.md 'Reprise')

## IErrorResponseFactory Interface

Specifies the contract for creating error responses.

```csharp
public interface IErrorResponseFactory
```
### Methods

<a name='Reprise.IErrorResponseFactory.Create(Reprise.ErrorContext_FluentValidation.ValidationException_)'></a>

## IErrorResponseFactory.Create(ErrorContext<ValidationException>) Method

Creates a response body when handling a [FluentValidation.ValidationException](https://docs.microsoft.com/en-us/dotnet/api/FluentValidation.ValidationException 'FluentValidation.ValidationException').   
Return [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null') to omit the body.

```csharp
object? Create(Reprise.ErrorContext<FluentValidation.ValidationException> context);
```
#### Parameters

<a name='Reprise.IErrorResponseFactory.Create(Reprise.ErrorContext_FluentValidation.ValidationException_).context'></a>

`context` [Reprise.ErrorContext&lt;](Reprise.ErrorContext_TException_.md 'Reprise.ErrorContext<TException>')[FluentValidation.ValidationException](https://docs.microsoft.com/en-us/dotnet/api/FluentValidation.ValidationException 'FluentValidation.ValidationException')[&gt;](Reprise.ErrorContext_TException_.md 'Reprise.ErrorContext<TException>')

#### Returns
[System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object')

<a name='Reprise.IErrorResponseFactory.Create(Reprise.ErrorContext_Microsoft.AspNetCore.Http.BadHttpRequestException_)'></a>

## IErrorResponseFactory.Create(ErrorContext<BadHttpRequestException>) Method

Creates a response body when handling a [Microsoft.AspNetCore.Http.BadHttpRequestException](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.AspNetCore.Http.BadHttpRequestException 'Microsoft.AspNetCore.Http.BadHttpRequestException').   
Return [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null') to omit the body.

```csharp
object? Create(Reprise.ErrorContext<Microsoft.AspNetCore.Http.BadHttpRequestException> context);
```
#### Parameters

<a name='Reprise.IErrorResponseFactory.Create(Reprise.ErrorContext_Microsoft.AspNetCore.Http.BadHttpRequestException_).context'></a>

`context` [Reprise.ErrorContext&lt;](Reprise.ErrorContext_TException_.md 'Reprise.ErrorContext<TException>')[Microsoft.AspNetCore.Http.BadHttpRequestException](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.AspNetCore.Http.BadHttpRequestException 'Microsoft.AspNetCore.Http.BadHttpRequestException')[&gt;](Reprise.ErrorContext_TException_.md 'Reprise.ErrorContext<TException>')

#### Returns
[System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object')

<a name='Reprise.IErrorResponseFactory.Create(Reprise.ErrorContext_System.Exception_)'></a>

## IErrorResponseFactory.Create(ErrorContext<Exception>) Method

Creates a response when handling an [System.Exception](https://docs.microsoft.com/en-us/dotnet/api/System.Exception 'System.Exception').   
Return [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null') to omit the body.

```csharp
object? Create(Reprise.ErrorContext<System.Exception> context);
```
#### Parameters

<a name='Reprise.IErrorResponseFactory.Create(Reprise.ErrorContext_System.Exception_).context'></a>

`context` [Reprise.ErrorContext&lt;](Reprise.ErrorContext_TException_.md 'Reprise.ErrorContext<TException>')[System.Exception](https://docs.microsoft.com/en-us/dotnet/api/System.Exception 'System.Exception')[&gt;](Reprise.ErrorContext_TException_.md 'Reprise.ErrorContext<TException>')

#### Returns
[System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object')