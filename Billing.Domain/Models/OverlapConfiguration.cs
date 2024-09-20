using Billing.Domain.Common.Abstractions;

namespace Billing.Domain.Models;

/// <summary>
/// Represents the implementation of a configuration for how to handle overlapping billable days.
/// </summary>
public delegate BillableDay OverlapCalculator(BillableDay first, BillableDay second);

/// <summary>
/// Represents a configuration for how to handle overlapping billable days.
/// </summary>
public record OverlapConfiguration : Enumeration
{
    private readonly OverlapCalculator _calculator;

    /// <summary>
    /// Represents the configuration to take the first day when overlapping.
    /// </summary>
    public static OverlapConfiguration TakeFirst = 
        new OverlapConfiguration("FIRST", "Take First Day", TakeFirstCalculation);

    /// <summary>
    /// Represents the configuration to take the last day when overlapping.
    /// </summary>
    public static OverlapConfiguration TakeLast = 
        new OverlapConfiguration("Last", "Take Last Day", TakeLastCalculation);

    /// <summary>
    /// Represents the configuration to take the greater amount when overlapping.
    /// </summary>
    public static OverlapConfiguration TakeGreater =
        new OverlapConfiguration("GREATER", "Take Greater Amount", TakeGreaterCalculation);

    /// <summary>
    /// Represents the configuration to take the lesser amount when overlapping.
    /// </summary>
    public static OverlapConfiguration TakeLesser = 
        new OverlapConfiguration("LESSER", "Take Lesser Amount", TakeLesserCalculation);

    private OverlapConfiguration(string key, string name, OverlapCalculator calculator) : base(key, name)
    {
        _calculator = calculator;
    }

    /// <summary>
    /// Calculates the billable day based on the configuration.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="last"></param>
    /// <returns></returns>
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