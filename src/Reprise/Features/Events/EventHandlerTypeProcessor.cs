namespace Reprise
{
    internal sealed class EventHandlerTypeProcessor : AbstractTypeProcessor
    {
        internal override void Process(WebApplicationBuilder builder, Type type)
        {
            var interfaceTypes = type.GetInterfaces();
            foreach (var interfaceType in interfaceTypes)
            {
                if (interfaceType.IsGenericType)
                {
                    var genericTypeDefinition = interfaceType.GetGenericTypeDefinition();
                    if (genericTypeDefinition == typeof(IEventHandler<>))
                    {
                        builder.Services.AddScoped(interfaceType, type);

                        break;
                    }
                }
            }
        }

        internal override void PostProcess(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IEventBus, EventBus>();
        }
    }
}
