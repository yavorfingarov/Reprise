# 𝄇 Reprise

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

Deeper nested sub-sections could as well be bound using a key like `"Foo:Bar"`. Check 
[the documentation](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-7.0) 
for more information about configuration.

## Filters (.NET 7 only)

You can add filters for all API endpoints: 

```csharp
app.MapEndpoints(options => 
{
    options.AddEndpointFilter<FilterA>();
    options.AddEndpointFilter<FilterB>();
    options.AddEndpointFilter<FilterC>();
});
```

The filters should implement `IEndpointFilter` and will be executed in the order of registration. 

You can also apply a filter to specific endpoints by using the `FilterAttribute`. 
Such a filter will be executed after the global ones.

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

It's also possible to add validation as a filter for all endpoints. This way, you don't need 
to manually inject the validator and validate.

```csharp
app.MapEndpoints(options => options.AddValidationFilter());
```

The filter is registered using an endpoint filter factory, so it will be invoked only on endpoints 
having a parameter of type for which a validator is found. If multiple parameters are 
validatable, only the first one will be handled by the filter. 

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

## OpenAPI

You can enhance the OpenAPI description of your endpoints by decorating the `Handle` method with attributes.

* `TagsAttribute` assigns custom tags that are typically used to group operations in the Swagger UI. 
If no attribute is found, Reprise extracts a tag from the route of every endpoint. The first segment 
that is not "api" (case insensitive) or a parameter is capitalized and set as a tag.
"/" is used when no match is found. 

* `NameAttribute` assigns a name that is used for link generation and is also treated as the operation ID 
in the OpenAPI description.

* `ProducesAttribute` describes a response returned from an API endpoint. 
**NB** Make sure you are using the Reprise' attribute and not the one from `Microsoft.AspNetCore.Mvc`.

* `ExcludeFromDescriptionAttribute` marks an endpoint that is excluded from the OpenAPI description.

Check 
[the documentation](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/openapi?view=aspnetcore-7.0) 
for more information about OpenAPI.

## Self-checks

Reprise tries to make sure the API will behave the way you would expect. For this reason, 
it performs various self-checks on application startup and throws an `InvalidOperationException` 
when a problem is encountered. The problems covered by those checks include programming errors 
(e.g., an endpoint has no public static `Handle` method), misconfigurations (e.g., a configuration 
model could not be bound), code duplications (e.g., a model is validated by multiple validators), 
and ambiguities (e.g., an HTTP method and route combination is handled by more than one endpoint).

## Performance

Besides the assembly scan at application startup when configuring the DI container 
and discovering endpoints, Reprise doesn't add any performance overhead when handling requests.

## Support

If you spot any problems and/or have improvement ideas, please share them via
the [issue tracker](https://github.com/yavorfingarov/Reprise/issues).
