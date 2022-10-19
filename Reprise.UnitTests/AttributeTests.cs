namespace Reprise.UnitTests
{
    [UsesVerify]
    public class AttributeTests
    {
        [Fact]
        public Task MapAttribute_WithSingleMethod()
        {
            var attribute = new MapAttribute("HEAD", "/users");

            return Verify(new { attribute.Methods, attribute.Route });
        }

        [Fact]
        public Task MapAttribute_WithMultipleMethods()
        {
            var attribute = new MapAttribute(new[] { "HEAD", "OPTIONS" }, "/users");

            return Verify(new { attribute.Methods, attribute.Route });
        }

        [Fact]
        public Task GetAttribute()
        {
            var attribute = new GetAttribute("/users");

            return Verify(new { attribute.Methods, attribute.Route });
        }

        [Fact]
        public Task PostAttribute()
        {
            var attribute = new PostAttribute("/users");

            return Verify(new { attribute.Methods, attribute.Route });
        }

        [Fact]
        public Task PutAttribute()
        {
            var attribute = new PutAttribute("/users");

            return Verify(new { attribute.Methods, attribute.Route });
        }

        [Fact]
        public Task PatchAttribute()
        {
            var attribute = new PatchAttribute("/users");

            return Verify(new { attribute.Methods, attribute.Route });
        }

        [Fact]
        public Task DeleteAttribute()
        {
            var attribute = new DeleteAttribute("/users");

            return Verify(new { attribute.Methods, attribute.Route });
        }
    }
}
