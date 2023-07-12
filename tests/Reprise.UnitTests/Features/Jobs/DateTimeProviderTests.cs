namespace Reprise.UnitTests.Features.Jobs
{
    public class DateTimeProviderTests
    {
        [Fact]
        public void UtcNow()
        {
            var dateTimeProvider = new DateTimeProvider();

            Assert.Equal(DateTime.UtcNow, dateTimeProvider.UtcNow, precision: TimeSpan.FromMilliseconds(50));
        }
    }
}
