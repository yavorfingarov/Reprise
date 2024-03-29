﻿using System.Linq.Expressions;
using Microsoft.AspNetCore.Routing;

namespace Reprise
{
    internal class EndpointMapper
    {
        internal List<Type> EndpointTypes { get; set; } = new();

        internal Dictionary<(string Method, string Route), Type> MappedRoutes { get; set; } = new();

        public void Add(Type type)
        {
            EndpointTypes.Add(type);
        }

        public virtual void MapEndpoints(IEndpointRouteBuilder app, EndpointOptions options,
            IRouteHandlerBuilderProcessor[] processors)
        {
            foreach (var endpointType in EndpointTypes)
            {
                var handlerInfo = GetHandlerInfo(endpointType);
                var (methods, route) = GetMethodsAndRoute(handlerInfo);
                var typeArgs = handlerInfo.GetParameters()
                    .Select(p => p.ParameterType)
                    .Append(handlerInfo.ReturnType)
                    .ToArray();
                var delegateType = Expression.GetDelegateType(typeArgs);
                var handler = Delegate.CreateDelegate(delegateType, handlerInfo);
                var builder = app.MapMethods(route, methods, handler);
                foreach (var processor in processors)
                {
                    processor.Process(builder, handlerInfo, options, route);
                }
            }
        }

        private static MethodInfo GetHandlerInfo(Type endpointType)
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

        private (string[] Methods, string Route) GetMethodsAndRoute(MethodInfo handlerInfo)
        {
            var mapAttribute = GetMapAttribute(handlerInfo);
            if (mapAttribute.Route.IsEmpty())
            {
                throw new InvalidOperationException($"{handlerInfo.GetFullName()} has an empty route.");
            }
            if (mapAttribute.Methods.Any(m => m.IsEmpty()))
            {
                throw new InvalidOperationException($"{handlerInfo.GetFullName()} has an empty HTTP method.");
            }
            foreach (var method in mapAttribute.Methods)
            {
                var key = (method, mapAttribute.Route);
                if (MappedRoutes.TryGetValue(key, out var existingEndpointType))
                {
                    throw new InvalidOperationException(
                        $"{method} {mapAttribute.Route} is handled by both {handlerInfo.DeclaringType} and {existingEndpointType}.");
                }
                MappedRoutes[key] = handlerInfo.DeclaringType!;
            }

            return (mapAttribute.Methods, mapAttribute.Route);
        }

        private static MapAttribute GetMapAttribute(MethodInfo handlerInfo)
        {
            var mapAttributes = handlerInfo.GetCustomAttributes<MapAttribute>().ToList();
            if (mapAttributes.Count == 0)
            {
                throw new InvalidOperationException($"{handlerInfo.GetFullName()} has no HTTP method and route attribute.");
            }
            if (mapAttributes.Count > 1)
            {
                throw new InvalidOperationException($"{handlerInfo.GetFullName()} has multiple HTTP method and route attributes.");
            }

            return mapAttributes[0];
        }
    }
}
