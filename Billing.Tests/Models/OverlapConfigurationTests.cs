using System;
using Billing.Domain.Common.Abstractions;
using Billing.Domain.Models;
using FluentAssertions;
using Xunit;

namespace Billing.Tests.Models;

public class OverlapConfigurationTests
{
    private static BillableDay MakeDay(string dateVal, DayType dayType, CityType cityType)
    {
        var date = DateOnly.Parse(dateVal);
        var project = new Project(Guid.NewGuid().ToString(), cityType, new DateRange(date, date));
        return new BillableDay(project, date, dayType);
    }


    [Fact]
    public void TakeFirst_ReturnsFirstDay()
    {
        var firstDay = MakeDay("2023-01-01", DayType.Full, CityType.HiCost);
        var secondDay = MakeDay("2023-01-02", DayType.Travel, CityType.LoCost);
        var result = OverlapConfiguration.TakeFirst.Calculate(firstDay, secondDay);
        result.Should().Be(firstDay);
    }

    [Fact]
    public void TakeLast_ReturnsLastDay()
    {
        var firstDay = MakeDay("2023-01-01", DayType.Full, CityType.HiCost);
        var secondDay = MakeDay("2023-01-02", DayType.Travel, CityType.LoCost);
        var result = OverlapConfiguration.TakeLast.Calculate(firstDay, secondDay);
        result.Should().Be(secondDay);
    }

    [Fact]
    public void TakeGreater_ReturnsDayWithGreaterCost()
    {
        var firstDay = MakeDay("2023-01-01", DayType.Full, CityType.HiCost);
        var secondDay = MakeDay("2023-01-02", DayType.Travel, CityType.LoCost);
        var result = OverlapConfiguration.TakeGreater.Calculate(firstDay, secondDay);
        result.Should().Be(firstDay);
    }

    [Fact]
    public void TakeLesser_ReturnsDayWithLesserCost()
    {
        var firstDay = MakeDay("2023-01-01", DayType.Full, CityType.HiCost);
        var secondDay = MakeDay("2023-01-02", DayType.Travel, CityType.LoCost);
        var result = OverlapConfiguration.TakeLesser.Calculate(firstDay, secondDay);
        result.Should().Be(secondDay);
    }
}