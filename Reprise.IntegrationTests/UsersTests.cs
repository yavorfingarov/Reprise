using System.Net.Http.Json;
using Bogus;
using Reprise.SampleApi.Endpoints.Users;

namespace Reprise.IntegrationTests
{
    public class UsersTests : TestBase
    {
        private readonly Faker<UserDto> _Users;

        public UsersTests()
        {
            Randomizer.Seed = new Random(47151);
            _Users = new Faker<UserDto>()
                .CustomInstantiator(f => new UserDto(f.Name.FirstName(), f.Name.LastName()));
        }

        [Fact]
        public async Task Create()
        {
            await GetToken();

            await Verify(await Client.PostAsJsonAsync("/users", _Users.Generate()))
                .ScrubMember("Authorization");
        }

        [Fact]
        public async Task Get()
        {
            await GetToken();
            await CreateUsers();

            await Verify(await Client.GetAsync("/users/2"))
                .ScrubMember("Authorization");
        }

        [Fact]
        public async Task Get_NotFound()
        {
            await GetToken();
            await CreateUsers();

            await Verify(await Client.GetAsync("/users/7"))
                .ScrubMember("Authorization");
        }

        [Fact]
        public async Task GetAll()
        {
            await GetToken();
            await CreateUsers();

            await Verify(await Client.GetAsync("/users"))
                .ScrubMember("Authorization");
        }

        [Fact]
        public async Task Update()
        {
            await GetToken();
            await CreateUsers();

            await Verify(await Client.PutAsJsonAsync("/users/2", _Users.Generate()))
                .ScrubMember("Authorization");
        }

        [Fact]
        public async Task Update_NotFound()
        {
            await GetToken();
            await CreateUsers();

            await Verify(await Client.PutAsJsonAsync("/users/7", _Users.Generate()))
                .ScrubMember("Authorization");
        }

        [Fact]
        public async Task Delete()
        {
            await GetToken();
            await CreateUsers();

            await Verify(await Client.DeleteAsync("/users/2"))
                .ScrubMember("Authorization");
        }

        [Fact]
        public async Task Delete_NotFound()
        {
            await GetToken();
            await CreateUsers();

            await Verify(await Client.DeleteAsync("/users/7"))
                .ScrubMember("Authorization");
        }

        private async Task GetToken()
        {
            var tokenResponse = await Client.GetFromJsonAsync<TokenResponse>("/token");
            Client.DefaultRequestHeaders.Authorization = new(tokenResponse!.Type, tokenResponse.Token);
        }

        private async Task CreateUsers()
        {
            for (var i = 0; i < 5; i++)
            {
                await Client.PostAsJsonAsync("/users", _Users.Generate());
            }
        }
    }
}
