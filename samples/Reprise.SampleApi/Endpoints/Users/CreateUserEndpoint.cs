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
            DataContext context,
            LinkGenerator linker)
        {
#if NET6_0
            validator.ValidateAndThrow(userDto);
#endif
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
