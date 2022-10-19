namespace Reprise.SampleApi.Endpoints.Users
{
    [Endpoint]
    public class GetAllUsersEndpoint
    {
        [Get("/users")]
        public static IEnumerable<User> Handle(DataContext context) => context.Users;
    }
}
