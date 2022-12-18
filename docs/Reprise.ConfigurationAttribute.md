### [Reprise](Reprise.md 'Reprise')

## ConfigurationAttribute Class

Identifies a strongly-typed hierarchical configuration model bound at application startup.

```csharp
public sealed class ConfigurationAttribute : System.Attribute
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; [System.Attribute](https://docs.microsoft.com/en-us/dotnet/api/System.Attribute 'System.Attribute') &#129106; ConfigurationAttribute

### Remarks
A configuration class should be public non-abstract with a public parameterless constructor.  
All public read-write properties of the type are bound.
### Constructors

<a name='Reprise.ConfigurationAttribute.ConfigurationAttribute(string)'></a>

## ConfigurationAttribute(string) Constructor

Creates a new [ConfigurationAttribute](Reprise.ConfigurationAttribute.md 'Reprise.ConfigurationAttribute').

```csharp
public ConfigurationAttribute(string subSectionKey);
```
#### Parameters

<a name='Reprise.ConfigurationAttribute.ConfigurationAttribute(string).subSectionKey'></a>

`subSectionKey` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')