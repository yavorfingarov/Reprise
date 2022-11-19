using System.Reflection;

namespace Reprise.UnitTests.Features.Endpoints
{
    [UsesVerify]
    public class EndpointMapperTests
    {
        private readonly IEndpointRouteBuilder _App = WebApplication.Create();

        private readonly EndpointOptions _Options = new();

        private readonly List<IRouteHandlerBuilderProcessor> _Processors = new();

        private readonly EndpointMapper _EndpointMapper = new();

        public EndpointMapperTests()
        {
            MockRouteHandlerBuilderProcessor.Options = _Options;
        }

        [Fact]
        public Task Add()
        {
            _EndpointMapper.Add(typeof(StubGetEndpoint));
            _EndpointMapper.Add(typeof(StubPostEndpoint));

            return Verify(_EndpointMapper._EndpointTypes.Select(t => t.FullName));
        }

        [Fact]
        public Task MapEndpoints()
        {
            _EndpointMapper.Add(typeof(StubGetEndpoint));
            _EndpointMapper.Add(typeof(StubPostEndpoint));
            _Processors.Add(new MockRouteHandlerBuilderProcessor("A"));
            _Processors.Add(new MockRouteHandlerBuilderProcessor("B"));

            _EndpointMapper.MapEndpoints(_App, _Options, _Processors);

            var snapshot = new
            {
                _App.DataSources,
                MockRouteHandlerBuilderProcessor.Invocations,
                _EndpointMapper._EndpointTypes,
                _EndpointMapper._MappedRoutes
            };

            return Verify(snapshot);
        }

        [Fact]
        public Task MapEndpoints_MultipleHandleMethods()
        {
            _EndpointMapper.Add(typeof(StubEndpointMultipleHandleMethod));

            return Throws(() => _EndpointMapper.MapEndpoints(_App, _Options, _Processors))
                .IgnoreStackTrace();
        }

        [Fact]
        public Task MapEndpoints_NoHandleMethod()
        {
            _EndpointMapper.Add(typeof(StubEndpointNoHandleMethod));

            return Throws(() => _EndpointMapper.MapEndpoints(_App, _Options, _Processors))
                .IgnoreStackTrace();
        }

        [Fact]
        public Task MapEndpoints_NoMethodRouteAttribute()
        {
            _EndpointMapper.Add(typeof(StubEndpointNoMethodRouteAttribute));

            return Throws(() => _EndpointMapper.MapEndpoints(_App, _Options, _Processors))
                .IgnoreStackTrace();
        }

        [Fact]
        public Task MapEndpoints_MultipleMethodRouteAttributes()
        {
            _EndpointMapper.Add(typeof(StubEndpointMultipleMethodRouteAttributes));

            return Throws(() => _EndpointMapper.MapEndpoints(_App, _Options, _Processors))
                .IgnoreStackTrace();
        }

        [Fact]
        public Task MapEndpoints_EmptyHttpMethod()
        {
            _EndpointMapper.Add(typeof(StubEndpointEmptyHttpMethod));

            return Throws(() => _EndpointMapper.MapEndpoints(_App, _Options, _Processors))
                .IgnoreStackTrace();
        }

        [Fact]
        public Task MapEndpoints_EmptyRoute()
        {
            _EndpointMapper.Add(typeof(StubEndpointEmptyRoute));

            return Throws(() => _EndpointMapper.MapEndpoints(_App, _Options, _Processors))
                .IgnoreStackTrace();
        }

        [Fact]
        public Task MapEndpoints_DuplicateRoute()
        {
            _EndpointMapper.Add(typeof(StubGetEndpoint));
            _EndpointMapper.Add(typeof(StubDuplicateGetEndpoint));

            return Throws(() => _EndpointMapper.MapEndpoints(_App, _Options, _Processors))
                .IgnoreStackTrace();
        }
    }

    internal class MockRouteHandlerBuilderProcessor : IRouteHandlerBuilderProcessor
    {
        public static EndpointOptions? Options;

        public static List<string> Invocations = new();

        private readonly string _Name;

        internal MockRouteHandlerBuilderProcessor(string id)
        {
            _Name = $"{nameof(MockTypeProcessor)}{id}";
        }

        public void Process(RouteHandlerBuilder builder, MethodInfo handlerInfo, EndpointOptions options, string route)
        {
            Assert.Same(Options, options);
            Invocations.Add($"{_Name}.Process({handlerInfo.DeclaringType}.{handlerInfo.Name}, {route})");
        }
    }

    internal class StubGetEndpoint
    {
        [Get("/")]
        public static string Handle() => "Hello, world!";
    }

    internal class StubPostEndpoint
    {
        [Post("/")]
        public static string Handle() => "Hello, world!";
    }

    internal class StubEndpointMultipleHandleMethod
    {
        public static string Handle() => "Hello, world!";

        public static string Handle(string input) => input;
    }

    internal class StubEndpointNoHandleMethod
    {
        public static string NoHandle() => "Hello, world!";
    }

    internal class StubEndpointNoMethodRouteAttribute
    {
        public static string Handle() => "Hello, world!";
    }

    internal class StubEndpointMultipleMethodRouteAttributes
    {
        [Get("/")]
        [Post("/")]
        public static string Handle() => "Hello, world!";
    }

    internal class StubEndpointEmptyHttpMethod
    {
        [Map(new[] { "GET", "" }, "/")]
        public static string Handle() => "Hello, world!";
    }

    internal class StubEndpointEmptyRoute
    {
        [Get("")]
        public static string Handle() => "Hello, world!";
    }

    internal class StubDuplicateGetEndpoint
    {
        [Get("/")]
        public static string Handle() => "Hello, world!";
    }
}
