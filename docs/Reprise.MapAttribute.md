### [Reprise](Reprise.md 'Reprise')

## MapAttribute Class

Identifies a public static `Handle` method that supports custom HTTP method(s).

```csharp
public class MapAttribute : System.Attribute
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; [System.Attribute](https://docs.microsoft.com/en-us/dotnet/api/System.Attribute 'System.Attribute') &#129106; MapAttribute

Derived  
&#8627; [DeleteAttribute](Reprise.DeleteAttribute.md 'Reprise.DeleteAttribute')  
&#8627; [GetAttribute](Reprise.GetAttribute.md 'Reprise.GetAttribute')  
&#8627; [PatchAttribute](Reprise.PatchAttribute.md 'Reprise.PatchAttribute')  
&#8627; [PostAttribute](Reprise.PostAttribute.md 'Reprise.PostAttribute')  
&#8627; [PutAttribute](Reprise.PutAttribute.md 'Reprise.PutAttribute')
### Constructors

<a name='Reprise.MapAttribute.MapAttribute(string,string)'></a>

## MapAttribute(string, string) Constructor

Creates a new [MapAttribute](Reprise.MapAttribute.md 'Reprise.MapAttribute') with a single HTTP method.

```csharp
public MapAttribute(string method, string route);
```
#### Parameters

<a name='Reprise.MapAttribute.MapAttribute(string,string).method'></a>

`method` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

<a name='Reprise.MapAttribute.MapAttribute(string,string).route'></a>

`route` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

<a name='Reprise.MapAttribute.MapAttribute(string[],string)'></a>

## MapAttribute(string[], string) Constructor

Creates a new [MapAttribute](Reprise.MapAttribute.md 'Reprise.MapAttribute') with multiple HTTP methods.

```csharp
public MapAttribute(string[] methods, string route);
```
#### Parameters

<a name='Reprise.MapAttribute.MapAttribute(string[],string).methods'></a>

`methods` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')[[]](https://docs.microsoft.com/en-us/dotnet/api/System.Array 'System.Array')

<a name='Reprise.MapAttribute.MapAttribute(string[],string).route'></a>

`route` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')