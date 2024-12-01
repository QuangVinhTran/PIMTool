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
	public class GroupRepositoryTests : SetupTest
	{
		private readonly IGroupRepository _groupRepository;
		public GroupRepositoryTests()
		{
			_groupRepository = new GroupRepository(
				_dbContext);
		}

        [Fact]
        public async Task GroupRepository_AddAsync_ShouldReturnCorrectData()
        {
            // Arrange
            var fixture = new CustomizedFixture();
            var mockData = fixture.Create<Group>();

            // Act
            await _dbContext.AddAsync(mockData);
            await _dbContext.SaveChangesAsync();
            var addedEntity = await _dbContext.Groups.FirstOrDefaultAsync(g => g.Id == mockData.Id);

            // Assert
            addedEntity.Should().NotBeNull();
        }

        [Fact]
        public async Task GroupRepository_AddRangeAsync_ShouldReturnCorrectData()
        {
            // Arrange
            var fixture = new CustomizedFixture();
            var mockData = fixture.CreateMany<Group>(10).ToList();

            // Act
            await _dbContext.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();

            // Assert
            var count = await _dbContext.Groups.CountAsync();
            count.Should().Be(10);
        }

        [Fact]
        public async Task GroupRepository_GetAllAsync_ShouldReturnCorrectData()
        {
            // Arrange
            var fixture = new CustomizedFixture();
            var mockData = fixture.CreateMany<Group>(10).ToList();

            // Act
            await _dbContext.Groups.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();

            // Assert
            var result = await _groupRepository.GetAllAsync();
            result.Should().BeEquivalentTo(mockData);
        }

        [Fact]
        public async Task GroupRepository_GetAllAsync_ShouldReturnEmptyWhenHaveNoData()
        {
            var result = await _groupRepository.GetAllAsync();

            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GroupRepository_GetByIdAsync_ShouldReturnCorrectData()
        {
            // Arrange
            var fixture = new CustomizedFixture();
            var mockData = fixture.Create<Group>();

            // Act
            await _dbContext.Groups.AddAsync(mockData);
            await _dbContext.SaveChangesAsync();

            // Assert
            var result = await _groupRepository.GetAsync(mockData.Id);
            result.Should().BeEquivalentTo(mockData);
        }

        [Fact]
        public async Task GroupRepository_GetByIdAsync_ShouldReturnEmptyWhenHaveNoData()
        {
            var result = await _groupRepository.GetAsync(0);

            result.Should().BeNull();
        }

        [Fact]
        public async Task GroupRepository_Delete_ShouldReturnCorrectData()
        {
            // Arrange
            var fixture = new CustomizedFixture();
            var mockData = fixture.Create<Group>();

            // Act
            _dbContext.Groups.AddRange(mockData);
            await _dbContext.SaveChangesAsync();
            _groupRepository.Delete(mockData);
            await _dbContext.SaveChangesAsync();

            // Assert
            var exists = await _dbContext.Groups.AnyAsync(x => x.Id == mockData.Id);
            exists.Should().BeFalse();
        }

        [Fact]
        public async Task GroupRepository_Update_ShouldReturnCorrectData()
        {
            // Arrange
            var fixture = new CustomizedFixture();
            var mockData = fixture.Create<Group>();

            // Act
            await _dbContext.Groups.AddAsync(mockData);
            await _dbContext.SaveChangesAsync();
            _groupRepository.Update(mockData);
            var result = await _dbContext.SaveChangesAsync();

            // Assert
            result.Should().Be(1);
        }
    }
}

