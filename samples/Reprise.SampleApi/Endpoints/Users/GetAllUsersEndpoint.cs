namespace Reprise.SampleApi.Endpoints.Users
{
    [Endpoint]
    public class GetAllUsersEndpoint
    {
        [Get("/users")]
        [Produces(StatusCodes.Status200OK, typeof(IEnumerable<User>))]
        public static IEnumerable<UserDto> Handle(DataContext context, IMapper<User, UserDto> mapper)
        {
            return context.Users.Select(mapper.Map);
        }
    }
}
