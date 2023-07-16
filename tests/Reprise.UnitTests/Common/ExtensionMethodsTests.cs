namespace Reprise.UnitTests.Common
{
    [UsesVerify]
    public class ExtensionMethodsTests
    {
        [Fact]
        public void CreateInstance()
        {
            Assert.IsType<StubType>(typeof(StubType).CreateInstance());
        }

        [Fact]
        public Task CreateInstance_NoCtor()
        {
            return Throws(() => typeof(StubTypeNoCtor).CreateInstance())
                .IgnoreStackTrace();
        }

        [Fact]
        public Task CreateInstance_Nullable()
        {
            return Throws(() => typeof(int?).CreateInstance())
                .IgnoreStackTrace();
        }

        [Fact]
        public void GetInternalDependency()
        {
            var builder = WebApplication.CreateBuilder();
            builder.Services.AddSingleton<StubType>();
            var app = builder.Build();

            Assert.IsType<StubType>(app.Services.GetInternalDependency<StubType>());
        }

        [Fact]
        public Task GetInternalDependency_MissingService()
        {
            var app = WebApplication.Create();

            return Throws(() => app.Services.GetInternalDependency<StubType>())
                .IgnoreStackTrace();
        }

        [Fact]
        public void GetFullName()
        {
            var fullName = GetType().GetMethod(nameof(GetFullName))!.GetFullName();

            Assert.Equal("Reprise.UnitTests.Common.ExtensionMethodsTests.GetFullName", fullName);
        }

        [Theory]
        [InlineData(null, true)]
        [InlineData("", true)]
        [InlineData("   ", true)]
        [InlineData(".", false)]
        [InlineData("test", false)]
        public void IsEmpty(string input, bool expected)
        {
            Assert.Equal(expected, input.IsEmpty());
        }

        [Fact]
        public void TryGetGenericInterfaceType()
        {
            Assert.True(typeof(StubImplementingType).TryGetGenericInterfaceType(typeof(IStubInterface<>), out var interfaceType));
            Assert.Equal(typeof(IStubInterface<object>), interfaceType);
        }

        [Fact]
        public void TryGetGenericInterfaceType_NoImplementation()
        {
            Assert.False(typeof(StubType).TryGetGenericInterfaceType(typeof(IStubInterface<>), out var interfaceType));
            Assert.Null(interfaceType);
        }
    }

    internal class StubType
    {
    }

    internal class StubTypeNoCtor
    {
        private StubTypeNoCtor()
        {
        }
    }

    internal interface IStubInterface<T>
    {
    }

    internal class StubImplementingType : IStubInterface<object>
    {
    }
}
