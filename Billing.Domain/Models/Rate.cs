namespace Billing.Domain.Models;

/// <summary>
/// Represents a rate for a city type.
/// </summary>
public record ExtendedRate(CityType CityType, decimal Amount);