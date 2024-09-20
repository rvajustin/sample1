namespace Billing.Domain.Common.Abstractions;

public record DateRange(DateOnly Start, DateOnly End) 
{
    public DateRange(string start, string end) : this(DateOnly.Parse(start), DateOnly.Parse(end)) { }

    public virtual bool Equals(DateRange? other) => other?.Start == Start && other?.End == End;

    public override int GetHashCode() => $"{Start}-{End}".GetHashCode();
    
    public bool IsIntersectionOrAdjacent(DateRange other, out DateOnly[] dates) 
        => IsIntersecting(other, out dates) || IsAdjacent(other, out dates);

    public bool IsIntersecting(DateRange other, out DateOnly[] dates)
    {
        if (Start == other.End)
        {
            dates = [Start];
            return true;
        }

        if (End == other.Start)
        {
            dates = [End];
            return true;
        }

        dates = [];
        return false;
    }

    public bool IsAdjacent(DateRange other, out DateOnly[] dates)
    {
        if (other.End.AddDays(1) == Start)
        {
            dates = [other.End, Start];
            return true;
        }
        
        if (other.Start.AddDays(-1) == End)
        {
            dates = [other.Start, End];
            return true;
        }

        dates = [];
        return other?.End.AddDays(1) == Start || other?.Start.AddDays(-1) == End;
    }

    public DateOnly[] InterstitialDays()
    {
        if (End <= Start.AddDays(1))
        {
            return [];  // Return an empty array if no middle days exist
        }

        var numberOfDays = End.DayNumber - Start.DayNumber - 1;
        var days = new DateOnly[numberOfDays];
    
        for (var i = 0; i < numberOfDays; i++)
        {
            days[i] = Start.AddDays(i + 1);
        }

        return days;
    }
}