namespace Reprise
{
    internal abstract class AbstractTypeProcessor
    {
        public virtual void PostProcess(WebApplicationBuilder builder)
        {
        }

        public abstract void Process(WebApplicationBuilder builder, Type type);
    }
}
