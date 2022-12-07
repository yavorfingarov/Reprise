namespace Reprise
{
    internal sealed class EndpointTypeProcessor : AbstractTypeProcessor
    {
        internal readonly EndpointMapper _EndpointMapper = new();

        internal override void Process(WebApplicationBuilder builder, Type type)
        {
            if (type.IsDefined(typeof(EndpointAttribute)))
            {
                _EndpointMapper.Add(type);
            }
        }

        internal override void PostProcess(WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton(_EndpointMapper);
        }
    }
}
