using Extensions = Reprise.WebApplicationBuilderExtensions;

namespace Reprise.UnitTests
{
    [UsesVerify]
    public sealed class WebApplicationBuilderExtensions : IDisposable
    {
        private readonly WebApplicationBuilder _Builder = WebApplication.CreateBuilder();

        public WebApplicationBuilderExtensions()
        {
            MockTypeProcessor.Builder = _Builder;
        }

        [Fact]
        public Task Processors()
        {
            var processors = Extensions._TypeProcessorFactory.CreateAll()
                .Select(p => p.GetType().FullName);

            return Verify(processors);
        }

        [Fact]
        public Task ConfigureServices()
        {
            var mockProcessorA = new MockTypeProcessor("A");
            var mockProcessorB = new MockTypeProcessor("B");
            Extensions._TypeProcessorFactory = new StubTypeProcessorFactory(mockProcessorA, mockProcessorB);
            _Builder.ConfigureServices();

            return Verify(MockTypeProcessor.Invocations)
                .UniqueForRuntimeAndVersion();
        }

        [Fact]
        public Task ConfigureServices_BuilderNull()
        {
            return Throws(() => Extensions.ConfigureServices(null!))
                .IgnoreStackTrace();
        }

        [Fact]
        public Task ConfigureServicesAssembly()
        {
            var mockProcessorA = new MockTypeProcessor("A");
            var mockProcessorB = new MockTypeProcessor("B");
            Extensions._TypeProcessorFactory = new StubTypeProcessorFactory(mockProcessorA, mockProcessorB);
            _Builder.ConfigureServices(GetType().Assembly);

            return Verify(MockTypeProcessor.Invocations)
                .UniqueForRuntimeAndVersion();
        }

        [Fact]
        public Task ConfigureServicesAssembly_BuilderNull()
        {
            return Throws(() => Extensions.ConfigureServices(null!, GetType().Assembly))
                .IgnoreStackTrace();
        }

        [Fact]
        public Task ConfigureServicesAssembly_AssemblyNull()
        {
            return Throws(() => _Builder.ConfigureServices(null!))
                .IgnoreStackTrace();
        }

        public void Dispose()
        {
            Extensions._TypeProcessorFactory = new TypeProcessorFactory();
        }
    }

    internal class StubTypeProcessorFactory : TypeProcessorFactory
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
        public static List<string> Invocations = new();

        public static WebApplicationBuilder? Builder;

        private readonly string _Name;

        internal MockTypeProcessor(string id)
        {
            _Name = $"{nameof(MockTypeProcessor)}{id}";
            Invocations = new();
        }

        internal override void Process(WebApplicationBuilder builder, Type type)
        {
            Assert.Same(Builder, builder);
            Invocations.Add($"{_Name}.Process({type.FullName})");
        }

        internal override void PostProcess(WebApplicationBuilder builder)
        {
            Assert.Same(Builder, builder);
            Invocations.Add($"{_Name}.PostProcess()");
        }
    }

    public abstract class StubTypeToSkipInAssemblyScan
    {
    }
}
