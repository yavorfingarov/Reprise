namespace Reprise.SampleApi.Endpoints.Users
{
    [Endpoint]
    public class DeleteUserEndpoint
    {
        [Delete("/users/{id}")]
        [Produces(StatusCodes.Status204NoContent)]
        [Produces(StatusCodes.Status404NotFound)]
        public static IResult Handle(int id, DataContext context)
        {
            var index = context.Users.FindIndex(x => x.Id == id);
            if (index == -1)
            {
                return Results.NotFound();
            }
            context.Users.RemoveAt(index);

            return Results.NoContent();
        }
    }
}
