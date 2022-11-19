namespace Reprise.UnitTests
{
    [UsesVerify]
    public class Reflection
    {
        [Fact]
        public Task PublicTypes()
        {
            var publicTypes = typeof(EndpointAttribute).Assembly
                .GetExportedTypes()
                .Select(t => t.FullName);

            return Verify(publicTypes);
        }
    }
}
