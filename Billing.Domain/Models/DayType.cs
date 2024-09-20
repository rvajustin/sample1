using System.Collections.Immutable;
using Billing.Domain.Common.Abstractions;

namespace Billing.Domain.Models;

/// <summary>
/// Represents a type of day that can be billed.
/// </summary>
public record DayType : Enumeration
{
    public static DayType Travel = new DayType(
        "TRAVEL",
        "Travel Day",
        new Rate(CityType.HiCost, 55),
        new Rate(CityType.LoCost, 45)
    );

    public static DayType Full = new DayType(
        "FULL",
        "Full Day",
        new Rate(CityType.HiCost,85),
        new Rate(CityType.LoCost, 75)
    );


    public readonly ImmutableDictionary<CityType, Rate> Rates;

    private DayType(string key, string value, params Rate[] rates) : base(key, value)
    {
        Rates = rates.ToImmutableDictionary(x => x.CityType);
    }
}