namespace Reprise.SampleApi.Endpoints.Users
{
    [Endpoint]
    public class CreateUserEndpoint
    {
        [Post("/users")]
        public static IResult Handle(UserDto userDto, IValidator<UserDto> validator, DataContext context)
        {
            if (!validator.Validate(userDto).IsValid)
            {
                return Results.BadRequest();
            }
            var user = new User()
            {
                Id = !context.Users.Any() ? 0 : context.Users.Max(u => u.Id) + 1,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName
            };
            context.Users.Add(user);

            return Results.Created($"/users/{user.Id}", user);
        }
    }
}
