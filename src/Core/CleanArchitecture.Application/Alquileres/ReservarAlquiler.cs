using CleanArchitecture.Application.Abstractions.DataAccess;
using CleanArchitecture.Application.Abstractions.Email;
using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Alquileres;
using CleanArchitecture.Domain.Alquileres.Events;
using CleanArchitecture.Domain.Users;
using CleanArchitecture.Domain.Vehiculos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Alquileres;

public static class ReservarAlquiler
{
    public record Command(
        Guid VehiculoId,
        Guid UserId,
        DateOnly FechaInicio,
        DateOnly FechaFin
    ) : ICommand<Guid>;


    internal sealed class Handler : ICommandHandler<Command, Guid>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly PrecioService _precioService;

        public Handler(IApplicationDbContext dbContext, IUnitOfWork unitOfWork, PrecioService precioService)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
            _precioService = precioService;
        }

        public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
        {

            var user = await _dbContext.Users
                            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
            if (user is null)
            {
                return Result.Failure<Guid>(EntityErrors.NotFound<User>(request.UserId));
            }

            var vehiculo = await _dbContext.Vehiculos
                                .FirstOrDefaultAsync(v => v.Id == request.VehiculoId, cancellationToken);
            if (vehiculo is null)
            {
                return Result.Failure<Guid>(EntityErrors.NotFound<Vehiculo>(request.VehiculoId));
            }

            var duracionResult = DateRange.Create(request.FechaInicio, request.FechaFin);

            if (duracionResult.IsFailure)
            {
                return Result.Failure<Guid>(new Error("a", ""));
            }

            var periodo = duracionResult.Value;

            var isOverlaping = await _dbContext.Alquileres.AnyAsync(
                a =>
                a.VehiculoId == request.VehiculoId &&
                a.Periodo.Inicio <= periodo.Fin &&
                a.Periodo.Fin >= periodo.Inicio
            );

            if (isOverlaping)
            {
                return Result.Failure<Guid>(AlquilerErrors.Overlap);
            }

            var alquiler = Alquiler.Reservar(
                vehiculo,
                user.Id,
                periodo,
                DateTime.UtcNow,
                _precioService
            );

            await _dbContext.Alquileres.AddAsync(alquiler, cancellationToken);

            await _unitOfWork.SaveChangesAsync(_dbContext, cancellationToken);

            return Result.Success(alquiler.Id);
        }
    }

    internal sealed class ReservarAlquilerDomainEventHandler : INotificationHandler<AlquilerReservadoDomainEvent>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IEmailService _emailService;

        public ReservarAlquilerDomainEventHandler(IApplicationDbContext dbContext, IEmailService emailService)
        {
            _dbContext = dbContext;
            _emailService = emailService;
        }

        public async Task Handle(AlquilerReservadoDomainEvent notification, CancellationToken cancellationToken)
        {
            var alquiler = await _dbContext.Alquileres
                                .FirstOrDefaultAsync(a => a.Id == notification.Id, cancellationToken);

            if (alquiler is null) return;

            var user = await _dbContext.Users
                                .FirstOrDefaultAsync(u => u.Id == alquiler.UserId, cancellationToken);

            if (user is null) return;


            await _emailService.SendAsync(
                user.Email,
                "Alquiler reservado",
                $"Tienes que confirmar esta reserva: {alquiler.Id}");

        }
    }
}
