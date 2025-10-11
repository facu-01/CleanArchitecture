
namespace CleanArchitecture.Domain.Abstractions;

public static class EntityErrors
{

    public static Error NotFound<TEntity>(Guid Id) where TEntity : Entity =>
    Error.MakeError<TEntity>(
        nameof(NotFound),
        $"No existe la entidad para el id {Id}"
    );

}
