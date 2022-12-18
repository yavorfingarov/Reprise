namespace Reprise.UnitTests.Common
{
    [UsesVerify]
    public class ExtensionMethods
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
        public void GetRequiredServiceSafe()
        {
            var builder = WebApplication.CreateBuilder();
            builder.Services.AddSingleton<StubType>();
            var app = builder.Build();

            Assert.IsType<StubType>(app.Services.GetRequiredServiceSafe<StubType>());
        }

        [Fact]
        public Task GetRequiredServiceSafe_MissingService()
        {
            var app = WebApplication.Create();

            return Throws(() => app.Services.GetRequiredServiceSafe<StubType>())
                .IgnoreStackTrace();
        }

        [Fact]
        public void GetFullName()
        {
            var fullName = GetType().GetMethod(nameof(ExtensionMethods.GetFullName))!.GetFullName();

            Assert.Equal("Reprise.UnitTests.Common.ExtensionMethods.GetFullName", fullName);
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
}
