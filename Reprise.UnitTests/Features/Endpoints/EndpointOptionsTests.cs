namespace Reprise.UnitTests.Features.Endpoints
{
    [UsesVerify]
    public class EndpointOptionsTests
    {
        [Fact]
        public Task DefaultValues()
        {
            var options = new EndpointOptions();

            return Verify(options.GetSnapshot());
        }

        [Fact]
        public Task RequireAuthorization()
        {
            var options = new EndpointOptions();

            options.RequireAuthorization();

            return Verify(options.GetSnapshot());
        }

        [Fact]
        public Task RequireAuthorization_PolicyName()
        {
            var options = new EndpointOptions();

            options.RequireAuthorization("Policy1");

            return Verify(options.GetSnapshot());
        }

        [Fact]
        public Task RequireAuthorization_MultiplePolicyNames()
        {
            var options = new EndpointOptions();

            options.RequireAuthorization("Policy1", "Policy2");

            return Verify(options.GetSnapshot());
        }
    }

    internal static class EndpointOptionsExtensions
    {
        internal static object GetSnapshot(this EndpointOptions options)
        {
            return new
            {
                options._RequireAuthorization,
                options._AuthorizationPolicyNames
            };
        }
    }
}
