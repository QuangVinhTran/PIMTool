using Application.Interfaces.Repositories;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using Domain.Tests.Customized;
using FluentAssertions;
using Infrastructures.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories
{
    public class ProjectEmployeeRepositoryTests : SetupTest
    {
        private readonly IProjectEmployeeRepository _projectEmployeeRepository;

        public ProjectEmployeeRepositoryTests()
        {
            _projectEmployeeRepository = new ProjectEmployeeRepository(
                _dbContext);
        }

        [Fact]
        public async Task ProjectEmployeeRepository_AddAsync_ShouldReturnCorrectData()
        {
            // Arrange
            var fixture = new CustomizedFixture();
            var projectMockData = fixture.Create<Project>();
            var employeeMockData = fixture.Create<Employee>();

            var projectEmployee = new ProjectEmployee
            {
                ProjectId = projectMockData.Id,
                EmployeeId = employeeMockData.Id
            };

            // Act
            await _dbContext.ProjectEmployees.AddAsync(projectEmployee);
            await _dbContext.SaveChangesAsync();
            var addedEntity = await _dbContext.ProjectEmployees.FirstOrDefaultAsync(g => g.ProjectId == projectEmployee.ProjectId);

            // Assert
            addedEntity.Should().NotBeNull();
        }
        
    }
}