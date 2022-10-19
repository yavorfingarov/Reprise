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
        public async Task Create() => await Verify(await Client.PostAsJsonAsync("/users", _Users.Generate()));

        [Fact]
        public async Task Get()
        {
            await CreateUsers();

            await Verify(await Client.GetAsync("/users/2"));
        }

        [Fact]
        public async Task Get_NotFound()
        {
            await CreateUsers();

            await Verify(await Client.GetAsync("/users/7"));
        }

        [Fact]
        public async Task GetAll()
        {
            await CreateUsers();

            await Verify(await Client.GetAsync("/users"));
        }

        [Fact]
        public async Task Update()
        {
            await CreateUsers();

            await Verify(await Client.PutAsJsonAsync("/users/2", _Users.Generate()));
        }

        [Fact]
        public async Task Update_NotFound()
        {
            await CreateUsers();

            await Verify(await Client.PutAsJsonAsync("/users/7", _Users.Generate()));
        }

        [Fact]
        public async Task Delete()
        {
            await CreateUsers();

            await Verify(await Client.DeleteAsync("/users/2"));
        }

        [Fact]
        public async Task Delete_NotFound()
        {
            await CreateUsers();

            await Verify(await Client.DeleteAsync("/users/7"));
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
