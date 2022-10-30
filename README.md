# Reprise

Reprise is a micro-framework that brings the REPR (Request-Endpoint-Response) 
pattern and vertical slice architecture into ASP.NET Core 6 Minimal APIs. 
It aims to be unopioniated towards the API behavior and to provide a thin layer 
of abstractions that encourages the creation of convention-based and modular codebases.

## Getting Started

1. Create a new ASP.NET Core 6 empty project.

2. Install the [Reprise NuGet package](https://www.nuget.org/packages/Reprise).

3. Modify your `Program.cs` to use Reprise. 

```csharp
var builder = WebApplication.CreateBuilder();
builder.ConfigureServices();
var app = builder.Build();
app.MapEndpoints();
app.Run();
```

4. Create an endpoint `Endpoints/GetHelloEndpoint.cs`. 

```csharp
[Endpoint]
public class GetHelloEndpoint
{
    [Get("/")]
    public static string Handle() => "Hello, world!";
}
```

5. Run the application.

## Endpoints

An endpoint is a public class that is decorated with the `EndpointAttribute`. 
It should contain a public static `Handle` method which should be decorated 
with one of the HTTP method and route attributes. These are:
* `GetAttribute`
* `PostAttribute`
* `PutAttribute`
* `PatchAttribute`
* `DeleteAttribute`
* `MapAttribute` - for other/multiple HTTP methods

The `Handle` method can be synchronous as well as asynchronous and can have 
any signature and any return type.

```csharp
[Endpoint]
public class UpdateUserEndpoint
{
    [Authorize]
    [Put("/users/{id}")]
    public static IResult Handle(int id, UserDto userDto, DataContext context)
    {
        // ...
    }
}
```

In the example `id` comes from the route, `userDto` - from the body, and 
`context` - from the DI container. Check the 
[Minimal APIs documentation](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis) 
for more information on the topic.

On application startup, `app.MapEndpoints()` and `app.MapEndpoints(assembly)` 
perform an assembly scan and map all endpoints. Reprise will throw 
an `InvalidOperationExcaption` when:
* An endpoint has no `Handle` method
* An endpoint has multiple `Handle` methods
* A `Handle` method has no HTTP method and route attribute
* A `Handle` method has multiple HTTP method and route attributes
* A `Handle` method has an empty route
* A `Handle` method has an empty HTTP method
* An HTTP method and route combination is handled by more than one endpoint

## Services

Any class (including an endpoint) that has a public parameterless constructor 
and implements `IServiceConfgurator` can configure services. 

```csharp
[Endpoint]
public class GetWeather : IServiceConfigurator
{
    public void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddWeatherService();
    }

    [Get("/weather")]
    public static async Task<WeatherForecast[]> Handle(IWeatherService weatherService)
    {
        return await weatherService.GetForecast();
    }
}
```

On application startup, `builder.ConfigureServices()` and 
`builder.ConfigureServices(assembly)` perform an assembly scan and 
invoke all `ConfigureServices(WebApplicationBuilder builder)` implementations. 
Reprise will throw an `InvalidOperationException` if a service configurator 
has no public paramereterless constructor.

## Configuration

Reprise makes it possible to bind a strongly typed hierarchical configuration. 
A simple example would look like this:

```csharp
[Endpoint]
public class GetGreetings
{
    [Get("/greetings")]
    public static IEnumerable<string> Handle(GreetingConfiguration configuration)
    {
        return configuration.Names.Select(name => string.Format(configuration.Message, name));
    }
}

[Configuration("Greeting")]
public class GreetingConfiguration
{
    public string Message { get; set; } = null!;

    public List<string> Names { get; set; } = null!;
}
```

The `GreetingConfiguration` is added to the DI container with a singleton lifetime and 
is bound to the following section in `appsettings.json`:

```json
{
    "Greeting": {
        "Message": "Hello, {0}!",
        "Names": [ "Alice", "John", "Skylar" ]
    }
}
```

Deeper nested sub-sections could as well be bound using a key like `"Foo:Bar"`.

On application startup, `builder.ConfigureServices()` and 
`builder.ConfigureServices(assembly)` perform an assembly scan and 
add all configurations with a singleton lifetime. Reprise will 
throw an `InvalidOperationException` if a configuration model 
could not be bound or when a sub-section is bound to multiple types.

## OpenAPI

In order to group endpoints in the Swagger UI, Reprise extracts a tag from 
the route of every endpoint. The first segment that is not "api" (case insensitive) 
or a parameter is capitalized and set as a tag. "/" is used when no match is found. 

## Performance

Besides the assembly scans at application startup when configuring the DI container 
and mapping endpoints, Reprise doesn't add any performance overhead when handling requests.

## Support

If you spot any problems and/or have improvement ideas, please share them via
the [issue tracker](https://github.com/yavorfingarov/Reprise/issues).
