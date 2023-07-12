namespace Reprise
{
    public partial class EndpointOptions
    {
        internal string[]? AuthorizationPolicyNames { get; private set; }

        /// <summary>
        /// Enables authorization for all API endpoints.
        /// </summary>
        public void RequireAuthorization(params string[] policyNames)
        {
            AuthorizationPolicyNames = policyNames;
        }
    }
}
