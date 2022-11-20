# Reprise

Reprise is a micro-framework that brings the REPR (Request-Endpoint-Response) 
pattern and vertical slice architecture into the ASP.NET Core 6+ Minimal APIs. 
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
[the documentation](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-7.0#parameter-binding-1) 
for more information on the topic.

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

## Configuration

You can bind a strongly typed hierarchical configuration using the `ConfigurationAttribute`. 
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
        if (!validator.Validate(userDto).IsValid)
        {
            return Results.BadRequest();
        }
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
for more information.

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
for more information.

## OpenAPI

OpenAPI tags are typically used to group operations in the Swagger UI. You can assign custom tag(s) 
by decorating the `Handle` method with the `TagsAttribute`. If no such attribute is found, 
Reprise extracts a tag from the route of every endpoint. The first segment that is 
not "api" (case insensitive) or a parameter is capitalized and set as a tag.
"/" is used when no match is found. 

## Self-checks

Reprise will throw an `InvalidOperationExcaption` on application startup when:
* An endpoint has no `Handle` method
* An endpoint has multiple `Handle` methods
* A `Handle` method has no HTTP method and route attribute
* A `Handle` method has multiple HTTP method and route attributes
* A `Handle` method has an empty route
* A `Handle` method has an empty HTTP method
* An HTTP method and route combination is handled by more than one endpoint
* A service configurator has no public paramereterless constructor
* A configuration model could not be bound (e.g. missing property for a configuration key)
* A configuration sub-section is bound to multiple types
* A model is validated by multiple validators
* An OpenAPI tag is empty

## Performance

Besides the assembly scan at application startup when configuring the DI container 
and discovering endpoints, Reprise doesn't add any performance overhead when handling requests.

## Support

If you spot any problems and/or have improvement ideas, please share them via
the [issue tracker](https://github.com/yavorfingarov/Reprise/issues).
