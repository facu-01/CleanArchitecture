namespace CleanArchitecture.Domain.Abstractions;
internal static class EntityErrors<TEntity> where TEntity : IEntity
{
    internal static Error NotFound() =>
        Error.MakeError<TEntity>(
        nameof(NotFound),
        $"Entidad {typeof(TEntity).Name} no encontrada",
        System.Net.HttpStatusCode.NotFound
    );
}
