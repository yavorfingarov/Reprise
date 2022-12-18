namespace Reprise
{
    internal interface IRouteHandlerBuilderProcessor
    {
        void Process(RouteHandlerBuilder builder, MethodInfo handlerInfo, EndpointOptions options, string route);
    }
}
