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
    }
}
