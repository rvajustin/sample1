using System.ComponentModel.Design;

namespace Billing.Domain.Common.Abstractions;

/// <summary>
/// Base record for enumerations; used for implemening the Enumeration pattern instead of using enums.
/// </summary>
public abstract record Enumeration(string Key, string Value)
{
    public override string ToString() => Value;
    
    public static bool Equals(Enumeration left, Enumeration right) => left?.Key == right?.Key;

    // Whenever overriding Equals, you should also override GetHashCode.
    public override int GetHashCode() => Key.GetHashCode();
}

