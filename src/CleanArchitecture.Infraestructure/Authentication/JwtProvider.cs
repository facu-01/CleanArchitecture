
using CleanArchitecture.Application.Abstractions.Authentication;
using CleanArchitecture.Domain.Users;

using Microsoft.Extensions.Options;

namespace CleanArchitecture.Infraestructure.Authentication;
internal sealed class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _jwtOptions;

    public JwtProvider(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    public Task<string> Generate(User user)
    {

    }
}
