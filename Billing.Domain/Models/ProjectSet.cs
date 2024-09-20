namespace Billing.Domain.Models;

using BillableDays = SortedDictionary<DateOnly, BillableDay>;
using ProjectBills = Dictionary<Project, decimal>;

public record ProjectSet(params Project[] Projects)
{
    private IOrderedEnumerable<Project> ToSorted() 
        => Projects.OrderBy(d => d.Dates.Start).ThenBy(d => d.Dates.End);

    public ProjectBills Calculate(OverlapConfiguration? overlapConfiguration)
    {
        var projects = ToSorted().ToArray();
        var billableDays = GetBillableDays(projects, overlapConfiguration);
        billableDays = ReviseTravelDaysToFullDays(billableDays);
        var projectBills = CalculateProjectBills(billableDays);
        
        return projectBills;
    }

    private static ProjectBills CalculateProjectBills(BillableDays billableDays)
    {
        var projectBills = new ProjectBills();

        foreach (var (_, billableDay) in billableDays)
        {
            projectBills.TryAdd(billableDay.Project, 0);
            var rate = billableDay.Type.Rates[billableDay.Project.CityType];
            projectBills[billableDay.Project] = projectBills[billableDay.Project] + rate.Amount;
            // Console.WriteLine("Project: {0}, Date: {1}, Type: {2}, Rate: {3:C}", billableDay.Project.Name, billableDay.Date, billableDay.Type.Value, rate.Amount);
        }

        return projectBills;
    }

    private static BillableDays GetBillableDays(Project[] projects, OverlapConfiguration overlapConfiguration)
    {
        var billableDays = new BillableDays();

        foreach (var project in projects)
        {
            if (billableDays.TryGetValue(project.Dates.Start, out var start))
            {
                var billableDay = overlapConfiguration.Calculate(
                    start with { Type = DayType.Full },
                    new BillableDay(project, project.Dates.Start, DayType.Full));

                billableDays[project.Dates.Start] = billableDay;
            }
            else
            {
                billableDays[project.Dates.Start] = new BillableDay(project, project.Dates.Start, DayType.Travel);
            }

            foreach (var interstitialDay in project.Dates.InterstitialDays())
            {
                billableDays[interstitialDay] = new BillableDay(project, interstitialDay, DayType.Full);
            }

            if (billableDays.TryGetValue(project.Dates.End, out var end))
            {
                var billableDay = overlapConfiguration.Calculate(
                    end with { Type = DayType.Full },
                    new BillableDay(project, project.Dates.End, DayType.Full));

                billableDays[project.Dates.End] = billableDay;
            }
            else
            {
                billableDays[project.Dates.End] = new BillableDay(project, project.Dates.End, DayType.Travel);
            }
        }

        return billableDays;
    }

    private static BillableDays ReviseTravelDaysToFullDays(BillableDays billableDays)
    {
        for (var i = 0; i < billableDays.Count; i++)
        {
            var current = billableDays.ElementAt(i).Value;
            var next = i < billableDays.Count - 1 ? billableDays.ElementAt(i + 1).Value : null;
            var last = i > 0 ? billableDays.ElementAt(i - 1).Value : null;

            var edges = new [] {last, next}.Where(x => x != null).Select(x=> x!).ToArray();
            
            foreach (var edge in edges)
            {
                if (!edge.Project.Dates.IsIntersectionOrAdjacent(current.Project.Dates, out var intersectingDates))
                {
                    continue;
                }
                
                foreach (var date in intersectingDates)
                {
                    billableDays[date] = billableDays[date] with { Type = DayType.Full };
                }
            }
        }
        
        return billableDays;
    }
}