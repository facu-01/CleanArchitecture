
using CleanArchitecture.Application.Abstractions.Authentication;
using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Users;

using Microsoft.AspNetCore.Identity;


namespace CleanArchitecture.Application.Users;
public static class LoginFeature
{

    public sealed record Command(
        string Email,
        string Password) : ICommand<string>;


    internal sealed class Handler : ICommandHandler<Command, string>
    {

        private readonly IUserRepository _userRepository;
        private readonly IJwtProvider _jwtProvider;


        public Handler(IUserRepository userRepository, IJwtProvider jwtProvider)
        {
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;

        }

        public async Task<Result<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var userEmail = new Email(request.Email);

            var user = await _userRepository.GetByEmailAsync(userEmail, cancellationToken);

            if (user is null)
            {
                return Result.Failure<string>(UserErrors.InvalidCredentials());
            }

            var passworHasher = new PasswordHasher<User>();

            var verificationResult = passworHasher.VerifyHashedPassword(
                user,
                user.PasswordHash.Value,
                request.Password);

            if (verificationResult == PasswordVerificationResult.Failed)
            {
                return Result.Failure<string>(UserErrors.InvalidCredentials());
            }

            var jwt = await _jwtProvider.Generate(user);

            return Result.Success(jwt);

        }
    }


}
