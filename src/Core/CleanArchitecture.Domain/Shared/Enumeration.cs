namespace CleanArchitecture.Domain.Shared;
public abstract class Enumeration<TEnum> : IEquatable<Enumeration<TEnum>>
    where TEnum : Enumeration<TEnum>
{

    private static readonly Dictionary<int, TEnum> Enumerations = CreateEnumerations();

    public int Id { get; protected init; }
    public string Name { get; protected init; }

    public Enumeration(int id, string name)
    {
        Id = id;
        Name = name;
    }

    private static Dictionary<int, TEnum> CreateEnumerations()
    {
        var enumerationType = typeof(TEnum);
        var fields = enumerationType.GetFields(
            System.Reflection.BindingFlags.Public |
            System.Reflection.BindingFlags.Static |
            System.Reflection.BindingFlags.FlattenHierarchy)
            .Where(fi => enumerationType.IsAssignableFrom(fi.FieldType))
            .Select(fi => (TEnum)fi.GetValue(default)!);

        return fields.ToDictionary(e => e.Id);
    }

    public static IReadOnlyCollection<TEnum> GetValues() => Enumerations.Values;

    public static TEnum? FromValue(int id)
    {
        return Enumerations.TryGetValue(id, out var enumeration) ? enumeration : null;
    }

    public static TEnum? FromName(string name)
    {
        return Enumerations.Values.FirstOrDefault(e => e.Name == name);
    }

    public bool Equals(Enumeration<TEnum>? other)
    {
        if (other is null) return false;
        return GetType() == other.GetType() && Id == other.Id;
    }

    public override bool Equals(object? obj)
    {
        return obj is Enumeration<TEnum> other && Equals(other);
    }

    public override int GetHashCode() => Id.GetHashCode();

    public override string ToString() => Name;

}
