namespace Reprise.SampleApi.Endpoints.Users
{
    [Endpoint]
    public class GetUserEndpoint
    {
        public const string Id = "GetUser";

        [Get("/users/{id}")]
        [Produces(StatusCodes.Status200OK, typeof(User))]
        [Produces(StatusCodes.Status404NotFound)]
        [Name(Id)]
        public static IResult Handle(int id, DataContext context)
        {
            var user = context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(user);
        }
    }
}
