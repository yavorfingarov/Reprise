namespace Reprise.UnitTests.Features.Validation
{
    [UsesVerify]
    public class ValidatorTypeProcessorTests
    {
        private readonly WebApplicationBuilder _Builder = WebApplication.CreateBuilder();

        private readonly ValidatorTypeProcessor _Processor = new();

        public ValidatorTypeProcessorTests()
        {
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
