﻿namespace Reprise.UnitTests.Features.OpenApi
{
    [UsesVerify]
    public class Tags
    {
        private readonly IEndpointRouteBuilder _App = WebApplication.Create();

        private readonly TagsProcessor _Processor = new();

        [Fact]
        public Task TagsAttribute()
        {
            return Verify(new TagsAttribute("Test"));
        }

        [Fact]
        public Task TagsAttribute_MultipleTags()
        {
            return Verify(new TagsAttribute("Test1", "Test2", "Test3"));
        }

        [Fact]
        public Task Process()
        {
            var builder = _App.MapGet("/", () => "Hello, world!");
            var handlerInfo = typeof(StubTagType).GetMethod(nameof(StubTagType.WithTag))!;

            _Processor.Process(builder, handlerInfo, null!, null!);

            return Verify(_App.DataSources);
        }

        [Fact]
        public Task Process_EmptyTag()
        {
            var builder = _App.MapGet("/", () => "Hello, world!");
            var handlerInfo = typeof(StubTagType).GetMethod(nameof(StubTagType.WithEmptyTag))!;

            return Throws(() => _Processor.Process(builder, handlerInfo, null!, null!))
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
            var builder = _App.MapGet("/", () => "Hello, world!");
            var handlerInfo = typeof(StubTagType).GetMethod(nameof(StubTagType.WithoutTag))!;

            _Processor.Process(builder, handlerInfo, null!, route);

            return Verify(_App.DataSources)
                .UseParameters(route);
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
