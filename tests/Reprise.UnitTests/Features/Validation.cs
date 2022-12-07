namespace Reprise.UnitTests.Features
{
    [UsesVerify]
    public class Validation
    {
        private readonly WebApplicationBuilder _Builder;

        private readonly ValidatorTypeProcessor _Processor = new();

        public Validation()
        {
            _Builder = WebApplication.CreateBuilder();
            _Builder.Services.Clear();
        }

        [Fact]
        public Task Process()
        {
            _Processor.Process(_Builder, typeof(StubValidator));

            return Verify(_Builder)
                .UniqueForRuntimeAndVersion();
        }

        [Fact]
        public Task Process_NoImplementation()
        {
            _Processor.Process(_Builder, GetType());

            return Verify(_Builder)
                .UniqueForRuntimeAndVersion();
        }

        [Fact]
        public Task Process_DuplicateValidator()
        {
            _Processor.Process(_Builder, typeof(StubValidator));

            return Throws(() => _Processor.Process(_Builder, typeof(StubDuplicateValidator)))
                .IgnoreStackTrace();
        }

        [Fact]
        public Task PostProcess()
        {
            _Processor.PostProcess(_Builder);

            return Verify(_Builder)
                .UniqueForRuntimeAndVersion();
        }
    }

    internal class StubValidator : AbstractValidator<StubValidatedType>
    {
    }

    internal class StubDuplicateValidator : AbstractValidator<StubValidatedType>
    {
    }

    internal class StubValidatedType
    {
    }
}
