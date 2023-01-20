# 𝄇 Reprise

> **reprise** /rɪˈpriːz/ _noun_ [French] A repeated part of something, especially a piece of music

Reprise is a micro-framework that brings the REPR (Request-Endpoint-Response) 
pattern and vertical slice architecture into the ASP.NET Core 6/7 Minimal APIs. 
It aims to be unopioniated towards API behavior and to provide a thin layer 
of abstractions that encourages the creation of convention-based and modular implementations.

## Getting started

1. Create a new ASP.NET Core 6/7 empty project.

2. Install the [Reprise NuGet package](https://www.nuget.org/packages/Reprise).

3. Modify your `Program.cs` to use Reprise. 

```csharp
var builder = WebApplication.CreateBuilder();
builder.ConfigureServices();
var app = builder.Build();
app.UseExceptionHandling();
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
    [Put("/users/{id}")]
    public static IResult Handle(int id, UserDto userDto, DataContext context)
    {
        // ...
    }
}
```

In the example `id` comes from the route, `userDto` - from the body, and 
`context` - from the DI container. Check 
[the documentation](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/parameter-binding?view=aspnetcore-7.0) 
for more information about parameter binding.

## Services

Any class (including an endpoint) that has a public parameterless constructor 
and implements `IServiceConfgurator` can configure services. 

```csharp
[Endpoint]
public class GetWeatherEndpoint : IServiceConfigurator
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

## Configuration

You can bind a strongly typed hierarchical configuration using the `ConfigurationAttribute`. 
A simple example would look like this:

```csharp
[Endpoint]
public class GetGreetingsEndpoint
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

Deeper nested sub-sections could as well be bound using a key like `"Foo:Bar"`. Check 
[the documentation](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-7.0) 
for more information about configuration.

## Filters (.NET 7 only)

Reprise supports global and endpoint-specific filters that implement `IEndpointFilter`. 

The filter execution order is determined by an order value that is set implicitly or explicitly. 
A higher value means the filter will be executed after the ones with a lower value. 
Filters having equal order values are run in an arbitrary order. 

Global filters are implicitly assigned an incrementing order value starting from `0` 
according to the registration order. 

```csharp
app.MapEndpoints(options => 
{
    options.AddEndpointFilter<FilterA>(); // implicit order 0
    options.AddEndpointFilter<FilterB>(); // implicit order 1
    options.AddEndpointFilter<FilterC>(); // implicit order 2
    options.AddEndpointFilter<FilterD>(100); // explicit order 100
});
```

Endpoint-specific filters are implicitly assigned an order value of `int.MaxValue`.

```csharp
[Endpoint]
public class DeleteUserEndpoint
{
    [Delete("/users/{id}")]
    [Filter(typeof(FilterE))] // implicit order int.MaxValue
    [Filter(typeof(FilterF))] // implicit order int.MaxValue
    [Filter(typeof(FilterG), -1)] // explicit order -1
    public static IResult Handle(int id, DataContext context)
    {
        // ...
    }
}
```

Check 
[the documentation](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/min-api-filters?view=aspnetcore-7.0) 
for more information about filters.

## Validation

Reprise relies on FluentValidation. On application startup, all `IValidator<T>` implementations are
added with a singleton lifetime. You can then inject the validator in your `Handle` method.

```csharp
[Endpoint]
public class CreateUserEndpoint
{
    [Post("/users")]
    public static IResult Handle(UserDto userDto, IValidator<UserDto> validator, DataContext context)
    {
        validator.ValidateAndThrow(userDto);
        // ...
    }
}

public class UserDtoValidator : AbstractValidator<UserDto>
{
    public UserDtoValidator()
    {
        RuleFor(u => u.FirstName).NotEmpty();
        RuleFor(u => u.LastName).NotEmpty();
    }
}
```

Check 
[the documentation](https://docs.fluentvalidation.net/en/latest/start.html) 
for more information about FluentValidation.

### Validation filter (.NET 7 only)

It's also possible to add a global validation filter. This way, you don't need to manually inject 
validators and validate.

```csharp
app.MapEndpoints(options => options.AddValidationFilter());
```

The filter is registered using an endpoint filter factory, so it will be invoked only on endpoints 
having one or more parameters that are validatable. 

## Exception handling

By default, the exception handler returns no body and:
* On `BadHttpRequestException` (e.g. thrown when the request body couldn't be deserialized) 
logs an error and returns status code `400`.
* On `ValidationException` returns status code `400`.
* On all other exceptions logs an error and returns status code `500`.

To customize exception logging, you can implement `IExceptionLogger`. 
Similarly, you can customize the response body by implementing `IErrorResponseFactory`.

## Authorization

There are two ways to secure your API. You can require authorization only for specific 
endpoints by decorating the `Handle` method with the `AuthorizeAttribute` from 
`Microsoft.AspNetCore.Authorization`. Alternatively, you can require authorization 
for all endpoints and opt-out for specific ones using the `AllowAnonymousAttribute`.

```csharp
app.MapEndpoints(options => options.RequireAuthorization());
```

Check 
[the documentation](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/security?view=aspnetcore-7.0) 
for more information about authentication and authorization.

## CORS

The CORS middleware and attributes work with Reprise exactly the same way as when using pure Minimal APIs. 

Check 
[the documentation](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-7.0#cors) 
for more information about CORS.

## OpenAPI

You can enhance the OpenAPI description of your endpoints by decorating the `Handle` method with attributes.

* `TagsAttribute` assigns custom tags that are typically used to group operations in the Swagger UI. 
If no attribute is found, Reprise extracts a tag from the route of every endpoint. The first segment 
that is not "api" (case insensitive) or a parameter is capitalized and set as a tag.
"/" is used when no match is found. 

* `NameAttribute` assigns a name that is used for link generation and is also treated as the operation ID 
in the OpenAPI description.

* `ProducesAttribute` describes a response returned from an API endpoint. 
**NB:** Make sure you are using the Reprise' attribute and not the one from `Microsoft.AspNetCore.Mvc`.

* `ExcludeFromDescriptionAttribute` marks an endpoint that is excluded from the OpenAPI description.

Check 
[the documentation](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/openapi?view=aspnetcore-7.0) 
for more information about OpenAPI.

## Events

Events are an easy way to do some work in the background using the publish-subscribe pattern. 
On application startup, all `IEventHandler<T>` implementations are added with a scoped lifetime.

```csharp
[Endpoint]
public class PostGreetingsEndpoint
{
    [Post("/greetings")]
    public static IResult Handle(Greeting greeting, IEventBus eventBus)
    {
        eventBus.Publish(greeting);

        return Results.Accepted();
    }
}

public record Greeting(string Message) : IEvent;

public class GreetingHandler : IEventHandler<Greeting>
{
    private readonly ILogger<GreetingHandler> _Logger;

    public GreetingHandler(ILogger<GreetingHandler> logger)
    {
        _Logger = logger;
    }

    public async Task Handle(Greeting payload, CancellationToken stoppingToken)
    {
        await Task.Delay(1_000, stoppingToken);
        _Logger.LogInformation("Received greeting with message: {Message}", payload.Message);
    }
}
```

The `Publish` method returns immediately. Under the hood, Reprise creates a new `IServiceScope`, 
resolves all matching event handlers, and executes them in parallel. 
`IHostApplicationLifetime.ApplicationStopping` is passed as `stoppingToken`.

If a handler throws, the exception is logged.

## Self-checks

Reprise tries to make sure the API will behave exactly the way you would expect. For this reason, 
it performs various self-checks on application startup and throws an `InvalidOperationException` 
when a problem is encountered. The problems covered by those checks include:
* Programming errors (e.g., an endpoint has no public static `Handle` method) 
* Misconfigurations (e.g., a configuration model could not be bound) 
* Code duplications (e.g., a model is validated by multiple validators) 
* Ambiguities (e.g., an HTTP method and route combination is handled by more than one endpoint).

## Performance

Besides the assembly scan at application startup when configuring the DI container 
and discovering endpoints, Reprise doesn't add any performance overhead when handling requests.

### Startup

|        Method |     Mean |     Error |    StdDev |   Median |    Gen0 |    Gen1 | Allocated |
|-------------- |---------:|----------:|----------:|---------:|--------:|--------:|----------:|
|       Reprise | 5.249 ms | 0.4375 ms | 1.2901 ms | 4.896 ms | 31.2500 | 31.2500 | 302.22 KB |
|        Carter | 4.193 ms | 0.2483 ms | 0.7321 ms | 4.032 ms | 31.2500 | 31.2500 | 268.96 KB |
| FastEndpoints | 4.901 ms | 0.3249 ms | 0.9580 ms | 4.797 ms | 31.2500 | 31.2500 | 319.60 KB |
|   MinimalApis | 4.111 ms | 0.2300 ms | 0.6781 ms | 3.998 ms | 31.2500 | 31.2500 | 272.38 KB |

### Request

|        Method |     Mean |   Error |  StdDev |   Gen0 | Allocated |
|-------------- |---------:|--------:|--------:|-------:|----------:|
|       Reprise | 112.1 μs | 0.60 μs | 0.53 μs | 5.6152 |  17.13 KB |
|        Carter | 115.2 μs | 1.28 μs | 1.20 μs | 5.3711 |  16.63 KB |
| FastEndpoints | 123.5 μs | 2.41 μs | 2.68 μs | 5.8594 |  17.91 KB |
|   MinimalApis | 114.9 μs | 0.85 μs | 0.80 μs | 5.3711 |  16.67 KB |

## Further reading

* [API reference](https://github.com/yavorfingarov/Reprise/blob/master/docs/Reprise.md)

* [Sample app](https://github.com/yavorfingarov/Reprise/tree/master/samples/Reprise.SampleApi)

## Support

If you spot any problems and/or have improvement ideas, please share them via
the [issue tracker](https://github.com/yavorfingarov/Reprise/issues).
