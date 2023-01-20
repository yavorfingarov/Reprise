## Reprise Namespace

| Classes | |
| :--- | :--- |
| [ApplicationBuilderExtensions](Reprise.ApplicationBuilderExtensions.md 'Reprise.ApplicationBuilderExtensions') | Extension methods for adding custom middleware. |
| [ConfigurationAttribute](Reprise.ConfigurationAttribute.md 'Reprise.ConfigurationAttribute') | Identifies a strongly-typed hierarchical configuration model bound at application startup. |
| [DeleteAttribute](Reprise.DeleteAttribute.md 'Reprise.DeleteAttribute') | Identifies a public static `Handle` method that supports the HTTP DELETE method. |
| [EndpointAttribute](Reprise.EndpointAttribute.md 'Reprise.EndpointAttribute') | Identifies a type that is an API endpoint. |
| [EndpointOptions](Reprise.EndpointOptions.md 'Reprise.EndpointOptions') | Provides configuration for API endpoints. |
| [EndpointRouteBuilderExtensions](Reprise.EndpointRouteBuilderExtensions.md 'Reprise.EndpointRouteBuilderExtensions') | Extension methods for mapping API endpoints. |
| [ErrorContext&lt;TException&gt;](Reprise.ErrorContext_TException_.md 'Reprise.ErrorContext<TException>') | Encapsulates the exception context. |
| [ExcludeFromDescriptionAttribute](Reprise.ExcludeFromDescriptionAttribute.md 'Reprise.ExcludeFromDescriptionAttribute') | Specifies an API endpoint that is excluded from the OpenAPI description. |
| [FilterAttribute](Reprise.FilterAttribute.md 'Reprise.FilterAttribute') | Specifies a filter for the API endpoint. |
| [GetAttribute](Reprise.GetAttribute.md 'Reprise.GetAttribute') | Identifies a public static `Handle` method that supports the HTTP GET method. |
| [MapAttribute](Reprise.MapAttribute.md 'Reprise.MapAttribute') | Identifies a public static `Handle` method that supports custom HTTP method(s). |
| [NameAttribute](Reprise.NameAttribute.md 'Reprise.NameAttribute') | Specifies a name that is used for link generation and is treated as the operation ID in the OpenAPI description. |
| [PatchAttribute](Reprise.PatchAttribute.md 'Reprise.PatchAttribute') | Identifies a public static `Handle` method that supports the HTTP PATCH method. |
| [PostAttribute](Reprise.PostAttribute.md 'Reprise.PostAttribute') | Identifies a public static `Handle` method that supports the HTTP POST method. |
| [ProducesAttribute](Reprise.ProducesAttribute.md 'Reprise.ProducesAttribute') | Describes a response returned from an API endpoint. |
| [PutAttribute](Reprise.PutAttribute.md 'Reprise.PutAttribute') | Identifies a public static `Handle` method that supports the HTTP PUT method. |
| [TagsAttribute](Reprise.TagsAttribute.md 'Reprise.TagsAttribute') | Specifies custom OpenAPI tags for the API endpoint. |
| [WebApplicationBuilderExtensions](Reprise.WebApplicationBuilderExtensions.md 'Reprise.WebApplicationBuilderExtensions') | Extension methods for configuring services at application startup. |

| Interfaces | |
| :--- | :--- |
| [IErrorResponseFactory](Reprise.IErrorResponseFactory.md 'Reprise.IErrorResponseFactory') | Specifies the contract for creating error responses. |
| [IEvent](Reprise.IEvent.md 'Reprise.IEvent') | Marker interface to represent an event. |
| [IEventBus](Reprise.IEventBus.md 'Reprise.IEventBus') | Defines an event bus to encapsulate publishing of events. |
| [IEventHandler&lt;T&gt;](Reprise.IEventHandler_T_.md 'Reprise.IEventHandler<T>') | Specifies the contract to handle events. |
| [IExceptionLogger](Reprise.IExceptionLogger.md 'Reprise.IExceptionLogger') | Specifies the contract for exception logging. |
| [IServiceConfigurator](Reprise.IServiceConfigurator.md 'Reprise.IServiceConfigurator') | Specifies the contract to configure services at application startup. |
