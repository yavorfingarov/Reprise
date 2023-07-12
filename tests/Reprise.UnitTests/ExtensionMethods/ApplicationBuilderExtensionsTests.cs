namespace Reprise.UnitTests.ExtensionMethods
{
    [UsesVerify]
    public class ApplicationBuilderExtensionsTests
    {
        [Fact]
        public Task UseExceptionHandling()
        {
            var mockApplicationBuilder = new Mock<IApplicationBuilder>();

            mockApplicationBuilder.Object.UseExceptionHandling();

            return Verify(mockApplicationBuilder);
        }

        [Fact]
        public Task UseExceptionHandling_AppNull()
        {
            return Throws(() => ApplicationBuilderExtensions.UseExceptionHandling(null!))
                .IgnoreStackTrace();
        }
    }
}
