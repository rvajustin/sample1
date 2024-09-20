using Billing.Domain.Common.Abstractions;

namespace Billing.Domain.Models;

public delegate BillableDay OverlapCalculator(BillableDay first, BillableDay second);

public record OverlapConfiguration : Enumeration
{
    private readonly OverlapCalculator _calculator;

    public static OverlapConfiguration TakeFirst = 
        new OverlapConfiguration("FIRST", "Take First Day", TakeFirstCalculation);

    public static OverlapConfiguration TakeLast = 
        new OverlapConfiguration("Last", "Take Last Day", TakeLastCalculation);

    public static OverlapConfiguration TakeGreater =
        new OverlapConfiguration("GREATER", "Take Greater Cost", TakeGreaterCalculation);

    public static OverlapConfiguration TakeLesser = 
        new OverlapConfiguration("LESSER", "Take Lesser Cost", TakeLesserCalculation);

    private OverlapConfiguration(string key, string name, OverlapCalculator calculator) : base(key, name)
    {
        _calculator = calculator;
    }

    public BillableDay Calculate(BillableDay first, BillableDay last) => _calculator(first, last);

    private static BillableDay TakeFirstCalculation(BillableDay first, BillableDay _) => first;

    private static BillableDay TakeLastCalculation(BillableDay _, BillableDay last) => last;

    private static BillableDay[] ArrayOf(params BillableDay[] billableDays) 
        => billableDays.OrderBy(d => d.Date).ToArray();

    private static BillableDay TakeGreaterCalculation(BillableDay first, BillableDay last)
        => ArrayOf(first, last).MaxBy(x => x.Type.Rates[x.Project.CityType].Amount) ??
           throw new InvalidOperationException();

    private static BillableDay TakeLesserCalculation(BillableDay first, BillableDay last)
        => ArrayOf(first, last).MinBy(x => x.Type.Rates[x.Project.CityType].Amount) ??
           throw new InvalidOperationException();

}