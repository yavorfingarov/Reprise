namespace Reprise.UnitTests.Features.OpenApi
{
    [UsesVerify]
    public class ExcludeFromDescription
    {
        private readonly IEndpointRouteBuilder _App = WebApplication.Create();

        private readonly RouteHandlerBuilder _Builder;

        private readonly ExcludeFromDescriptionProcessor _Processor = new();

        public ExcludeFromDescription()
        {
            _Builder = _App.MapGet("/", () => "Hello, world!");
        }

        [Fact]
        public Task Process()
        {
            var handlerInfo = typeof(StubExcludeFromDescriptionType).GetMethod(nameof(StubExcludeFromDescriptionType.Exclude))!;

            _Processor.Process(_Builder, handlerInfo, null!, null!);

            return Verify(_App.DataSources)
                .IgnoreMember("Target")
                .UniqueForRuntimeAndVersion();
        }

        [Fact]
        public Task Process_NoAttribute()
        {
            var handlerInfo = typeof(StubExcludeFromDescriptionType).GetMethod(nameof(StubExcludeFromDescriptionType.Include))!;

            _Processor.Process(_Builder, handlerInfo, null!, null!);

            return Verify(_App.DataSources)
                .IgnoreMember("Target")
                .UniqueForRuntimeAndVersion();
        }
    }

    internal class StubExcludeFromDescriptionType
    {
        [ExcludeFromDescription]
        public static void Exclude()
        {
        }

        public static void Include()
        {
        }
    }
}
