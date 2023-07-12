namespace Reprise
{
    internal sealed class EndpointTypeProcessor : AbstractTypeProcessor
    {
        internal EndpointMapper EndpointMapper { get; } = new();

        public override void Process(WebApplicationBuilder builder, Type type)
        {
            if (type.IsDefined(typeof(EndpointAttribute)))
            {
                EndpointMapper.Add(type);
            }
        }

        public override void PostProcess(WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton(EndpointMapper);
        }
    }
}
