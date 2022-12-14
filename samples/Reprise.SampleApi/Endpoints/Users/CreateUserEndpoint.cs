namespace Reprise.SampleApi.Endpoints.Users
{
    [Endpoint]
    public class CreateUserEndpoint
    {
        [Post("/users")]
        [Produces(StatusCodes.Status201Created)]
        [ProducesBadRequest]
        public static IResult Handle(UserDto userDto, DataContext context, LinkGenerator linker)
        {
            var user = new User()
            {
                Id = !context.Users.Any() ? 0 : context.Users.Max(u => u.Id) + 1,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName
            };
            context.Users.Add(user);

            return Results.Created(linker.GetPathByName(GetUserEndpoint.Id, values: new { user.Id })!, user);
        }
    }
}
