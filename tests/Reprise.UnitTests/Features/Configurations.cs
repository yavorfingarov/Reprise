namespace Reprise.UnitTests.Features
{
    [UsesVerify]
    public class Configurations
    {
        private readonly ConfigurationTypeProcessor _Processor = new();

        [Fact]
        public Task Process()
        {
            var builder = WebApplication.CreateBuilder(new[]
            {
                "TestConfiguration:Number=123",
                "TestConfiguration:Message=Hello,world!"
            });
            builder.Services.Clear();

            _Processor.Process(builder, typeof(StubConfiguration));

            return Verify(builder)
                .UniqueForRuntimeAndVersion();
        }

        [Fact]
        public Task Process_Complex()
        {
            var builder = WebApplication.CreateBuilder(new[]
            {
                "TestConfiguration:Number=123",
                "TestConfiguration:SubSection:Message=Hello,world!"
            });
            builder.Services.Clear();

            _Processor.Process(builder, typeof(StubConfigurationComplex));

            return Verify(builder)
                .UniqueForRuntimeAndVersion();
        }

        [Fact]
        public Task Process_NoAttribute()
        {
            var builder = WebApplication.CreateBuilder();
            builder.Services.Clear();

            _Processor.Process(builder, typeof(StubConfigurationSection));

            return Verify(builder)
                .UniqueForRuntimeAndVersion();
        }

        [Fact]
        public Task Process_EmptySubSectionKey()
        {
            var builder = WebApplication.CreateBuilder();

            return Throws(() => _Processor.Process(builder, typeof(StubConfigurationEmptySubSectionKey)))
                .IgnoreStackTrace();
        }

        [Fact]
        public Task Process_DuplicateConfiguration()
        {
            var builder = WebApplication.CreateBuilder(new[]
            {
                "TestConfiguration:Number=123",
                "TestConfiguration:Message=Hello,world!"
            });
            _Processor.Process(builder, typeof(StubConfiguration));

            return Throws(() => _Processor.Process(builder, typeof(StubConfigurationDuplicate)))
                .IgnoreStackTrace();
        }

        [Fact]
        public Task Process_InvalidSubSectionKey()
        {
            var builder = WebApplication.CreateBuilder();

            return Throws(() => _Processor.Process(builder, typeof(StubConfigurationInvalidSubSection)))
                .IgnoreStackTrace();
        }

        [Fact]
        public Task Process_NoParameterlessCtor()
        {
            var builder = WebApplication.CreateBuilder(new[]
            {
                "TestConfiguration:Number=123",
                "TestConfiguration:SubSection:Message=Hello,world!"
            });

            return Throws(() => _Processor.Process(builder, typeof(StubConfigurationNoParameterlessCtor)))
                .IgnoreStackTrace();
        }

        [Fact]
        public Task Process_InvalidValue()
        {
            var builder = WebApplication.CreateBuilder(new[]
            {
                "TestConfiguration:Number=True",
                "TestConfiguration:Message=Hello,world!"
            });

            return Throws(() => _Processor.Process(builder, typeof(StubConfiguration)))
                .IgnoreStackTrace()
                .UniqueForRuntimeAndVersion();
        }

        [Fact]
        public Task Process_ComplexInvalidValue()
        {
            var builder = WebApplication.CreateBuilder(new[]
            {
                "TestConfiguration:Number=True",
                "TestConfiguration:SubSection:Message=Hello,world!"
            });

            return Throws(() => _Processor.Process(builder, typeof(StubConfigurationComplex)))
                .IgnoreStackTrace()
                .UniqueForRuntimeAndVersion();
        }

        [Fact]
        public Task Process_MissingProperty()
        {
            var builder = WebApplication.CreateBuilder(new[]
            {
                "TestConfiguration:Number=123",
                "TestConfiguration:Message=Hello,world!",
                "TestConfiguration:AnotherNumber=456"
            });

            return Throws(() => _Processor.Process(builder, typeof(StubConfiguration)))
                .IgnoreStackTrace();
        }

        [Fact]
        public Task Process_ComplexMissingProperty()
        {
            var builder = WebApplication.CreateBuilder(new[]
            {
                "TestConfiguration:Number=123",
                "TestConfiguration:SubSection:Message=Hello,world!",
                "TestConfiguration:SubSection:Number=456"
            });

            return Throws(() => _Processor.Process(builder, typeof(StubConfigurationComplex)))
                .IgnoreStackTrace();
        }

        [Fact]
        public Task PostProcess()
        {
            var builder = WebApplication.CreateBuilder();
            builder.Services.Clear();

            _Processor.PostProcess(builder);

            return Verify(builder)
                .UniqueForRuntimeAndVersion();
        }
    }

    [Configuration("TestConfiguration")]
    internal class StubConfiguration
    {
        public int Number { get; set; }

        public string Message { get; set; } = null!;
    }

    [Configuration("TestConfiguration")]
    internal class StubConfigurationComplex
    {
        public int Number { get; set; }

        public StubConfigurationSection SubSection { get; set; } = null!;
    }

    internal class StubConfigurationSection
    {
        public string Message { get; set; } = null!;
    }

    [Configuration("   ")]
    internal class StubConfigurationEmptySubSectionKey
    {
    }

    [Configuration("TestConfiguration")]
    internal class StubConfigurationDuplicate
    {
    }

    [Configuration("InvalidKey")]
    internal class StubConfigurationInvalidSubSection
    {
    }

    [Configuration("TestConfiguration")]
    internal record StubConfigurationNoParameterlessCtor(int Number, string Message);
}
