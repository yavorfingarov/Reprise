namespace Reprise.SampleApi.Endpoints.Users
{
    [Endpoint]
    public class UpdateUserEndpoint
    {
        [Put("/users/{id}")]
        public static IResult Handle(int id, UserDto userDto, IValidator<UserDto> validator, DataContext context)
        {
            if (!validator.Validate(userDto).IsValid)
            {
                return Results.BadRequest();
            }
            var user = context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return Results.NotFound();
            }
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;

            return Results.NoContent();
        }
    }
}
