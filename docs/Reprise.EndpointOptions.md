### [Reprise](Reprise.md 'Reprise')

## EndpointOptions Class

Provides configuration for API endpoints.

```csharp
public sealed class EndpointOptions
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; EndpointOptions
### Methods

<a name='Reprise.EndpointOptions.AddEndpointFilter_TFilter_(System.Nullable_int_)'></a>

## EndpointOptions.AddEndpointFilter<TFilter>(Nullable<int>) Method

Adds a filter for all API endpoints.

```csharp
public void AddEndpointFilter<TFilter>(System.Nullable<int> order=null)
    where TFilter : Microsoft.AspNetCore.Http.IEndpointFilter;
```
#### Type parameters

<a name='Reprise.EndpointOptions.AddEndpointFilter_TFilter_(System.Nullable_int_).TFilter'></a>

`TFilter`
#### Parameters

<a name='Reprise.EndpointOptions.AddEndpointFilter_TFilter_(System.Nullable_int_).order'></a>

`order` [System.Nullable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Nullable-1 'System.Nullable`1')[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Nullable-1 'System.Nullable`1')

<a name='Reprise.EndpointOptions.AddValidationFilter(System.Nullable_int_)'></a>

## EndpointOptions.AddValidationFilter(Nullable<int>) Method

Adds a validation filter for all API endpoints.

```csharp
public void AddValidationFilter(System.Nullable<int> order=null);
```
#### Parameters

<a name='Reprise.EndpointOptions.AddValidationFilter(System.Nullable_int_).order'></a>

`order` [System.Nullable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Nullable-1 'System.Nullable`1')[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Nullable-1 'System.Nullable`1')

<a name='Reprise.EndpointOptions.RequireAuthorization(string[])'></a>

## EndpointOptions.RequireAuthorization(string[]) Method

Enables authorization for all API endpoints.

```csharp
public void RequireAuthorization(params string[] policyNames);
```
#### Parameters

<a name='Reprise.EndpointOptions.RequireAuthorization(string[]).policyNames'></a>

`policyNames` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')[[]](https://docs.microsoft.com/en-us/dotnet/api/System.Array 'System.Array')