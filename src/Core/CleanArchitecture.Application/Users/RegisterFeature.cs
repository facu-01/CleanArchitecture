using System;

using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Users;

using FluentValidation;

using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Application.Users;

public static class RegisterFeature
{

    public record Command(
    string Nombre,
    string Apellido,
    string Email,
    string Password
    ) : ICommand;

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(c => c.Nombre).NotEmpty();
            RuleFor(c => c.Apellido).NotEmpty();
            RuleFor(c => c.Email).EmailAddress().NotEmpty();
            RuleFor(c => c.Password).MinimumLength(3).MaximumLength(50).NotEmpty();
        }
    }

    internal sealed class Handler : ICommandHandler<Command>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var userEmail = new Email(request.Email);

            var exists = await _userRepository.UserExistsByEmailAsync(userEmail, cancellationToken);

            if (exists)
            {
                return Result.Failure(UserErrors.EmailYaEnUso);
            }

            var passwordHasher = new PasswordHasher<object>();

            var passwordHash = passwordHasher.HashPassword(null!, request.Password);

            var newUser = User.Registrar(
                new Nombre(request.Nombre),
                new Apellido(request.Apellido),
                userEmail,
                new PasswordHash(passwordHash)
            );

            await _userRepository.AddAsync(newUser, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }



}
