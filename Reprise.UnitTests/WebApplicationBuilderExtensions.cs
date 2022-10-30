using Extensions = Reprise.WebApplicationBuilderExtensions;

namespace Reprise.UnitTests
{
    [UsesVerify]
    public sealed class WebApplicationBuilderExtensions : IDisposable
    {
        private readonly WebApplicationBuilder _Builder = WebApplication.CreateBuilder();

        [Fact]
        public Task Processors() => Verify(Extensions._TypeProcessorFactory.CreateAll().Select(p => p.GetType().FullName));

        [Fact]
        public Task ConfigureServices()
        {
            var mockProcessorA = new MockTypeProcessor(_Builder, "A");
            var mockProcessorB = new MockTypeProcessor(_Builder, "B");
            Extensions._TypeProcessorFactory = new StubTypeProcessorFactory(mockProcessorA, mockProcessorB);
            _Builder.ConfigureServices();

            return Verify(MockTypeProcessor._Invocations);
        }

        [Fact]
        public Task ConfigureServices_BuilderNull() => Throws(() => Extensions.ConfigureServices(null!)).IgnoreStackTrace();

        [Fact]
        public Task ConfigureServicesAssembly()
        {
            var mockProcessorA = new MockTypeProcessor(_Builder, "A");
            var mockProcessorB = new MockTypeProcessor(_Builder, "B");
            Extensions._TypeProcessorFactory = new StubTypeProcessorFactory(mockProcessorA, mockProcessorB);
            _Builder.ConfigureServices(typeof(Program).Assembly);

            return Verify(MockTypeProcessor._Invocations);
        }

        [Fact]
        public Task ConfigureServicesAssembly_BuilderNull() =>
            Throws(() => Extensions.ConfigureServices(null!, typeof(Program).Assembly))
                .IgnoreStackTrace();

        [Fact]
        public Task ConfigureServicesAssembly_AssemblyNull() => Throws(() => _Builder.ConfigureServices(null!)).IgnoreStackTrace();

        public void Dispose() => Extensions._TypeProcessorFactory = new TypeProcessorFactory();
    }

    internal sealed class StubTypeProcessorFactory : TypeProcessorFactory
    {
        private readonly List<AbstractTypeProcessor> _Processors;

        internal StubTypeProcessorFactory(params AbstractTypeProcessor[] processors)
        {
            _Processors = processors.ToList();
        }

        internal override List<AbstractTypeProcessor> CreateAll() => _Processors;
    }

    internal class MockTypeProcessor : AbstractTypeProcessor
    {
        internal static List<string> _Invocations = new();

        private static WebApplicationBuilder? _Builder;

        private readonly string _Name;

        internal MockTypeProcessor(WebApplicationBuilder builder, string id)
        {
            _Builder = builder;
            _Name = $"{nameof(MockTypeProcessor)}{id}";
            _Invocations = new();
        }

        internal override void Process(WebApplicationBuilder builder, Type type)
        {
            EnsureSameBuilder(builder);
            _Invocations.Add($"{_Name}.Process({type.FullName})");
        }

        internal override void PostProcess(WebApplicationBuilder builder)
        {
            EnsureSameBuilder(builder);
            _Invocations.Add($"{_Name}.PostProcess()");
        }

        private static void EnsureSameBuilder(WebApplicationBuilder builder)
        {
            if (_Builder != builder)
            {
                throw new InvalidOperationException();
            }
        }
    }
}
