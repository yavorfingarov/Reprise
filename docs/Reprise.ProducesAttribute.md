### [Reprise](Reprise.md 'Reprise')

## ProducesAttribute Class

Describes a response returned from an API endpoint.

```csharp
public class ProducesAttribute : System.Attribute
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; [System.Attribute](https://docs.microsoft.com/en-us/dotnet/api/System.Attribute 'System.Attribute') &#129106; ProducesAttribute
### Constructors

<a name='Reprise.ProducesAttribute.ProducesAttribute(int,System.Type,string,string[])'></a>

## ProducesAttribute(int, Type, string, string[]) Constructor

Creates a new [ProducesAttribute](Reprise.ProducesAttribute.md 'Reprise.ProducesAttribute').

```csharp
public ProducesAttribute(int statusCode, System.Type? responseType=null, string? contentType=null, params string[] additionalContentTypes);
```
#### Parameters

<a name='Reprise.ProducesAttribute.ProducesAttribute(int,System.Type,string,string[]).statusCode'></a>

`statusCode` [System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='Reprise.ProducesAttribute.ProducesAttribute(int,System.Type,string,string[]).responseType'></a>

`responseType` [System.Type](https://docs.microsoft.com/en-us/dotnet/api/System.Type 'System.Type')

<a name='Reprise.ProducesAttribute.ProducesAttribute(int,System.Type,string,string[]).contentType'></a>

`contentType` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

<a name='Reprise.ProducesAttribute.ProducesAttribute(int,System.Type,string,string[]).additionalContentTypes'></a>

`additionalContentTypes` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')[[]](https://docs.microsoft.com/en-us/dotnet/api/System.Array 'System.Array')