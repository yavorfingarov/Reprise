namespace Reprise
{
    /// <summary>
    /// Provides configuration for API endpoints.
    /// </summary>
    public class EndpointOptions
    {
        internal bool _RequireAuthorization;

        internal string[] _AuthorizationPolicyNames = Array.Empty<string>();

        internal EndpointOptions()
        {
        }

        /// <summary>
        /// Enables authorization for all API endpoints.
        /// </summary>
        public void RequireAuthorization(params string[] policyNames)
        {
            _RequireAuthorization = true;
            _AuthorizationPolicyNames = policyNames;
        }
    }
}
