namespace Reprise.UnitTests.Features
{
    [UsesVerify]
    public class Authorization
    {
        private readonly IEndpointRouteBuilder _App = WebApplication.Create();

        private readonly AuthorizationProcessor _Processor = new();

        [Fact]
        public Task Process()
        {
            var builder = _App.MapGet("/", () => "Hello, world!");
            var options = new EndpointOptions();
            options.RequireAuthorization();

            _Processor.Process(builder, null!, options, null!);

            return Verify(_App.DataSources)
                .UniqueForRuntimeAndVersion();
        }

        [Fact]
        public Task Process_PolicyName()
        {
            var builder = _App.MapGet("/", () => "Hello, world!");
            var options = new EndpointOptions();
            options.RequireAuthorization("Policy1");

            _Processor.Process(builder, null!, options, null!);

            return Verify(_App.DataSources)
                .UniqueForRuntimeAndVersion();
        }

        [Fact]
        public Task Process_MultiplePolicyNames()
        {
            var builder = _App.MapGet("/", () => "Hello, world!");
            var options = new EndpointOptions();
            options.RequireAuthorization("Policy1", "Policy2");

            _Processor.Process(builder, null!, options, null!);

            return Verify(_App.DataSources)
                .UniqueForRuntimeAndVersion();
        }

        [Fact]
        public Task Process_NoAuthorization()
        {
            var builder = _App.MapGet("/", () => "Hello, world!");
            var options = new EndpointOptions();

            _Processor.Process(builder, null!, options, null!);

            return Verify(_App.DataSources)
                .UniqueForRuntimeAndVersion();
        }
    }
}
