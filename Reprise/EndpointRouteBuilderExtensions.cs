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
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
        {
            ArgumentNullException.ThrowIfNull(app);

            return app.MapEndpoints(Assembly.GetCallingAssembly());
        }

        /// <summary>
        /// Maps all API endpoints from the specified assembly.
        /// </summary>
        /// <remarks>
        /// An API endpoint is a type decorated with the <see cref="EndpointAttribute"/>.
        /// </remarks>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app, Assembly assembly)
        {
            ArgumentNullException.ThrowIfNull(app);
            ArgumentNullException.ThrowIfNull(assembly);
            var mappedRoutes = new Dictionary<(string Method, string Route), Type>();
            var endpointTypes = assembly.GetExportedTypes()
                .Where(t => !t.IsAbstract && t.GetCustomAttribute<EndpointAttribute>() != null);
            foreach (var endpointType in endpointTypes)
            {
                var handlerInfo = GetHandlerInfo(endpointType);
                var (route, methods) = GetRouteAndMethods(handlerInfo, mappedRoutes);
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

        internal static (string Route, string[] Methods) GetRouteAndMethods(MethodInfo handlerInfo, 
            Dictionary<(string Method, string Route), Type> mappedRoutes)
        {
            var mapAttributes = handlerInfo.GetCustomAttributes<MapAttribute>().ToList();
            if (mapAttributes.Count == 0)
            {
                throw new InvalidOperationException($"{handlerInfo.DeclaringType}.Handle has no HTTP method and route attribute.");
            }
            if (mapAttributes.Count > 1)
            {
                throw new InvalidOperationException($"{handlerInfo.DeclaringType}.Handle has multiple HTTP method and route attributes.");
            }
            var (route, methods) = (mapAttributes[0].Route, mapAttributes[0].Methods);
            if (string.IsNullOrWhiteSpace(route))
            {
                throw new InvalidOperationException($"{handlerInfo.DeclaringType}.Handle has an empty route.");
            }
            if (methods.Any(m => string.IsNullOrWhiteSpace(m)))
            {
                throw new InvalidOperationException($"{handlerInfo.DeclaringType}.Handle has an empty HTTP method.");
            }
            foreach (var method in methods)
            {
                var key = (method, route);
                if (mappedRoutes.TryGetValue(key, out var existingEndpointType))
                {
                    throw new InvalidOperationException(
                        $"{method} {route} is handled by both {handlerInfo.DeclaringType} and {existingEndpointType}.");
                }
                mappedRoutes[key] = handlerInfo.DeclaringType!;
            }

            return (route, methods);
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
