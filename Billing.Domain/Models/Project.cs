using System.Security.Cryptography.X509Certificates;
using Billing.Domain.Common.Abstractions;

namespace Billing.Domain.Models;

/// <summary>
/// Represents a project that can be billed.
/// </summary>
public record Project
{
    public Guid Id { get; }
    public string Name { get; }
    public CityType CityType { get; }
    public DateRange Dates { get; }

    public Project(string name, CityType cityType, DateRange dates) : this(Guid.NewGuid(), name, cityType, dates)
    {
    }

    public Project(Guid id, string name, CityType cityType, DateRange dates)
    {
        Name = name;
        CityType = cityType;
        Dates = dates;
        Id = id;
    }

    public virtual bool Equals(Project? other) => other?.Id == Id;

    // Whenever overriding Equals, you should also override GetHashCode.
    public override int GetHashCode() => Id.GetHashCode();

    public void Deconstruct(out Guid id, out string name, out CityType cityType, out DateRange dates)
    {
        id = Id;
        name = Name;
        cityType = CityType;
        dates = Dates;
    }
}