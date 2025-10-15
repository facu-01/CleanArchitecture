namespace CleanArchitecture.Domain.Abstractions;
internal static class EntityErrors<TEntity> where TEntity : IEntity
{
    internal static Error NotFound(object identity) =>
        Error.MakeError<TEntity>(
        nameof(NotFound),
        $"Entidad {typeof(TEntity).Name} no encontrada con {identity}"
    );
}
