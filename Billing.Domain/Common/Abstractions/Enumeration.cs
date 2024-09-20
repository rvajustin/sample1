using System.ComponentModel.Design;

namespace Billing.Domain.Common.Abstractions;

public record Enumeration(string Key, string Value)
{
    public override string ToString() => Value;
    
    public static bool Equals(Enumeration left, Enumeration right) => left?.Key == right?.Key;

    public override int GetHashCode() => Key.GetHashCode();
}

