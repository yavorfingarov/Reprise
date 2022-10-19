# Reprise

Reprise is a micro-framework that brings REPR (Request-Endpoint-Respone) pattern and vertical slice architecture into ASP.NET Core Minimal APIs. 

## Getting Started

1. Create a new ASP.NET Core empty project.

2. Install the [Reprise NuGet package](https://www.nuget.org/packages/Reprise).

3. Modify your `Program.cs` to use Reprise. 

```csharp
var builder = WebApplication.CreateBuilder(args);
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

An endpoint is a public class that is decorated with the `EndpointAttribute`. It should contain a public static `Handle` method which should be decorated with one of the HTTP method and route attributes. These are:
* `GetAttribute`
* `PostAttribute`
* `PutAttribute`
* `PatchAttribute`
* `DeleteAttribute`
* `MapAttribute` - for other/multiple HTTP methods

The `Handle` method can be synchronous as well as asynchronous and can have any signature and any return type.

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

In the example `id` comes from the route, `userDto` - from the body and `context` - from the DI container. Check [Minimal APIs documentation](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis) for more information on the topic.

`app.MapEndpoints()` and `app.MapEndpoints(assembly)` perform an assebly scan and map all endpoints on application startup.

## Services

Any class that has a public parameterless constructor and implements `IServiceConfgurator` can configure services. 

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

`builder.ConfigureServices()` and `builder.ConfigureServices(assembly)` perform an assebly scan and invoke all `ConfigureServices(WebApplicationBuilder builder)` implementations on application startup. 

## OpenAPI

In order to group enpoints in the Swagger UI, Reprise extracts a tag from the route of every endpoint. The first segment that is not "api" (case insensitive) or a parameter is capitalized and set as tag. "/" is used when no match is found. 

## Performance

Besides the assembly scans at application startup when configuring services and mapping endpoints, Reprise doesn't add any performance overhead when handling requests.

## Support

If you spot any problems and/or have improvement ideas, please share them via
the [issue tracker](https://github.com/yavorfingarov/Reprise/issues).
