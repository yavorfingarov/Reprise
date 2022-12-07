namespace Reprise.UnitTests.Features.OpenApi
{
    [UsesVerify]
    public class Produces
    {
        private readonly IEndpointRouteBuilder _App = WebApplication.Create();

        private readonly RouteHandlerBuilder _Builder;

        private readonly ProducesProcessor _Processor = new();

        public Produces()
        {
            _Builder = _App.MapGet("/", () => "Hello, world!");
        }

        [Fact]
        public Task Process()
        {
            var handlerInfo = typeof(StubProducesType).GetMethod(nameof(StubProducesType.WithProduces))!;

            _Processor.Process(_Builder, handlerInfo, null!, null!);

            return Verify(_App.DataSources)
                .UniqueForRuntimeAndVersion();
        }

        [Fact]
        public Task Process_NoAttribute()
        {
            var handlerInfo = typeof(StubProducesType).GetMethod(nameof(StubProducesType.WithoutProduces))!;

            _Processor.Process(_Builder, handlerInfo, null!, null!);

            return Verify(_App.DataSources)
                .UniqueForRuntimeAndVersion();
        }
    }

    internal class StubProducesType
    {
        [Produces(StatusCodes.Status202Accepted, typeof(StubProducesResponse), "application/json", "application/xml")]
        [Produces(StatusCodes.Status418ImATeapot, typeof(StubProducesErrorResponse), "application/json", "application/xml")]
        public static void WithProduces()
        {
        }

        public static void WithoutProduces()
        {
        }
    }

    internal class StubProducesResponse
    {
        public string ErrorMessage { get; set; } = null!;
    }

    internal class StubProducesErrorResponse
    {
        public string ErrorMessage { get; set; } = null!;
    }
}
