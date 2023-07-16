using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Reprise.SampleApi.Security
{
    public class ServiceConfigurator : IServiceConfigurator
    {
        public void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication("Jwt")
                .AddJwtBearer("Jwt", options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(builder.Configuration["Jwt:Key"]!))
                    };
                });

            builder.Services.AddAuthorization();

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
            var signingKey = Convert.FromBase64String(_JwtConfiguration.Key);
            var securityKey = new SymmetricSecurityKey(signingKey);
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(_JwtConfiguration.Issuer, _JwtConfiguration.Audience,
                null, DateTime.UtcNow, DateTime.UtcNow.AddDays(1), signingCredentials);
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
