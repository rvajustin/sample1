namespace Billing.Domain.Models;

/// <summary>
/// Represents a day that can be billed.
/// </summary>
public record BillableDay(Project Project, DateOnly Date, DayType Type);