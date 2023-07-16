namespace Reprise.UnitTests.Features.Mappers
{
    [UsesVerify]
    public class MapperTypeProcessorTests
    {
        private readonly WebApplicationBuilder _Builder = WebApplication.CreateBuilder();

        private readonly MapperTypeProcessor _Processor = new();

        public MapperTypeProcessorTests()
        {
            _Builder.Services.Clear();
        }

        [Fact]
        public Task Process()
        {
            _Processor.Process(_Builder, typeof(StubTypeMapper));

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
        public Task Process_DuplicateMapper()
        {
            _Processor.Process(_Builder, typeof(StubTypeMapper));

            return Throws(() => _Processor.Process(_Builder, typeof(StubTypeDuplicateMapper)))
                .IgnoreStackTrace();
        }

        [Fact]
        public Task Process_DuplicateMirrorMapper()
        {
            _Processor.Process(_Builder, typeof(StubTypeMapper));

            return Throws(() => _Processor.Process(_Builder, typeof(StubTypeDuplicateMirrorMapper)))
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

    internal class StubType1
    {
    }

    internal class StubType2
    {
    }

    internal class StubTypeMapper : IMapper<StubType1, StubType2>
    {
        public StubType1 Map(StubType2 source) => throw new NotImplementedException();
        public StubType2 Map(StubType1 source) => throw new NotImplementedException();
        public void Map(StubType1 destination, StubType2 source) => throw new NotImplementedException();
        public void Map(StubType2 destination, StubType1 source) => throw new NotImplementedException();
    }

    internal class StubTypeDuplicateMapper : IMapper<StubType1, StubType2>
    {
        public StubType1 Map(StubType2 source) => throw new NotImplementedException();
        public StubType2 Map(StubType1 source) => throw new NotImplementedException();
        public void Map(StubType1 destination, StubType2 source) => throw new NotImplementedException();
        public void Map(StubType2 destination, StubType1 source) => throw new NotImplementedException();
    }

    internal class StubTypeDuplicateMirrorMapper : IMapper<StubType2, StubType1>
    {
        public StubType2 Map(StubType1 source) => throw new NotImplementedException();
        public StubType1 Map(StubType2 source) => throw new NotImplementedException();
        public void Map(StubType2 destination, StubType1 source) => throw new NotImplementedException();
        public void Map(StubType1 destination, StubType2 source) => throw new NotImplementedException();
    }
}
