namespace Reprise.UnitTests
{
    [UsesVerify]
    public class AttributeTests
    {
        [Fact]
        public Task MapAttribute_WithSingleMethod() => Verify(new MapAttribute("HEAD", "/users"));

        [Fact]
        public Task MapAttribute_WithMultipleMethods() => Verify(new MapAttribute(new[] { "HEAD", "OPTIONS" }, "/users"));

        [Fact]
        public Task GetAttribute() => Verify(new GetAttribute("/users"));

        [Fact]
        public Task PostAttribute() => Verify(new PostAttribute("/users"));

        [Fact]
        public Task PutAttribute() => Verify(new PutAttribute("/users"));

        [Fact]
        public Task PatchAttribute() => Verify(new PatchAttribute("/users"));

        [Fact]
        public Task DeleteAttribute() => Verify(new DeleteAttribute("/users"));
    }
}
