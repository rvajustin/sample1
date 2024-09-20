namespace Billing.Domain.Models;

public record BillableDay(Project Project, DateOnly Date, DayType Type);