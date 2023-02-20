namespace Reprise.UnitTests.Features.OpenApi
{
    [UsesVerify]
    public class Name
    {
        private readonly IEndpointRouteBuilder _App = WebApplication.Create();

        private readonly RouteHandlerBuilder _Builder;

        private readonly NameProcessor _Processor = new();

        public Name()
        {
            _Builder = _App.MapGet("/", () => "Hello, world!");
        }

        [Fact]
        public Task Process()
        {
            var handlerInfo = typeof(StubNameEndpoint).GetMethod(nameof(StubNameEndpoint.WithName))!;

            _Processor.Process(_Builder, handlerInfo, null!, null!);

            return Verify(_App.DataSources)
                .IgnoreMember("Target")
                .UniqueForRuntimeAndVersion();
        }

        [Fact]
        public Task Process_NoAttribute()
        {
            var handlerInfo = typeof(StubNameEndpoint).GetMethod(nameof(StubNameEndpoint.WithoutName))!;

            _Processor.Process(_Builder, handlerInfo, null!, null!);

            return Verify(_App.DataSources)
                .IgnoreMember("Target")
                .UniqueForRuntimeAndVersion();
        }

        [Fact]
        public Task Process_EmptyName()
        {
            var handlerInfo = typeof(StubNameEndpoint).GetMethod(nameof(StubNameEndpoint.WithEmptyName))!;

            return Throws(() => _Processor.Process(_Builder, handlerInfo, null!, null!))
                .IgnoreStackTrace();
        }

        [Fact]
        public Task Process_Duplicate()
        {
            var handlerInfo = typeof(StubNameEndpoint).GetMethod(nameof(StubNameEndpoint.WithName))!;
            var handlerInfoDuplicate = typeof(StubNameEndpointDuplicate).GetMethod(nameof(StubNameEndpointDuplicate.WithName))!;
            _Processor.Process(_Builder, handlerInfo, null!, null!);

            return Throws(() => _Processor.Process(_Builder, handlerInfoDuplicate, null!, null!))
                .IgnoreStackTrace();
        }
    }

    internal class StubNameEndpoint
    {
        [Name("Test")]
        public static void WithName()
        {
        }

        public static void WithoutName()
        {
        }

        [Name("")]
        public static void WithEmptyName()
        {
        }
    }

    internal class StubNameEndpointDuplicate
    {
        [Name("Test")]
        public static void WithName()
        {
        }
    }
}
