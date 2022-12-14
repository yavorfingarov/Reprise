namespace Reprise.SampleApi.Endpoints.Users
{
    [Endpoint]
    public class UpdateUserEndpoint
    {
        [Put("/users/{id}")]
        [Produces(StatusCodes.Status204NoContent)]
        [Produces(StatusCodes.Status404NotFound)]
        [ProducesBadRequest]
        public static IResult Handle(int id, UserDto userDto, DataContext context)
        {
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
