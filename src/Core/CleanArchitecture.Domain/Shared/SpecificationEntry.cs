using System;

namespace CleanArchitecture.Domain.Shared;

public record SpecificationEntry
{
    public string? Sort { get; set; }

    public int PageIndex { get; set; } = 1;

    private const int MaxPageSize = 50;
    private int _pageSize = 10;
    public int PageSize
    {
        get => _pageSize;
        set
        {
            if (value > MaxPageSize) _pageSize = MaxPageSize;
            if (value < 1) _pageSize = 1;
            _pageSize = value;
        }
    }

    public string? Search { get; set; }

}
