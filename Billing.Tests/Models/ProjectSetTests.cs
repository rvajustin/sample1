using Billing.Domain.Common.Abstractions;
using Billing.Domain.Models;
using Billing.Tests.Abstractions;
using Xunit;

namespace Billing.Tests.Models;

public class ProjectSetTests : ProjectSetTest
{
    private static readonly OverlapConfiguration _overlapConfiguration = OverlapConfiguration.TakeFirst;
    
    [Fact]
    public void ProjectSet1AmountIsAccurate()
    {
        var p1 = new Project("Project 1", CityType.LoCost, new DateRange("2015-09-01", "2015-09-03"));
        
        Given(p1);
        WhenCalculated(_overlapConfiguration);
        ThenProjectBillEquals(p1, 165);
    }
    
    [Fact]
    public void ProjectSet2AmountIsAccurate()
    {
        var p1 = new Project("Project 1", CityType.LoCost, new DateRange("2015-09-01", "2015-09-01"));
        var p2 = new Project("Project 2", CityType.HiCost, new DateRange("2015-09-02", "2015-09-06"));
        var p3 = new Project("Project 3", CityType.LoCost, new DateRange("2015-09-06", "2015-09-08"));
        
        Given(p1, p2, p3);
        WhenCalculated(_overlapConfiguration);
        ThenProjectBillEquals(p1, 75);
        ThenProjectBillEquals(p2, 425);
        ThenProjectBillEquals(p3, 120);
    }
    
    [Fact]
    public void ProjectSet3AmountIsAccurate()
    {
        var p1 = new Project("Project 1", CityType.LoCost, new DateRange("2015-09-01", "2015-09-03"));
        var p2 = new Project("Project 2", CityType.HiCost, new DateRange("2015-09-05", "2015-09-07"));
        var p3 = new Project("Project 3", CityType.HiCost, new DateRange("2015-09-08", "2015-09-08"));
        
        Given(p1, p2, p3);
        WhenCalculated(_overlapConfiguration);
        ThenProjectBillEquals(p1, 165);
        ThenProjectBillEquals(p2, 225);
        ThenProjectBillEquals(p3, 85);
    }
    
    [Fact]
    public void ProjectSet4AmountIsAccurate()
    {
        var p1 = new Project("Project 1", CityType.LoCost, new DateRange("2015-09-01", "2015-09-01"));
        var p2 = new Project("Project 2", CityType.LoCost, new DateRange("2015-09-01", "2015-09-01"));
        var p3 = new Project("Project 3", CityType.HiCost, new DateRange("2015-09-02", "2015-09-02"));
        var p4 = new Project("Project 4", CityType.HiCost, new DateRange("2015-09-02", "2015-09-03"));
        
        Given(p1, p2, p3, p4);
        WhenCalculated(_overlapConfiguration);
        ThenProjectBillEquals(p1, 75);
        ThenProjectBillShouldNotInclude(p2);
        ThenProjectBillEquals(p3, 85);
        ThenProjectBillEquals(p4, 55);
    }
}
