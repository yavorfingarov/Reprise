namespace Reprise.UnitTests.Shared
{
    [UsesVerify]
    public class TypeExtensions
    {
        [Fact]
        public void CreateInstance() => Assert.IsType<TypeExtensions>(typeof(TypeExtensions).CreateInstance());

        [Fact]
        public Task CreateInstance_NoCtor() => Throws(() => typeof(StubTypeNoCtor).CreateInstance()).IgnoreStackTrace();

        [Fact]
        public Task CreateInstance_Nullable() => Throws(() => typeof(int?).CreateInstance()).IgnoreStackTrace();
    }

    public class StubTypeNoCtor
    {
        private StubTypeNoCtor()
        {
        }
    }
}
