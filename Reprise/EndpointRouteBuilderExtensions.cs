using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Reprise
{
    /// <summary>
    /// Extension methods for mapping API endpoints.
    /// </summary>
    public static class EndpointRouteBuilderExtensions
    {
        /// <summary>
        /// Maps all API endpoints from the current assembly.
        /// </summary>
        /// <remarks>
        /// An API endpoint is a type decorated with the <see cref="EndpointAttribute"/>.
        /// </remarks>
        /// <exception cref="InvalidOperationException"/>
        public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
        {
            return app.MapEndpoints(Assembly.GetCallingAssembly());
        }

        /// <summary>
        /// Maps all API endpoints from the specified assembly.
        /// </summary>
        /// <remarks>
        /// An API endpoint is a type decorated with the <see cref="EndpointAttribute"/>.
        /// </remarks>
        /// <exception cref="InvalidOperationException"/>
        public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app, Assembly assembly)
        {
            var mappedRoutes = new Dictionary<(string Method, string Route), Type>();
            var endpointTypes = assembly.GetExportedTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.GetCustomAttribute<EndpointAttribute>() != null);
            foreach (var endpointType in endpointTypes)
            {
                var handlerInfo = GetHandlerInfo(endpointType);
                var (route, methods) = GetRouteAndMethods(handlerInfo);
                CheckRoutes(mappedRoutes, route, methods, endpointType);
                var handler = CreateHandler(handlerInfo);
                var tag = GetTag(route);
                app.MapMethods(route, methods, handler).WithTags(tag);
            }

            return app;
        }

        internal static MethodInfo GetHandlerInfo(Type endpointType)
        {
            MethodInfo? handlerInfo;
            try
            {
                handlerInfo = endpointType.GetMethod("Handle", BindingFlags.Public | BindingFlags.Static);
            }
            catch (AmbiguousMatchException)
            {
                throw new InvalidOperationException($"{endpointType} has more than one public static Handle method.");
            }
            if (handlerInfo == null)
            {
                throw new InvalidOperationException($"{endpointType} has no public static Handle method.");
            }

            return handlerInfo;
        }

        internal static (string Route, string[] Methods) GetRouteAndMethods(MethodInfo handlerInfo)
        {
            var mapAttribute = handlerInfo.GetCustomAttribute<MapAttribute>();
            if (mapAttribute == null)
            {
                throw new InvalidOperationException($"{handlerInfo.DeclaringType}.Handle has no HTTP method and route attribute.");
            }
            if (string.IsNullOrWhiteSpace(mapAttribute.Route))
            {
                throw new InvalidOperationException($"{handlerInfo.DeclaringType}.Handle has an empty route.");
            }
            if (mapAttribute.Methods.Any(m => string.IsNullOrWhiteSpace(m)))
            {
                throw new InvalidOperationException($"{handlerInfo.DeclaringType}.Handle has an empty HTTP method.");
            }

            return (mapAttribute.Route, mapAttribute.Methods);
        }

        internal static void CheckRoutes(Dictionary<(string Method, string Route), Type> mappedRoutes,
            string route, string[] methods, Type endpointType)
        {
            foreach (var method in methods)
            {
                var key = (method, route);
                if (mappedRoutes.TryGetValue(key, out var existingEndpointType))
                {
                    throw new InvalidOperationException($"{method} {route} is handled by both {endpointType} and {existingEndpointType}.");
                }
                mappedRoutes[key] = endpointType;
            }
        }

        internal static Delegate CreateHandler(MethodInfo handlerInfo)
        {
            var parameterTypes = handlerInfo.GetParameters().Select(p => p.ParameterType);
            var delegateType = Expression.GetDelegateType(parameterTypes.Append(handlerInfo.ReturnType).ToArray());
            var handler = Delegate.CreateDelegate(delegateType, handlerInfo);

            return handler;
        }

        internal static string GetTag(string route) =>
            route.Split('/', StringSplitOptions.RemoveEmptyEntries)
                .SkipWhile(t => t.Equals("api", StringComparison.OrdinalIgnoreCase) || t.StartsWith('{') || string.IsNullOrWhiteSpace(t))
                .Select(t => new string(t.Select((c, i) => (i == 0) ? char.ToUpper(c) : c).ToArray()))
                .FirstOrDefault("/");
    }
}
