using System;
using Billing.Domain.Common.Abstractions;
using Billing.Domain.Models;
using FluentAssertions;
using Xunit;

namespace Billing.Tests.Models;

public class ProjectTests
{    [Fact]
    public void Project_Constructor_SetsPropertiesCorrectly()
    {
        var dateRange = new DateRange("2023-01-01", "2023-01-10");
        var project = new Project("Project A", CityType.HiCost, dateRange);
        project.Name.Should().Be("Project A");
        project.CityType.Should().Be(CityType.HiCost);
        project.Dates.Should().Be(dateRange);
    }

    [Fact]
    public void Project_EqualityCheck_ReturnsTrueForSameId()
    {
        var dateRange = new DateRange("2023-01-01", "2023-01-10");
        var project1 = new Project(Guid.NewGuid(), "Project A", CityType.HiCost, dateRange);
        var project2 = new Project(project1.Id, "Project B", CityType.LoCost, dateRange);
        project1.Equals(project2).Should().BeTrue();
    }

    [Fact]
    public void Project_EqualityCheck_ReturnsFalseForDifferentId()
    {
        var dateRange = new DateRange("2023-01-01", "2023-01-10");
        var project1 = new Project(Guid.NewGuid(), "Project A", CityType.HiCost, dateRange);
        var project2 = new Project(Guid.NewGuid(), "Project B", CityType.LoCost, dateRange);
        project1.Equals(project2).Should().BeFalse();
    }

    [Fact]
    public void Project_GetHashCode_ReturnsSameHashCodeForSameId()
    {
        var dateRange = new DateRange("2023-01-01", "2023-01-10");
        var project1 = new Project(Guid.NewGuid(), "Project A", CityType.HiCost, dateRange);
        var project2 = new Project(project1.Id, "Project B", CityType.LoCost, dateRange);
        project1.GetHashCode().Should().Be(project2.GetHashCode());
    }

    [Fact]
    public void Project_Deconstruct_AssignsValuesCorrectly()
    {
        var dateRange = new DateRange("2023-01-01", "2023-01-10");
        var project = new Project("Project A", CityType.HiCost, dateRange);
        project.Deconstruct(out var id, out var name, out var cityType, out var dates);
        id.Should().Be(project.Id);
        name.Should().Be("Project A");
        cityType.Should().Be(CityType.HiCost);
        dates.Should().Be(dateRange);
    }
}