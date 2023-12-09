using System;
using Application.Commons;
using Application.Interfaces.Repositories;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;
using Infrastructures.Repositories;
using Microsoft.EntityFrameworkCore;

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
            await _dbContext.SaveChangesAsync();

            var addedEntity = await _dbContext.Employees.FirstOrDefaultAsync(e => e.Id == mockData.Id);

            addedEntity.Should().NotBeNull();
        }

        [Fact]
        public async Task GenericRepository_AddRangeAsync_ShouldReturnCorrectData()
        {
            var mockData = _fixture.Build<Employee>().CreateMany(10).ToList();

            await _repository.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();

            var count = await _dbContext.Employees.CountAsync();
            count.Should().Be(10);
        }

        [Fact]
        public async Task GenericRepository_Delete_ShouldReturnCorrectData()
        {
            var mockData = _fixture.Build<Employee>().Create();
            _dbContext.Employees.AddRange(mockData);
            await _dbContext.SaveChangesAsync();

            _repository.Delete(mockData);

            await _dbContext.SaveChangesAsync();

            var exists = await _dbContext.Employees.AnyAsync(x => x.Id == mockData.Id);
            exists.Should().BeFalse();
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

            var pagination = await _repository.ToPagination(0, 10);

            pagination.Previous.Should().BeFalse();
            pagination.Next.Should().BeTrue();
            pagination.Items.Count.Should().Be(10);
            pagination.TotalItemsCount.Should().Be(45);
            pagination.TotalPagesCount.Should().Be(5);
            pagination.PageIndex.Should().Be(0);
            pagination.PageSize.Should().Be(10);
        }

        [Fact]
        public async Task GenericRepository_ToPagination_ShouldReturnCorrectDataSecondPage()
        {
            var mockData = _fixture.Build<Employee>().CreateMany(45).ToList();
            await _dbContext.Employees.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();


            var pagination = await _repository.ToPagination(1, 20);


            pagination.Previous.Should().BeTrue();
            pagination.Next.Should().BeTrue();
            pagination.Items.Count.Should().Be(20);
            pagination.TotalItemsCount.Should().Be(45);
            pagination.TotalPagesCount.Should().Be(3);
            pagination.PageIndex.Should().Be(1);
            pagination.PageSize.Should().Be(20);
        }

        [Fact]
        public async Task GenericRepository_ToPagination_ShouldReturnCorrectDataLastPage()
        {
            var mockData = _fixture.Build<Employee>().CreateMany(45).ToList();
            await _dbContext.Employees.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();


            var pagination = await _repository.ToPagination(2, 20);


            pagination.Previous.Should().BeTrue();
            pagination.Next.Should().BeFalse();
            pagination.Items.Count.Should().Be(5);
            pagination.TotalItemsCount.Should().Be(45);
            pagination.TotalPagesCount.Should().Be(3);
            pagination.PageIndex.Should().Be(2);
            pagination.PageSize.Should().Be(20);
        }

        [Fact]
        public async Task GenericRepository_ToPagination_ShouldReturnWithoutData()
        {
            var pagination = await _repository.ToPagination();

            pagination.Previous.Should().BeFalse();
            pagination.Next.Should().BeFalse();
            pagination.Items.Count.Should().Be(0);
            pagination.TotalItemsCount.Should().Be(0);
            pagination.TotalPagesCount.Should().Be(0);
            pagination.PageIndex.Should().Be(0);
            pagination.PageSize.Should().Be(5);
        }

    }
}

