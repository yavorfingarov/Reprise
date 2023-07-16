namespace Reprise
{
    internal sealed class EventHandlerTypeProcessor : AbstractTypeProcessor
    {
        public override void Process(WebApplicationBuilder builder, Type type)
        {
            if (type.TryGetGenericInterfaceType(typeof(IEventHandler<>), out var interfaceType))
            {
                builder.Services.AddScoped(interfaceType, type);
            }
        }

        public override void PostProcess(WebApplicationBuilder builder)
        {
            builder.Services.TryAddSingleton<TaskRunner>();
            builder.Services.AddScoped<IEventBus, EventBus>();
        }
    }
}
