using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Infraestructure.Outbox;
public static class OutboxMessageContentJsonSerializer
{
    // Build the derived types once
    private static readonly List<JsonDerivedType> _derivedTypes = BuildDerivedTypes();

    private static List<JsonDerivedType> BuildDerivedTypes()
    {
        var eventInterface = typeof(IDomainEvent);

        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(asm =>
            {
                try
                {
                    return asm.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    return ex.Types.Where(t => t != null)!;
                }
                catch
                {
                    return Array.Empty<Type>();
                }
            })
            .Where(t => t is not null
                        && eventInterface.IsAssignableFrom(t)
                        && t != eventInterface
                        && !t.IsAbstract
                        && t.IsPublic)
            .Distinct()
            .Select(t => new JsonDerivedType(t, t.AssemblyQualifiedName))
            .ToList();

        return types;
    }

    public static JsonSerializerOptions Options = new JsonSerializerOptions
    {
        AllowOutOfOrderMetadataProperties = true,
        TypeInfoResolver = new DefaultJsonTypeInfoResolver
        {
            Modifiers =
            {
                ti =>
                {
                    if (ti.Type == typeof(IDomainEvent))
                    {
                        var poly = new JsonPolymorphismOptions
                        {
                            TypeDiscriminatorPropertyName = "$type",
                            IgnoreUnrecognizedTypeDiscriminators = false
                        };

                        foreach (var derived in _derivedTypes)
                        {
                            poly.DerivedTypes.Add(derived);
                        }

                        ti.PolymorphismOptions = poly;
                    }
                }
            }
        }
    };
}