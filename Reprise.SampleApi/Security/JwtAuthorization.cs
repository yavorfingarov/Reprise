using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Reprise.SampleApi.Security
{
    public class JwtAuthorizationConfigurator : IServiceConfigurator
    {
        private const string _AuthenticationScheme = "Jwt";

        public void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication()
                .AddJwtBearer(_AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                    };
                });

            builder.Services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(_AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
            });

            builder.Services.AddSingleton<IJwtGenerator, JwtGenerator>();
        }
    }

    public interface IJwtGenerator
    {
        (string Type, string Token) Generate();
    }

    public class JwtGenerator : IJwtGenerator
    {
        private readonly JwtConfiguration _JwtConfiguration;

        public JwtGenerator(JwtConfiguration jwtConfiguration)
        {
            _JwtConfiguration = jwtConfiguration;
        }

        public (string Type, string Token) Generate()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_JwtConfiguration.Key));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(_JwtConfiguration.Issuer, _JwtConfiguration.Audience,
                null, DateTime.UtcNow, DateTime.UtcNow.AddHours(1), signingCredentials);
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.WriteToken(tokenDescriptor);

            return ("Bearer", token);
        }
    }

    [Configuration("Jwt")]
    public class JwtConfiguration
    {
        public string Issuer { get; set; } = null!;

        public string Audience { get; set; } = null!;

        public string Key { get; set; } = null!;
    }
}
