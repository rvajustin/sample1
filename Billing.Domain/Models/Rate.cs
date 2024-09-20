namespace Billing.Domain.Models;

/// <summary>
/// Represents a rate for a city type.
/// </summary>
public record Rate(CityType CityType, decimal Amount);