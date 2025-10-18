namespace CleanArchitecture.Domain.Abstractions;
public abstract class EntityErrors<TEntity> where TEntity : IEntity
{
    public static Error MakeError(string errorCode, string message)
    {
        return new Error(
            errorCode,
            $"{typeof(TEntity).Name}",
            message
        );
    }
    public static Error NotFound(object identity) =>
        MakeError(
        nameof(NotFound),
        $"Entidad {typeof(TEntity).Name} no encontrada con {identity}"
    );
}
