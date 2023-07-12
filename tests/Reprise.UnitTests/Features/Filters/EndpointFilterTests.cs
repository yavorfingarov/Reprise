#if NET7_0
namespace Reprise.UnitTests.Features.Filters
{
    public class EndpointFilterTests : FilterTestBase
    {
        [Fact]
        public async Task EndpointFilter()
        {
            await SetupHost(typeof(StubEndpointWithFilter));

            await Verify(await Client.GetAsync("/"));
        }

        [Fact]
        public async Task EndpointFilter_NoImplementation()
        {
            await ThrowsTask(() => SetupHost(typeof(StubEndpointWithFilterNoImplementation)))
                .IgnoreStackTrace();
        }

        [Fact]
        public async Task GlobalEndpointFilters()
        {
            await SetupHost(typeof(StubEndpointWithoutFilter), options =>
            {
                options.AddEndpointFilter<StubFilterA>();
                options.AddEndpointFilter<StubFilterB>();
                options.AddEndpointFilter<StubFilterC>();
            });

            await Verify(await Client.GetAsync("/"));
        }

        [Fact]
        public async Task GlobalEndpointFilters_EndpointFilter()
        {
            await SetupHost(typeof(StubEndpointWithFilter), options =>
            {
                options.AddEndpointFilter<StubFilterB>();
                options.AddEndpointFilter<StubFilterC>();
            });

            await Verify(await Client.GetAsync("/"));
        }

        [Fact]
        public async Task GlobalEndpointFilters_EndpointMultipleFilters()
        {
            await SetupHost(typeof(StubEndpointWithMultipleFilters), options =>
            {
                options.AddEndpointFilter<StubFilterB>(50);
                options.AddEndpointFilter<StubFilterC>(10);
            });

            await Verify(await Client.GetAsync("/"));
        }
    }
}
#endif
