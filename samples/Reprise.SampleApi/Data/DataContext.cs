namespace Reprise.SampleApi.Data
{
    public class ServiceConfigurator : IServiceConfigurator
    {
        public void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<DataContext>();
        }
    }

    public class DataContext
    {
        public List<User> Users { get; } = new();
    }

    public class User
    {
        public int Id { get; init; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;
    }
}
