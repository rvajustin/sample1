using System;
using System.Collections.Generic;
using Billing.Domain.Models;
using FluentAssertions;

namespace Billing.Tests.Abstractions;

public abstract class ProjectSetTest
{
    private ProjectSet? _projectSet;
    private Dictionary<Project, decimal>? _projectBills;
    
    protected void Given(params Project[] projects)
    {
        _projectSet = new ProjectSet(projects);
    }
    
    protected void WhenCalculated(OverlapConfiguration overlapConfiguration)
    {
        if (_projectSet is null)
        {
            throw new InvalidOperationException("Please call Given before WhenCalculated");
        }
        
        _projectBills = _projectSet.Calculate(overlapConfiguration);
    }
    
    protected void ThenProjectBillEquals(Project project, decimal amount)
    {
        if (_projectBills is null)
        {
            throw new InvalidOperationException("Please call WhenCalculated before asserting");
        }
        
        _projectBills.Should().ContainKey(project);
        _projectBills[project].Should().Be(amount);
    }
    
    
    protected void ThenProjectBillShouldNotInclude(Project project)
    {
        if (_projectBills is null)
        {
            throw new InvalidOperationException("Please call WhenCalculated before asserting");
        }
        
        _projectBills.Should().NotContainKey(project);
    }
}