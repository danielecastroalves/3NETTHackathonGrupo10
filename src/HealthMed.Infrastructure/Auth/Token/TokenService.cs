using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HealthMed.Application.Common.Auth.Token;
using HealthMed.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HealthMed.Infrastructure.Auth.Token;

public class TokenService(IConfiguration configuration) : ITokenService
{
    private readonly IConfiguration _configuration = configuration;

    public string GetUserToken(User usuario)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(_configuration.GetSection("Autenticacao").GetValue<string>("Secret")!);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                    new(ClaimTypes.Name, usuario.Login),
                    new(ClaimTypes.Role, usuario.Profiles.ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
