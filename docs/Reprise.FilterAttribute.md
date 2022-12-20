### [Reprise](Reprise.md 'Reprise')

## FilterAttribute Class

Specifies a filter for the API endpoint.

```csharp
public sealed class FilterAttribute : System.Attribute
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; [System.Attribute](https://docs.microsoft.com/en-us/dotnet/api/System.Attribute 'System.Attribute') &#129106; FilterAttribute
### Constructors

<a name='Reprise.FilterAttribute.FilterAttribute(System.Type)'></a>

## FilterAttribute(Type) Constructor

Creates a new [FilterAttribute](Reprise.FilterAttribute.md 'Reprise.FilterAttribute') with default order value.

```csharp
public FilterAttribute(System.Type filterType);
```
#### Parameters

<a name='Reprise.FilterAttribute.FilterAttribute(System.Type).filterType'></a>

`filterType` [System.Type](https://docs.microsoft.com/en-us/dotnet/api/System.Type 'System.Type')

<a name='Reprise.FilterAttribute.FilterAttribute(System.Type,int)'></a>

## FilterAttribute(Type, int) Constructor

Creates a new [FilterAttribute](Reprise.FilterAttribute.md 'Reprise.FilterAttribute') with custom order value.

```csharp
public FilterAttribute(System.Type filterType, int order);
```
#### Parameters

<a name='Reprise.FilterAttribute.FilterAttribute(System.Type,int).filterType'></a>

`filterType` [System.Type](https://docs.microsoft.com/en-us/dotnet/api/System.Type 'System.Type')

<a name='Reprise.FilterAttribute.FilterAttribute(System.Type,int).order'></a>

`order` [System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')