using Extensions = Reprise.ApplicationBuilderExtensions;

namespace Reprise.UnitTests
{
    [UsesVerify]
    public class ApplicationBuilderExtensions
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
            return Throws(() => Extensions.UseExceptionHandling(null!))
                .IgnoreStackTrace();
        }
    }
}
