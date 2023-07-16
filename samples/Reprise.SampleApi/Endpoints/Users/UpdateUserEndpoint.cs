namespace Reprise.SampleApi.Endpoints.Users
{
    [Endpoint]
    public class UpdateUserEndpoint
    {
        [Put("/users/{id}")]
        [Produces(StatusCodes.Status204NoContent)]
        [Produces(StatusCodes.Status404NotFound)]
        [ProducesBadRequest]
        public static IResult Handle(
            int id,
            UserDto userDto,
#if NET6_0
            IValidator<UserDto> validator,
#endif
            IMapper<User, UserDto> mapper,
            DataContext context)
        {
#if NET6_0
            validator.ValidateAndThrow(userDto);
#endif
            var user = context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return Results.NotFound();
            }
            mapper.Map(userDto, user);

            return Results.NoContent();
        }
    }
}
