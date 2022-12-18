namespace Reprise
{
    internal abstract class AbstractTypeProcessor
    {
        internal virtual void PostProcess(WebApplicationBuilder builder)
        {
        }

        internal abstract void Process(WebApplicationBuilder builder, Type type);
    }
}
