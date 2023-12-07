using System;
using Application;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
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
        public async Task TestUnitOfWork()
        {
            // arrange
            var mockData = _fixture.Build<Employee>().CreateMany(10).ToList();

             _employeeRepositoryMock.Setup(x => x.GetAllAsync(CancellationToken.None)).ReturnsAsync(mockData);

            // act
            var items = await _unitOfWork.EmployeeRepository.GetAllAsync();

            //assert
            items.Should().BeEquivalentTo(mockData);
        }
    }
}

