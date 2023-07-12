namespace Reprise.UnitTests.ExtensionMethods
{
    [UsesVerify]
    public sealed class WebApplicationBuilderExtensionsTests : IDisposable
    {
        private readonly WebApplicationBuilder _Builder = WebApplication.CreateBuilder();

        public WebApplicationBuilderExtensionsTests()
        {
            MockTypeProcessor.Builder = _Builder;
        }

        [Fact]
        public Task Processors()
        {
            var processors = WebApplicationBuilderExtensions.TypeProcessorFactory.CreateAll()
                .Select(p => p.GetType().FullName);

            return Verify(processors);
        }

        [Fact]
        public Task ConfigureServices()
        {
            var mockProcessorA = new MockTypeProcessor("A");
            var mockProcessorB = new MockTypeProcessor("B");
            WebApplicationBuilderExtensions.TypeProcessorFactory = new StubTypeProcessorFactory(mockProcessorA, mockProcessorB);
            _Builder.ConfigureServices();

            return Verify(MockTypeProcessor.Invocations)
                .UniqueForRuntimeAndVersion();
        }

        [Fact]
        public Task ConfigureServices_BuilderNull()
        {
            return Throws(() => WebApplicationBuilderExtensions.ConfigureServices(null!))
                .IgnoreStackTrace();
        }

        [Fact]
        public Task ConfigureServicesAssembly()
        {
            var mockProcessorA = new MockTypeProcessor("A");
            var mockProcessorB = new MockTypeProcessor("B");
            WebApplicationBuilderExtensions.TypeProcessorFactory = new StubTypeProcessorFactory(mockProcessorA, mockProcessorB);
            _Builder.ConfigureServices(GetType().Assembly);

            return Verify(MockTypeProcessor.Invocations)
                .UniqueForRuntimeAndVersion();
        }

        [Fact]
        public Task ConfigureServicesAssembly_BuilderNull()
        {
            return Throws(() => WebApplicationBuilderExtensions.ConfigureServices(null!, GetType().Assembly))
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
            WebApplicationBuilderExtensions.TypeProcessorFactory = new TypeProcessorFactory();
        }
    }

    internal class StubTypeProcessorFactory : TypeProcessorFactory
    {
        private readonly AbstractTypeProcessor[] _Processors;

        internal StubTypeProcessorFactory(params AbstractTypeProcessor[] processors)
        {
            _Processors = processors;
        }

        internal override AbstractTypeProcessor[] CreateAll() => _Processors;
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

        public override void Process(WebApplicationBuilder builder, Type type)
        {
            Assert.Same(Builder, builder);
            Invocations.Add($"{_Name}.Process({type.FullName})");
        }

        public override void PostProcess(WebApplicationBuilder builder)
        {
            Assert.Same(Builder, builder);
            Invocations.Add($"{_Name}.PostProcess()");
        }
    }

    public abstract class StubTypeToSkipInAssemblyScan
    {
    }
}
