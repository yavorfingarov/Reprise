### [Reprise](Reprise.md 'Reprise')

## TagsAttribute Class

Specifies custom OpenAPI tags for the API endpoint.

```csharp
public sealed class TagsAttribute : System.Attribute
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; [System.Attribute](https://docs.microsoft.com/en-us/dotnet/api/System.Attribute 'System.Attribute') &#129106; TagsAttribute
### Constructors

<a name='Reprise.TagsAttribute.TagsAttribute(string,string[])'></a>

## TagsAttribute(string, string[]) Constructor

Creates a new [TagsAttribute](Reprise.TagsAttribute.md 'Reprise.TagsAttribute').

```csharp
public TagsAttribute(string tag, params string[] additionalTags);
```
#### Parameters

<a name='Reprise.TagsAttribute.TagsAttribute(string,string[]).tag'></a>

`tag` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

<a name='Reprise.TagsAttribute.TagsAttribute(string,string[]).additionalTags'></a>

`additionalTags` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')[[]](https://docs.microsoft.com/en-us/dotnet/api/System.Array 'System.Array')