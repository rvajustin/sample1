namespace Billing.Domain.Models;

using BillableDays = SortedDictionary<DateOnly, BillableDay>;
using ProjectBills = Dictionary<Project, decimal>;

/// <summary>
/// Represents a set of projects that can be billed.
/// </summary>
public record ProjectSet(params Project[] Projects)
{
    private IOrderedEnumerable<Project> ToSorted() 
        => Projects.OrderBy(d => d.Dates.Start).ThenBy(d => d.Dates.End);

    /// <summary>
    /// Calculates the billable days for the projects in the set.
    /// </summary>
    /// <param name="overlapConfiguration"></param>
    /// <returns></returns>
    public ProjectBills Calculate(OverlapConfiguration? overlapConfiguration)
    {
        var projects = ToSorted().ToArray();
        var billableDays = GetBillableDays(projects, overlapConfiguration);
        billableDays = ReviseTravelDaysToFullDays(billableDays);
        var projectBills = CalculateProjectBills(billableDays);
        
        return projectBills;
    }

    /// <summary>
    /// Calculates the billable days for the projects in the set.
    /// </summary>
    /// <param name="billableDays"></param>
    /// <returns></returns>
    private static ProjectBills CalculateProjectBills(BillableDays billableDays)
    {
        var projectBills = new ProjectBills();

        foreach (var (_, billableDay) in billableDays)
        {
            projectBills.TryAdd(billableDay.Project, 0); // initialize the project bill to 0 if it doesn't exist
            var rate = billableDay.Type.Rates[billableDay.Project.CityType]; // get the rate for the project's city type
            projectBills[billableDay.Project] = projectBills[billableDay.Project] + rate.Amount; // add the rate to the project bill
            // Console.WriteLine("Project: {0}, Date: {1}, Type: {2}, Rate: {3:C}", billableDay.Project.Name, billableDay.Date, billableDay.Type.Value, rate.Amount);
        }

        return projectBills;
    }

    /// <summary>
    /// Gets the billable days for the projects in the set.
    /// </summary>
    /// <param name="projects"></param>
    /// <param name="overlapConfiguration"></param>
    /// <returns></returns>
    private static BillableDays GetBillableDays(Project[] projects, OverlapConfiguration overlapConfiguration)
    {
        var billableDays = new BillableDays();

        foreach (var project in projects)
        {
            // if the start date is already in the dictionary, calculate the overlap
            if (billableDays.TryGetValue(project.Dates.Start, out var start))
            {
                var billableDay = overlapConfiguration.Calculate(
                    start with { Type = DayType.Full },
                    new BillableDay(project, project.Dates.Start, DayType.Full));

                billableDays[project.Dates.Start] = billableDay;
            }
            else
            {
                // otherwise, add the start date to the dictionary as a travel day
                billableDays[project.Dates.Start] = new BillableDay(project, project.Dates.Start, DayType.Travel);
            }

            // add the full days to the dictionary
            foreach (var interstitialDay in project.Dates.InterstitialDays())
            {
                billableDays[interstitialDay] = new BillableDay(project, interstitialDay, DayType.Full);
            }

            // if the end date is already in the dictionary, calculate the overlap
            if (billableDays.TryGetValue(project.Dates.End, out var end))
            {
                var billableDay = overlapConfiguration.Calculate(
                    end with { Type = DayType.Full },
                    new BillableDay(project, project.Dates.End, DayType.Full));

                billableDays[project.Dates.End] = billableDay;
            }
            else
            {
                // otherwise, add the end date to the dictionary as a travel day
                billableDays[project.Dates.End] = new BillableDay(project, project.Dates.End, DayType.Travel);
            }
        }

        return billableDays;
    }

    /// <summary>
    /// Revises the travel days to full days *if* they intersect with or overlap other projects.
    /// </summary>
    private static BillableDays ReviseTravelDaysToFullDays(BillableDays billableDays)
    {
        for (var i = 0; i < billableDays.Count; i++)
        {
            var current = billableDays.ElementAt(i).Value;
            var next = i < billableDays.Count - 1 ? billableDays.ElementAt(i + 1).Value : null;
            var last = i > 0 ? billableDays.ElementAt(i - 1).Value : null;

            var edges = new [] {last, next} // get the previous and next days and stuff them into an array
                .Where(x => x != null) // filter out nulls
                .Select(x=> x!) // asserts that x is not null (for intellisense)
                .ToArray();
            
            foreach (var edge in edges)
            {
                // if the edge project dates do not intersect with the current project dates, skip
                if (!edge.Project.Dates.IsIntersectionOrAdjacent(current.Project.Dates, out var intersectingDates))
                {
                    continue;
                }
                
                foreach (var date in intersectingDates)
                {
                    // revise the travel day to a full day
                    billableDays[date] = billableDays[date] with { Type = DayType.Full };
                }
            }
        }
        
        return billableDays;
    }
}