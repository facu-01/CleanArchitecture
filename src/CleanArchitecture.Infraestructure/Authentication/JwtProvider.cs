
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using CleanArchitecture.Application.Abstractions.Authentication;
using CleanArchitecture.Domain.Users;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchitecture.Infraestructure.Authentication;
public sealed class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _jwtOptions;

    public JwtProvider(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    public Task<string> Generate(User user)
    {
        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Sub,user.Id.Value.ToString()),
            new (JwtRegisteredClaimNames.Email,user.Email.Value),
        };

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            claims,
            null,
            DateTime.UtcNow.AddHours(5),
            signingCredentials
        );

        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return Task.FromResult(tokenValue);

    }
}
