namespace Reprise.UnitTests.Features
{
    [UsesVerify]
    public class Configurations
    {
        private readonly WebApplicationBuilder _Builder;

        private readonly ConfigurationTypeProcessor _Processor = new();

        public Configurations()
        {
            _Builder = WebApplication.CreateBuilder(new[]
            {
                "TestConfiguration:Number=123",
                "TestConfiguration:SubSection:Message=Hello,world!"
            });
            _Builder.Services.Clear();
        }

        [Fact]
        public Task ConfigurationAttribute() => Verify(new ConfigurationAttribute("TestSubSection"));

        [Fact]
        public Task Process()
        {
            _Processor.Process(_Builder, typeof(TestConfiguration));

            return Verify(_Builder);
        }

        [Fact]
        public Task Process_NoAttribute()
        {
            _Processor.Process(_Builder, GetType());

            return Verify(_Builder);
        }

        [Fact]
        public Task PostProcess()
        {
            _Processor.PostProcess(_Builder);

            return Verify(_Builder);
        }

        [Fact]
        public Task AddConfiguration()
        {
            _Processor.AddConfiguration(_Builder, "TestConfiguration", typeof(TestConfiguration));

            return Verify(new
            {
                _Builder,
                TestConfiguration = _Builder.Services.BuildServiceProvider().GetRequiredService<TestConfiguration>()
            });
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public Task AddConfiguration_EmptySubSectionKey(string subSectionKey) =>
            Throws(() => _Processor.AddConfiguration(_Builder, subSectionKey, typeof(TestConfiguration)))
                .UseParameters(subSectionKey)
                .IgnoreStackTrace();

        [Fact]
        public Task AddConfiguration_DuplicateConfiguration()
        {
            _Processor.Process(_Builder, typeof(TestConfiguration));

            return Throws(() => _Processor.AddConfiguration(_Builder, "TestConfiguration", GetType()))
                .IgnoreStackTrace();
        }

        [Fact]
        public Task AddConfiguration_InvalidSubSectionKey() =>
            Throws(() => _Processor.AddConfiguration(_Builder, "InvalidSubSection", typeof(TestConfiguration)))
                .IgnoreStackTrace();

        [Fact]
        public Task AddConfiguration_InvalidValue()
        {
            var builder = WebApplication.CreateBuilder(new[]
            {
                "TestConfiguration:Number=True",
                "TestConfiguration:SubSection:Message=Hello,world!"
            });

            return Throws(() => _Processor.AddConfiguration(builder, "TestConfiguration", typeof(TestConfiguration)))
                .IgnoreStackTrace();
        }

        [Fact]
        public Task AddConfiguration_MissingProperty()
        {
            var builder = WebApplication.CreateBuilder(new[]
            {
                "TestConfiguration:Number=123",
                "TestConfiguration:SubSection:Message=Hello,world!",
                "TestConfiguration:SubSection:Number=456"
            });

            return Throws(() => _Processor.AddConfiguration(builder, "TestConfiguration", typeof(TestConfiguration)))
                .IgnoreStackTrace();
        }
    }

    [Configuration("TestConfiguration")]
    public class TestConfiguration
    {
        public int Number { get; set; }

        public TestConfigurationSection SubSection { get; set; } = null!;
    }

    public class TestConfigurationSection
    {
        public string Message { get; set; } = null!;
    }
}
