using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Reprise.SampleApi.Security
{
    public class JwtAuthorizationConfigurator : IServiceConfigurator
    {
        public void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication().AddJwtBearer();

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
            var signingKey = Convert.FromBase64String(_JwtConfiguration.SigningKeys.Single().Value);
            var securityKey = new SymmetricSecurityKey(signingKey);
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(_JwtConfiguration.ValidIssuer, _JwtConfiguration.ValidAudiences.Single(),
                null, DateTime.UtcNow, DateTime.UtcNow.AddDays(1), signingCredentials);
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.WriteToken(tokenDescriptor);

            return ("Bearer", token);
        }
    }

    [Configuration("Authentication:Schemes:Bearer")]
    public class JwtConfiguration
    {
        public string ValidIssuer { get; set; } = null!;

        public IEnumerable<string> ValidAudiences { get; set; } = Enumerable.Empty<string>();

        public IEnumerable<SigningKey> SigningKeys { get; set; } = null!;

        public class SigningKey
        {
            public string Issuer { get; set; } = null!;

            public string Value { get; set; } = null!;
        }
    }
}
