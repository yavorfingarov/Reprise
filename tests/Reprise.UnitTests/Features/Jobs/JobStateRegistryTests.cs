namespace Reprise.UnitTests.Features.Jobs
{
    public class JobStateRegistryTests
    {
        [Fact]
        public void Add()
        {
            var jobState = new JobState(GetType(), default, default, default);
            var jobStateRegistry = new JobStateRegistry();

            Assert.Empty(jobStateRegistry);

            jobStateRegistry.Add(jobState);

            Assert.Same(jobState, jobStateRegistry.Single());
        }
    }
}
