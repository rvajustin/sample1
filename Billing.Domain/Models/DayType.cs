using System.Collections.Immutable;
using Billing.Domain.Common.Abstractions;

namespace Billing.Domain.Models;

public record DayType : Enumeration
{
    public static DayType Travel = new DayType(
        "TRAVEL",
        "Travel Day",
        new ExtendedRate(CityType.HiCost, 55),
        new ExtendedRate(CityType.LoCost, 45)
    );

    public static DayType Full = new DayType(
        "FULL",
        "Full Day",
        new ExtendedRate(CityType.HiCost,85),
        new ExtendedRate(CityType.LoCost, 75)
    );


    public readonly ImmutableDictionary<CityType, ExtendedRate> Rates;

    private DayType(string key, string value, params ExtendedRate[] rates) : base(key, value)
    {
        Rates = rates.ToImmutableDictionary(x => x.CityType);
    }
}