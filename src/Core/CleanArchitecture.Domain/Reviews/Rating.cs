using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Reviews;

public record Rating
{
    private Rating() { }

    public static Error InvalidRating(int value) =>
        new(
        nameof(InvalidRating),
        nameof(Rating.InvalidRating),
        $"El valor {value} no es valido como rating"
    );
    public int Value { get; init; }

    private Rating(int rating) => Value = rating;


    public static Result<Rating> Create(int value)
    {
        if (1 > value || value > 5)
        {
            return Result.Failure<Rating>(InvalidRating(value));
        }
        return Result.Success(new Rating(value));
    }

}
