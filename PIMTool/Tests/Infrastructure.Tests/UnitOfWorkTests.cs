using System;
using System.Transactions;
using Application;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using Domain.Tests.Customized;
using FluentAssertions;
using Infrastructures;
using Moq;

namespace Infrastructure.Tests
{
    public class UnitOfWorkTests : SetupTest
    {
        private readonly IUnitOfWork _unitOfWork;
        public UnitOfWorkTests()
        {
            _unitOfWork = new UnitOfWork(
               _dbContext,
               _projectRepositoryMock.Object,
               _employeeRepositoryMock.Object,
               _groupRepositoryMock.Object,
               _projectEmployeeRepositoryMock.Object
               );
        }

        [Fact]
        public async Task TestEmployeeRepositoryMock_Should_ReturnCorrectData()
        {
            // arrange
            var mockData = _fixture.Build<Employee>().CreateMany(10).ToList();

            _employeeRepositoryMock.Setup(x => x.GetAllAsync(CancellationToken.None)).ReturnsAsync(mockData);

            // act
            var items = await _unitOfWork.EmployeeRepository.GetAllAsync();

            //assert
            items.Should().BeEquivalentTo(mockData);
        }

        [Fact]
        public async Task SaveChangesAsync_Should_SaveChangesAsync()
        {
            // Arrange
            var fixture = new CustomizedFixture();
            var mockData = fixture.Create<Employee>();

            // Act
            await _dbContext.Employees.AddRangeAsync(mockData);
            var result = await _unitOfWork.SaveChangeAsync(CancellationToken.None);

            // Assert
            result.Should().BeGreaterThan(0);
        }
    }
}
