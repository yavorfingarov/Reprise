namespace Reprise.SampleApi.Endpoints.Users
{
    [Endpoint]
    public class GetUserEndpoint
    {
        [Get("/users/{id}")]
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
