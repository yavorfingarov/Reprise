#if NET7_0
using System.Net.Http.Json;

namespace Reprise.UnitTests.Features.Filters
{
    [UsesVerify]
    public class ValidationFilterTests : FilterTestBase
    {
        [Fact]
        public async Task ValidRequest()
        {
            await SetupHost(typeof(StubEndpointWithValidation), options => options.AddValidationFilter());

            await Verify(await Client.PostAsync("/", JsonContent.Create(new StubDto("Hello, world!"))));
        }

        [Fact]
        public async Task InvalidRequest()
        {
            await SetupHost(typeof(StubEndpointWithValidation), options => options.AddValidationFilter());

            await Verify(await Client.PostAsync("/", JsonContent.Create(new StubDto(null!))));
        }

        [Fact]
        public async Task NullableType()
        {
            await SetupHost(typeof(StubEndpointWithValidationNullableType), options => options.AddValidationFilter());

            await Verify(await Client.PostAsync("/", JsonContent.Create(new StubDto("Hello, world!"))));
        }

        [Fact]
        public async Task NullableTypeNull()
        {
            await SetupHost(typeof(StubEndpointWithValidationNullableType), options => options.AddValidationFilter());

            await Verify(await Client.PostAsync("/", null));
        }

        [Fact]
        public async Task NullableTypeInvalidRequest()
        {
            await SetupHost(typeof(StubEndpointWithValidationNullableType), options => options.AddValidationFilter());

            await Verify(await Client.PostAsync("/", JsonContent.Create(new StubDto(null!))));
        }

        [Fact]
        public async Task MultipleTypes()
        {
            await SetupHost(typeof(StubEndpointWithValidationMultipleTypes), options => options.AddValidationFilter());

            await Verify(await Client.PostAsync("/?audience=world", JsonContent.Create(new StubDto("Hello"))));
        }

        [Fact]
        public async Task MultipleTypesInvalidRequest()
        {
            await SetupHost(typeof(StubEndpointWithValidationMultipleTypes), options => options.AddValidationFilter());

            await Verify(await Client.PostAsync("/?audience= ", JsonContent.Create(new StubDto("Hello"))));
        }

        [Fact]
        public async Task NoValidator()
        {
            await SetupHost(typeof(StubEndpointWithoutValidation), options => options.AddValidationFilter());

            await Verify(await Client.PostAsync("/", JsonContent.Create(new StubDtoNoValidator(null!))));
        }
    }
}
#endif
