#if NET7_0
namespace Reprise
{
    public partial class EndpointOptions
    {
        internal string? RateLimitingPolicy { get; private set; }

        /// <summary>
        /// Enables rate limiting for all API endpoints.
        /// </summary>
        public void RequireRateLimiting(string policyName)
        {
            RateLimitingPolicy = policyName;
        }
    }
}
#endif
