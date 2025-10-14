
namespace CleanArchitecture.Domain.Abstractions;

public static class EntityErrors
{

    public static Error NotFound<TEntity,TEntityId>(TEntityId id) 
    where TEntity : Entity<TEntityId>
     =>
    Error.MakeError<TEntity>(
        nameof(NotFound),
        $"No existe la entidad para el id {id}",
        System.Net.HttpStatusCode.NotFound
    );

}
