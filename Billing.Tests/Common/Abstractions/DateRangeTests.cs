using System;
using System.Linq;
using Billing.Domain.Common.Abstractions;
using FluentAssertions;
using Xunit;

namespace Billing.Tests.Abstractions;

public class DateRangeTests
{
     [Fact]
    public void DateRange_Constructor_ParsesStringsCorrectly()
    {
        var dateRange = new DateRange("2023-01-01", "2023-01-10");
        dateRange.Start.Should().Be(new DateOnly(2023, 1, 1));
        dateRange.End.Should().Be(new DateOnly(2023, 1, 10));
    }

    [Fact]
    public void Equals_ReturnsTrueForSameValues()
    {
        var dateRange1 = new DateRange("2023-01-01", "2023-01-10");
        var dateRange2 = new DateRange("2023-01-01", "2023-01-10");
        dateRange1.Should().Be(dateRange2);
    }

    [Fact]
    public void Equals_ReturnsFalseForDifferentValues()
    {
        var dateRange1 = new DateRange("2023-01-01", "2023-01-10");
        var dateRange2 = new DateRange("2023-01-02", "2023-01-11");
        dateRange1.Should().NotBe(dateRange2);
    }

    [Fact]
    public void IsIntersectionOrAdjacent_ReturnsTrueForIntersection()
    {
        var dateRange1 = new DateRange("2023-01-01", "2023-01-10");
        var dateRange2 = new DateRange("2023-01-10", "2023-01-20");
        dateRange1.IsIntersectionOrAdjacent(dateRange2, out var dates).Should().BeTrue();
        dates.Should().HaveCount(1);
        dates[0].Should().Be(new DateOnly(2023, 1, 10));
    }

    [Fact]
    public void IsIntersectionOrAdjacent_ReturnsTrueForAdjacent()
    {
        var dateRange1 = new DateRange("2023-01-01", "2023-01-10");
        var dateRange2 = new DateRange("2023-01-11", "2023-01-20");
        dateRange1.IsIntersectionOrAdjacent(dateRange2, out var dates).Should().BeTrue();
        Assert.Equal(2, dates.Length);
        Assert.Equal(new DateOnly(2023, 1, 10), dates.Min());
        Assert.Equal(new DateOnly(2023, 1, 11), dates.Max());
    }

    [Fact]
    public void IsIntersectionOrAdjacent_ReturnsFalseForNonAdjacent()
    {
        var dateRange1 = new DateRange("2023-01-01", "2023-01-10");
        var dateRange2 = new DateRange("2023-01-12", "2023-01-20");
        dateRange1.IsIntersectionOrAdjacent(dateRange2, out var dates).Should().BeFalse();
        dates.Should().BeEmpty();
    }

    [Fact]
    public void InterstitialDays_ReturnsCorrectDays()
    {
        var dateRange = new DateRange("2023-01-01", "2023-01-05");
        var days = dateRange.InterstitialDays();
        days.Length.Should().Be(3);
        days[0].Should().Be(new DateOnly(2023, 1, 2));
        days[1].Should().Be(new DateOnly(2023, 1, 3));
        days[2].Should().Be(new DateOnly(2023, 1, 4));
    }

    [Fact]
    public void InterstitialDays_ReturnsEmptyForNoMiddleDays()
    {
        var dateRange = new DateRange("2023-01-01", "2023-01-02");
        var days = dateRange.InterstitialDays();
        days.Should().BeEmpty();
    }
}