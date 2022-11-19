namespace Reprise.UnitTests.Features.Endpoints
{
    [UsesVerify]
    public class EndpointAttributes
    {
        [Fact]
        public Task MapAttribute()
        {
            return Verify(new MapAttribute("HEAD", "/users"));
        }

        [Fact]
        public Task MapAttribute_MultipleMethods()
        {
            return Verify(new MapAttribute(new[] { "HEAD", "OPTIONS" }, "/users"));
        }

        [Fact]
        public Task GetAttribute()
        {
            return Verify(new GetAttribute("/users"));
        }

        [Fact]
        public Task PostAttribute()
        {
            return Verify(new PostAttribute("/users"));
        }

        [Fact]
        public Task PutAttribute()
        {
            return Verify(new PutAttribute("/users"));
        }

        [Fact]
        public Task PatchAttribute()
        {
            return Verify(new PatchAttribute("/users"));
        }

        [Fact]
        public Task DeleteAttribute()
        {
            return Verify(new DeleteAttribute("/users"));
        }
    }
}
