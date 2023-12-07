using System;
using Application.Interfaces.Repositories;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;
using Infrastructures.Repositories;

namespace Infrastructure.Tests.Repositories
{
    public class Repository : SetupTest
    {
        private readonly IRepository<Employee> _repository;

        public Repository()
        {
            _repository = new Repository<Employee>(
                _dbContext);
        }

        [Fact]
        public async Task Repository_GetAllAsync_ShouldReturnCorrectData()
        {
            var mockData = _fixture.Build<Employee>().CreateMany(10).ToList();
            await _dbContext.Employees.AddRangeAsync(mockData);

            await _dbContext.SaveChangesAsync();

            var result = await _repository.GetAllAsync();

            result.Should().BeEquivalentTo(mockData);
        }

        [Fact]
        public async Task GenericRepository_GetAllAsync_ShouldReturnEmptyWhenHaveNoData()
        {
            var result = await _repository.GetAllAsync();

            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GenericRepository_GetByIdAsync_ShouldReturnCorrectData()
        {
            var mockData = _fixture.Build<Employee>().Create();
            await _dbContext.Employees.AddAsync(mockData);

            await _dbContext.SaveChangesAsync();

            var result = await _repository.GetAsync(mockData.Id);

            result.Should().BeEquivalentTo(mockData);
        }

        [Fact]
        public async Task GenericRepository_GetByIdAsync_ShouldReturnEmptyWhenHaveNoData()
        {
            var result = await _repository.GetAsync(0);

            result.Should().BeNull();
        }

        [Fact]
        public async Task GenericRepository_AddAsync_ShouldReturnCorrectData()
        {
            var mockData = _fixture.Build<Employee>().Create();

            await _repository.AddAsync(mockData);
            var result = await _dbContext.SaveChangesAsync();

            result.Should().Be(1);
        }

        [Fact]
        public async Task GenericRepository_AddRangeAsync_ShouldReturnCorrectData()
        {
            var mockData = _fixture.Build<Employee>().CreateMany(10).ToList();

            await _repository.AddRangeAsync(mockData);
            var result = await _dbContext.SaveChangesAsync();

            result.Should().Be(10);
        }

        [Fact]
        public async Task GenericRepository_Delete_ShouldReturnCorrectData()
        {
            var mockData = _fixture.Build<Employee>().Create();
            _dbContext.Employees.AddRange(mockData);
            await _dbContext.SaveChangesAsync();

            _repository.Delete(mockData);

            var result = await _dbContext.SaveChangesAsync();

            result.Should().Be(1);
        }

        [Fact]
        public async Task GenericRepository_Update_ShouldReturnCorrectData()
        {
            var mockData = _fixture.Build<Employee>().Create();
            await _dbContext.Employees.AddAsync(mockData);
            await _dbContext.SaveChangesAsync();

            _repository.Update(mockData);
            var result = await _dbContext.SaveChangesAsync();

            result.Should().Be(1);
        }

        [Fact]
        public async Task GenericRepository_ToPagination_ShouldReturnCorrectDataFirstsPage()
        {
            var mockData = _fixture.Build<Employee>().CreateMany(45).ToList();
            await _dbContext.Employees.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();

            var paginasion = await _repository.ToPagination();

            paginasion.Previous.Should().BeFalse();
            paginasion.Next.Should().BeTrue();
            paginasion.Items.Count.Should().Be(10);
            paginasion.TotalItemsCount.Should().Be(45);
            paginasion.TotalPagesCount.Should().Be(5);
            paginasion.PageIndex.Should().Be(0);
            paginasion.PageSize.Should().Be(10);
        }

        [Fact]
        public async Task GenericRepository_ToPagination_ShouldReturnCorrectDataSecoundPage()
        {
            var mockData = _fixture.Build<Employee>().CreateMany(45).ToList();
            await _dbContext.Employees.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();


            var paginasion = await _repository.ToPagination(1, 20);


            paginasion.Previous.Should().BeTrue();
            paginasion.Next.Should().BeTrue();
            paginasion.Items.Count.Should().Be(20);
            paginasion.TotalItemsCount.Should().Be(45);
            paginasion.TotalPagesCount.Should().Be(3);
            paginasion.PageIndex.Should().Be(1);
            paginasion.PageSize.Should().Be(20);
        }

        [Fact]
        public async Task GenericRepository_ToPagination_ShouldReturnCorrectDataLastPage()
        {
            var mockData = _fixture.Build<Employee>().CreateMany(45).ToList();
            await _dbContext.Employees.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();


            var paginasion = await _repository.ToPagination(2, 20);


            paginasion.Previous.Should().BeTrue();
            paginasion.Next.Should().BeFalse();
            paginasion.Items.Count.Should().Be(5);
            paginasion.TotalItemsCount.Should().Be(45);
            paginasion.TotalPagesCount.Should().Be(3);
            paginasion.PageIndex.Should().Be(2);
            paginasion.PageSize.Should().Be(20);
        }

        [Fact]
        public async Task GenericRepository_ToPagination_ShouldReturnWithoutData()
        {
            var paginasion = await _repository.ToPagination();

            paginasion.Previous.Should().BeFalse();
            paginasion.Next.Should().BeFalse();
            paginasion.Items.Count.Should().Be(0);
            paginasion.TotalItemsCount.Should().Be(0);
            paginasion.TotalPagesCount.Should().Be(0);
            paginasion.PageIndex.Should().Be(0);
            paginasion.PageSize.Should().Be(5);
        }

    }
}

