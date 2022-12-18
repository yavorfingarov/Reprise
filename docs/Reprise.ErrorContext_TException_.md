### [Reprise](Reprise.md 'Reprise')

## ErrorContext<TException> Class

Encapsulates the exception context.

```csharp
public sealed class ErrorContext<TException>
    where TException : System.Exception
```
#### Type parameters

<a name='Reprise.ErrorContext_TException_.TException'></a>

`TException`

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; ErrorContext<TException>
### Properties

<a name='Reprise.ErrorContext_TException_.ActivityId'></a>

## ErrorContext<TException>.ActivityId Property

Gets the current [System.Diagnostics.Activity.Id](https://docs.microsoft.com/en-us/dotnet/api/System.Diagnostics.Activity.Id 'System.Diagnostics.Activity.Id').

```csharp
public string? ActivityId { get; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

<a name='Reprise.ErrorContext_TException_.Exception'></a>

## ErrorContext<TException>.Exception Property

Gets the exception.

```csharp
public TException Exception { get; }
```

#### Property Value
[TException](Reprise.ErrorContext_TException_.md#Reprise.ErrorContext_TException_.TException 'Reprise.ErrorContext<TException>.TException')

<a name='Reprise.ErrorContext_TException_.Request'></a>

## ErrorContext<TException>.Request Property

Gets the [Microsoft.AspNetCore.Http.HttpContext.Request](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.AspNetCore.Http.HttpContext.Request 'Microsoft.AspNetCore.Http.HttpContext.Request').

```csharp
public Microsoft.AspNetCore.Http.HttpRequest Request { get; }
```

#### Property Value
[Microsoft.AspNetCore.Http.HttpRequest](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.AspNetCore.Http.HttpRequest 'Microsoft.AspNetCore.Http.HttpRequest')

<a name='Reprise.ErrorContext_TException_.TraceIdentifier'></a>

## ErrorContext<TException>.TraceIdentifier Property

Gets the [Microsoft.AspNetCore.Http.HttpContext.TraceIdentifier](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.AspNetCore.Http.HttpContext.TraceIdentifier 'Microsoft.AspNetCore.Http.HttpContext.TraceIdentifier').

```csharp
public string TraceIdentifier { get; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')