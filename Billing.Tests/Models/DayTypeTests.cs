using System;
using Billing.Domain.Models;
using FluentAssertions;
using Xunit;

namespace Billing.Tests.Models;

public class DayTypeTests
{
    [Fact]
    public void TravelDay_HasCorrectRates()
    {
        var travelDay = DayType.Travel;
        travelDay.Rates[CityType.HiCost].Amount.Should().Be(55);
        travelDay.Rates[CityType.LoCost].Amount.Should().Be(45);
    }

    [Fact]
    public void FullDay_HasCorrectRates()
    {
        var fullDay = DayType.Full;
        fullDay.Rates[CityType.HiCost].Amount.Should().Be(85);
        fullDay.Rates[CityType.LoCost].Amount.Should().Be(75);
    }

    [Fact]
    public void DayType_EqualityCheck()
    {
        var travelDay1 = DayType.Travel;
        var travelDay2 = DayType.Travel;
        travelDay1.Should().Be(travelDay2);
    }

    [Fact]
    public void DayType_InequalityCheck()
    {
        var travelDay = DayType.Travel;
        var fullDay = DayType.Full;
        travelDay.Should().NotBe(fullDay);
    }

    [Fact]
    public void DayType_RatesAreImmutable()
    {
        var travelDay = DayType.Travel;
        Assert.Throws<ArgumentException>(()
            => travelDay.Rates.Add(CityType.HiCost, new Rate(CityType.HiCost, 60)));
    }
}