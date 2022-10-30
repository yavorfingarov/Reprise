using System.Reflection;

namespace Reprise.UnitTests
{
    [UsesVerify]
    public class Reflection
    {
        [Fact]
        public Task PublicTypes() => Verify(typeof(EndpointAttribute).Assembly.GetExportedTypes().Select(t => t.FullName));
    }
}
