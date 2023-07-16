namespace Reprise.SampleApi.Endpoints.Users
{
    [Endpoint]
    public class CreateUserEndpoint
    {
        [Post("/users")]
        [Produces(StatusCodes.Status201Created)]
        [ProducesBadRequest]
        public static IResult Handle(
            UserDto userDto,
#if NET6_0
            IValidator<UserDto> validator,
#endif
            IMapper<User, UserDto> mapper,
            DataContext context,
            LinkGenerator linker)
        {
#if NET6_0
            validator.ValidateAndThrow(userDto);
#endif
            var user = new User()
            {
                Id = !context.Users.Any() ? 0 : context.Users.Max(u => u.Id) + 1
            };
            mapper.Map(userDto, user);
            context.Users.Add(user);

            return Results.Created(linker.GetPathByName(GetUserEndpoint.Id, values: new { user.Id })!, user);
        }
    }
}
