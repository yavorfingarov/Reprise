using System.Diagnostics;

namespace Reprise.UnitTests.Features.ExceptionHandling
{
    [UsesVerify]
    public class ErrorContextTests
    {
        private readonly Mock<HttpContext> _MockHttpContext = new();

        private readonly Exception _Exception = new("Test message");

        [Fact]
        public void Exception()
        {
            var context = new ErrorContext<Exception>(_MockHttpContext.Object, _Exception);

            Assert.Same(_Exception, context.Exception);
        }

        [Fact]
        public Task Request()
        {
            var context = new ErrorContext<Exception>(_MockHttpContext.Object, _Exception);

            _ = context.Request;

            return Verify(_MockHttpContext);
        }

        [Fact]
        public Task TraceIdentifier()
        {
            var context = new ErrorContext<Exception>(_MockHttpContext.Object, _Exception);

            _ = context.TraceIdentifier;

            return Verify(_MockHttpContext);
        }

        [Fact]
        public void ActivityId()
        {
            var context = new ErrorContext<Exception>(_MockHttpContext.Object, _Exception);
            Activity.Current = new Activity("test").Start();

            var activityId = context.ActivityId;

            Assert.Same(Activity.Current.Id, activityId);
        }
    }
}
