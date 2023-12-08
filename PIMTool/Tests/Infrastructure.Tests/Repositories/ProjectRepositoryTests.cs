using System;
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
	public class ProjectRepositoryTests : SetupTest
	{
		private readonly IProjectRepository _projectRepository;
		public ProjectRepositoryTests()
		{
			_projectRepository = new ProjectRepository(
				_dbContext);
		}

        [Fact]
        public async Task ProjectRepository_AddAsync_ShouldReturnCorrectData()
        {
            // Arrange
            var fixture = new CustomizedFixture();
            var mockData = fixture.Create<Project>();

            // Act
            await _dbContext.AddAsync(mockData);
            await _dbContext.SaveChangesAsync();
            var addedEntity = await _dbContext.Projects.FirstOrDefaultAsync(g => g.Id == mockData.Id);

            // Assert
            addedEntity.Should().NotBeNull();
        }

        [Fact]
        public async Task ProjectRepository_AddRangeAsync_ShouldReturnCorrectData()
        {
            // Arrange
            var fixture = new CustomizedFixture();
            var mockData = fixture.CreateMany<Project>(10).ToList();

            // Act
            await _dbContext.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();

            // Assert
            var count = await _dbContext.Projects.CountAsync();
            count.Should().Be(10);
        }

        [Fact]
        public async Task ProjectRepository_GetAllAsync_ShouldReturnCorrectData()
        {
            // Arrange
            var fixture = new CustomizedFixture();
            var mockData = fixture.CreateMany<Project>(10).ToList();

            // Act
            await _dbContext.Projects.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();

            // Assert
            var result = await _projectRepository.GetAllAsync();
            result.Should().BeEquivalentTo(mockData);
        }

        [Fact]
        public async Task ProjectRepository_GetAllAsync_ShouldReturnEmptyWhenHaveNoData()
        {
            var result = await _projectRepository.GetAllAsync();

            result.Should().BeEmpty();
        }

        [Fact]
        public async Task ProjectRepository_GetByIdAsync_ShouldReturnCorrectData()
        {
            // Arrange
            var fixture = new CustomizedFixture();
            var mockData = fixture.Create<Project>();

            // Act
            await _dbContext.Projects.AddAsync(mockData);
            await _dbContext.SaveChangesAsync();

            // Assert
            var result = await _projectRepository.GetAsync(mockData.Id);
            result.Should().BeEquivalentTo(mockData);
        }

        [Fact]
        public async Task ProjectRepository_GetByIdAsync_ShouldReturnEmptyWhenHaveNoData()
        {
            var result = await _projectRepository.GetAsync(0);

            result.Should().BeNull();
        }

        [Fact]
        public async Task ProjectRepository_Delete_ShouldReturnCorrectData()
        {
            // Arrange
            var fixture = new CustomizedFixture();
            var mockData = fixture.Create<Project>();

            // Act
            _dbContext.Projects.AddRange(mockData);
            await _dbContext.SaveChangesAsync();
            _projectRepository.Delete(mockData);
            await _dbContext.SaveChangesAsync();

            // Assert
            var exists = await _dbContext.Projects.AnyAsync(x => x.Id == mockData.Id);
            exists.Should().BeFalse();
        }

        [Fact]
        public async Task ProjectRepository_Update_ShouldReturnCorrectData()
        {
            // Arrange
            var fixture = new CustomizedFixture();
            var mockData = fixture.Create<Project>();

            // Act
            await _dbContext.Projects.AddAsync(mockData);
            await _dbContext.SaveChangesAsync();
            _projectRepository.Update(mockData);
            var result = await _dbContext.SaveChangesAsync();

            // Assert
            result.Should().Be(1);
        }

        [Fact]
        public async Task ProjectRepository_GetEmployeeInProject_ShouldReturnCorrectData()
        {
            // Arrange
            var fixture = new CustomizedFixture();
            var project = fixture.Create<Project>();
            var employee1 = fixture.Create<Employee>();
            var employee2 = fixture.Create<Employee>();

            var projectEmployees = new List<ProjectEmployee> {
                new ProjectEmployee {ProjectId = project.Id, EmployeeId = employee1.Id},
                new ProjectEmployee {ProjectId = project.Id, EmployeeId = employee2.Id}
            };
            await _dbContext.Projects.AddAsync(project);
            foreach (var item in projectEmployees)
            {
                await _dbContext.AddAsync(item);
            }
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _projectRepository.GetEmployeInProjectAsync(project.Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(project.ProjectEmployees.Count);
            result.Should().Equal(project.ProjectEmployees.Select(pe => pe.Employee));
        }

        [Fact]
        public async Task ProjectRepository_GetEmployeeInProject_ShouldReturnEmptyData()
        {
            // Arrange
            var fixture = new CustomizedFixture();
            var project = fixture.Create<Project>();
            await _dbContext.Projects.AddAsync(project);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _projectRepository.GetEmployeInProjectAsync(project.Id);

            // Assert
            result.Should().BeEmpty();
        }
    }
}

