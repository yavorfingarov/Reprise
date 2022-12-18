### [Reprise](Reprise.md 'Reprise')

## EndpointAttribute Class

Identifies a type that is an API endpoint.

```csharp
public sealed class EndpointAttribute : System.Attribute
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; [System.Attribute](https://docs.microsoft.com/en-us/dotnet/api/System.Attribute 'System.Attribute') &#129106; EndpointAttribute

### Remarks
An API endpoint contains a public static `Handle` method   
that is decorated with a [GetAttribute](Reprise.GetAttribute.md 'Reprise.GetAttribute'), [PostAttribute](Reprise.PostAttribute.md 'Reprise.PostAttribute'),  
[PutAttribute](Reprise.PutAttribute.md 'Reprise.PutAttribute'), [PatchAttribute](Reprise.PatchAttribute.md 'Reprise.PatchAttribute'), [DeleteAttribute](Reprise.DeleteAttribute.md 'Reprise.DeleteAttribute')  
or [MapAttribute](Reprise.MapAttribute.md 'Reprise.MapAttribute').