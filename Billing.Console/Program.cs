using Billing.Domain.Common.Abstractions;
using Billing.Domain.Models;

Console.WriteLine("Project Billing Calculator");

/*
 * Project 1: Low Cost City Start Date: 9/1/15 End Date: 9/3/15
 */
var set1 = new ProjectSet(
    new Project("Project  1", CityType.LoCost, new DateRange("2015-09-01", "2015-09-03"))
);

/*
 * Project 1: Low Cost City Start Date: 9/1/15 End Date: 9/1/15
 * Project 2: High Cost City Start Date: 9/2/15 End Date: 9/6/15
 * Project 3: Low Cost City Start Date: 9/6/15 End Date: 9/8/15
 */
var set2 = new ProjectSet(
    new Project("Project 1", CityType.LoCost, new DateRange("2015-09-01", "2015-09-01")),
    new Project("Project 2", CityType.HiCost, new DateRange("2015-09-02", "2015-09-06")),
    new Project("Project 3", CityType.LoCost, new DateRange("2015-09-06", "2015-09-08"))
);

/*
 * Project 1: Low Cost City Start Date: 9/1/15 End Date: 9/3/15
 * Project 2: High Cost City Start Date: 9/5/15 End Date: 9/7/15
 * Project 3: High Cost City Start Date: 9/8/15 End Date: 9/8/15
 */
var set3 = new ProjectSet(
    new Project("Project 1", CityType.LoCost, new DateRange("2015-09-01", "2015-09-03")),
    new Project("Project 2", CityType.HiCost, new DateRange("2015-09-05", "2015-09-07")),
    new Project("Project 3", CityType.HiCost, new DateRange("2015-09-08", "2015-09-08"))
);

/*
 * Project 1: Low Cost City Start Date: 9/1/15 End Date: 9/1/15
 * Project 2: Low Cost City Start Date: 9/1/15 End Date: 9/1/15
 * Project 3: High Cost City Start Date: 9/2/15 End Date: 9/2/15
 * Project 4: High Cost City Start Date: 9/2/15 End Date: 9/3/15
 */
var set4 = new ProjectSet(
    new Project("Project 1", CityType.LoCost, new DateRange("2015-09-01", "2015-09-01")),
    new Project("Project 2", CityType.LoCost, new DateRange("2015-09-01", "2015-09-01")),
    new Project("Project 3", CityType.HiCost, new DateRange("2015-09-02", "2015-09-02")),
    new Project("Project 4", CityType.HiCost, new DateRange("2015-09-02", "2015-09-03"))
);


ProjectSet[] sets = {set1, set2, set3, set4};

var i = 1;
foreach (var projectSet in sets)
{
    var projectBills = projectSet.Calculate(OverlapConfiguration.TakeGreater);

    Console.WriteLine($"Project Set #{i}");
    foreach (var (project, bill) in projectBills.OrderBy(pb => pb.Key.Dates.Start))
    {
        Console.WriteLine($"Project: {project.Name}, Bill: {bill:C}");
    }
    Console.WriteLine("\n\n\n");

    i++;
}



