﻿namespace Reprise.UnitTests.Features.OpenApi.Tags
{
    [UsesVerify]
    public class TagsProcessorTests
    {
        private readonly IEndpointRouteBuilder _App = WebApplication.Create();

        private readonly RouteHandlerBuilder _Builder;

        private readonly TagsProcessor _Processor = new();

        public TagsProcessorTests()
        {
            _Builder = _App.MapGet("/", () => "Hello, world!");
        }

        [Fact]
        public Task Process()
        {
            var handlerInfo = typeof(StubTagType).GetMethod(nameof(StubTagType.WithTag))!;

            _Processor.Process(_Builder, handlerInfo, null!, null!);

            return Verify(_App.DataSources)
                .IgnoreMember("Target")
                .UniqueForRuntimeAndVersion();
        }

        [Fact]
        public Task Process_EmptyTag()
        {
            var handlerInfo = typeof(StubTagType).GetMethod(nameof(StubTagType.WithEmptyTag))!;

            return Throws(() => _Processor.Process(_Builder, handlerInfo, null!, null!))
                .IgnoreStackTrace();
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("{id}")]
        [InlineData("/")]
        [InlineData("/{id}")]
        [InlineData("/api")]
        [InlineData("/api/{id}")]
        [InlineData("/users/{id}")]
        [InlineData("/{id}/users")]
        [InlineData("/users")]
        [InlineData("/api/users")]
        [InlineData("/api/users/{id}")]
        public Task Process_NoAttribute(string route)
        {
            var handlerInfo = typeof(StubTagType).GetMethod(nameof(StubTagType.WithoutTag))!;

            _Processor.Process(_Builder, handlerInfo, null!, route);

            return Verify(_App.DataSources)
                .IgnoreMember("Target")
                .UseParameters(route)
                .UniqueForRuntimeAndVersion();
        }
    }

    public class StubTagType
    {
        [Tags("Test")]
        public static void WithTag()
        {
        }

        [Tags("Test", "   ")]
        public static void WithEmptyTag()
        {
        }

        public static void WithoutTag()
        {
        }
    }
}
