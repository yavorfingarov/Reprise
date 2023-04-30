namespace Reprise.SampleApi.Endpoints.Users
{
    [Endpoint]
    public class GetAllUsersEndpoint
    {
        [Get("/users")]
        [Produces(StatusCodes.Status200OK, typeof(IEnumerable<User>))]
        public static IEnumerable<User> Handle(DataContext context)
        {
            return context.Users;
        }
    }
}
