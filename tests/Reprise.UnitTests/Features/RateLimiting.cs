#if NET7_0
namespace Reprise.UnitTests.Features
{
    [UsesVerify]
    public class RateLimiting
    {
        private readonly IEndpointRouteBuilder _App = WebApplication.Create();

        private readonly RateLimitingProcessor _Processor = new();

        [Fact]
        public Task Process()
        {
            var builder = _App.MapGet("/", () => "Hello, world!");
            var options = new EndpointOptions();
            options.RequireRateLimiting("TestPolicy");

            _Processor.Process(builder, null!, options, null!);

            return Verify(_App.DataSources)
                .IgnoreMember("Target")
                .UniqueForRuntimeAndVersion();
        }

        [Fact]
        public Task Process_NoRateLimiting()
        {
            var builder = _App.MapGet("/", () => "Hello, world!");
            var options = new EndpointOptions();

            _Processor.Process(builder, null!, options, null!);

            return Verify(_App.DataSources)
                .IgnoreMember("Target")
                .UniqueForRuntimeAndVersion();
        }
    }
}
#endif
