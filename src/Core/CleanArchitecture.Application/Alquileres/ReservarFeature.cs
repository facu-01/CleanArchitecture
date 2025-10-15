using CleanArchitecture.Application.Abstractions.Email;
using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Application.Exceptions;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Alquileres;
using CleanArchitecture.Domain.Alquileres.Events;
using CleanArchitecture.Domain.Users;
using CleanArchitecture.Domain.Vehiculos;

using FluentValidation;

using MediatR;

namespace CleanArchitecture.Application.Alquileres;

public static class ReservarFeature
{
    public record Command(
        Guid VehiculoId,
        Guid UserId,
        DateOnly FechaInicio,
        DateOnly FechaFin
    ) : ICommand<Guid>;

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(c => c.UserId).NotEmpty();
            RuleFor(c => c.VehiculoId).NotEmpty();
            RuleFor(c => c.FechaInicio).LessThan(c => c.FechaFin);
        }
    }

    internal sealed class Handler : ICommandHandler<Command, Guid>
    {

        private readonly PrecioService _precioService;
        private readonly IAlquilerRepository _alquilerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IVehiculoRepository _vehiculoRepository;
        private readonly IUnitOfWork _unitOfWork;


        private static readonly AlquilerStatus[] _activeAlquilerStatuses =
        [
            AlquilerStatus.Confirmado,
                AlquilerStatus.Reservado,
                AlquilerStatus.Completado
        ];

        public Handler(PrecioService precioService, IAlquilerRepository alquilerRepository, IUserRepository userRepository, IVehiculoRepository vehiculoRepository, IUnitOfWork unitOfWork)
        {
            _precioService = precioService;
            _alquilerRepository = alquilerRepository;
            _userRepository = userRepository;
            _vehiculoRepository = vehiculoRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
        {
            var userId = new UserId(request.UserId);
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

            if (user is null)
            {
                return Result.Failure<Guid>(UserErrors.NotFound(userId));
            }

            var vehiculoId = new VehiculoId(request.VehiculoId);
            var vehiculo = await _vehiculoRepository.GetByIdAsync(vehiculoId, cancellationToken);

            if (vehiculo is null)
            {
                return Result.Failure<Guid>(VehiculoErrors.NotFound(vehiculoId));
            }

            var duracionResult = DateRange.Create(request.FechaInicio, request.FechaFin);

            var periodo = duracionResult.Value;

            var isOverlaping = await _alquilerRepository.IsOverlapping(
                periodo,
                vehiculoId,
                _activeAlquilerStatuses,
                cancellationToken
            );

            if (isOverlaping)
            {
                return Result.Failure<Guid>(AlquilerErrors.Overlap);
            }

            try
            {
                var alquiler = Alquiler.Reservar(
                vehiculo,
                user.Id,
                periodo,
                DateTime.UtcNow,
                _precioService
            );

                await _alquilerRepository.AddAsync(alquiler, cancellationToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result.Success(alquiler.Id.Value);
            }
            catch (ConcurrencyException)
            {

                return Result.Failure<Guid>(AlquilerErrors.Overlap);
            }
        }
    }

    internal sealed class ReservarAlquilerDomainEventHandler : INotificationHandler<AlquilerReservadoDomainEvent>
    {
        private readonly IAlquilerRepository _alquilerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public ReservarAlquilerDomainEventHandler(IAlquilerRepository alquilerRepository, IUserRepository userRepository, IEmailService emailService)
        {
            _alquilerRepository = alquilerRepository;
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public async Task Handle(AlquilerReservadoDomainEvent notification, CancellationToken cancellationToken)
        {
            var alquiler = await _alquilerRepository.GetByIdAsync(notification.Id, cancellationToken);

            if (alquiler is null) return;

            var user = await _userRepository.GetByIdAsync(alquiler.UserId, cancellationToken);

            if (user is null) return;

            await _emailService.SendAsync(
                user.Email,
                "Alquiler reservado",
                $"Tienes que confirmar esta reserva: {alquiler.Id}");

        }
    }
}
