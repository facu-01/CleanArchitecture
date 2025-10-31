using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Shared;
using CleanArchitecture.Domain.Vehiculos;

namespace CleanArchitecture.Application.Vehiculos;
public static class GetVehiculosPaginated
{

    public enum VehiculoOrderBy
    {
        IdDesc,
        IdAsc,
        ModeloAsc,
        ModeloDesc,
        PrecioAsc,
        PrecioDesc,
        FechaUltimoAlquilerAsc,
        FechaUltimoAlquilerDesc
    }

    public sealed record Query : SpecificationEntry, IQuery<PaginationResult<Vehiculo, VehiculoId>>
    {
        public VehiculoOrderBy Sort { get; set; } = VehiculoOrderBy.IdAsc;
        public string? ModeloFilter { get; set; }
        public string? PaisFilter { get; set; }

    }


    private sealed class Spec : BaseSpecification<Vehiculo, VehiculoId>
    {

        public Spec(Query query)
        {
            // pagination
            var skip = (query.PageIndex - 1) * query.PageSize;
            var take = query.PageSize;
            ApplyPagin(skip, take);

            // sorting
            switch (query.Sort)
            {
                case VehiculoOrderBy.IdAsc:
                    AddOrderBy(x => x.Id);
                    break;
                case VehiculoOrderBy.IdDesc:
                    AddOrderByDesc(x => x.Id);
                    break;
                case VehiculoOrderBy.ModeloAsc:
                    AddOrderBy(x => x.Modelo.Value);
                    break;
                case VehiculoOrderBy.ModeloDesc:
                    AddOrderByDesc(x => x.Modelo.Value);
                    break;
                case VehiculoOrderBy.PrecioAsc:
                    AddOrderBy(x => x.Precio.Monto);
                    break;
                case VehiculoOrderBy.PrecioDesc:
                    AddOrderByDesc(x => x.Precio.Monto);
                    break;
                case VehiculoOrderBy.FechaUltimoAlquilerAsc:
                    AddOrderBy(x => x.FechaUltimoAlquiler);
                    break;
                case VehiculoOrderBy.FechaUltimoAlquilerDesc:
                    AddOrderByDesc(x => x.FechaUltimoAlquiler);
                    break;
                default:
                    AddOrderBy(x => x.Id);
                    break;
            }

            if (!string.IsNullOrEmpty(query.ModeloFilter))
            {
                AddCriteria(x => x.Modelo.Value.ToLower().Contains(query.ModeloFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(query.PaisFilter))
            {
                AddCriteria(x => x.Direccion.Pais.Contains(query.PaisFilter));
            }

        }

    }

    internal sealed class Handler : IQueryHandler<Query, PaginationResult<Vehiculo, VehiculoId>>
    {
        private readonly IVehiculoRepository _vehiculoRepository;

        public Handler(IVehiculoRepository vehiculoRepository)
        {
            _vehiculoRepository = vehiculoRepository;
        }

        public async Task<Result<PaginationResult<Vehiculo, VehiculoId>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var spec = new Spec(request);

            var count = await _vehiculoRepository.CountAsync(spec, cancellationToken);

            var vehiculos = await _vehiculoRepository.GetAllWithSpec(spec, cancellationToken);

            var result = new PaginationResult<Vehiculo, VehiculoId>
            {
                Count = count,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Data = vehiculos,
                PageCount = (int)Math.Ceiling((double)count / request.PageSize),
                ResultsByPage = vehiculos.Count
            };


            return Result.Success(result);

        }
    }

}
