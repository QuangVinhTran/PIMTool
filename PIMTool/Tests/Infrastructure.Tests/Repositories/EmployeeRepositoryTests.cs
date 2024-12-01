using System;
using Application.Interfaces.Repositories;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using Domain.Tests.Customized;
using Infrastructures.Repositories;

namespace Infrastructure.Tests.Repositories
{
	public class EmployeeRepositoryTests : SetupTest
	{
		private readonly IEmployeeRepository _employeeRepository;
		public EmployeeRepositoryTests()
		{
			_employeeRepository = new EmployeeRepository(
				_dbContext);
		}

		[Theory]
		[InlineData("ABC")]
		[InlineData("XYZ")]
		public async Task SearchEmployeeByVisaAsync_WithValidData_ShouldReturnCorrectData(string visaToSearch)
		{
			// Arrange
			var fixture = new CustomizedFixture();
			var mockData = fixture.CreateMany<Employee>(10).ToList();
			await _employeeRepository.AddRangeAsync(mockData);
			await _dbContext.SaveChangesAsync();

			// Act
			var result = await _employeeRepository.SearchEmployeeByVisaAsync(visaToSearch);

			// Assert
			Assert.NotNull(result);
			Assert.True(result.All(e => e.Visa != visaToSearch));
		}

		[Theory]
		[InlineData("NonExistentVisa")]
		public async Task SearchEmployeeByVisaAsync_WithNoMatch_ShouldReturnEmptyList(string visaToSearch)
		{
			// Arrange
			var fixture = new CustomizedFixture();
			var mockData = fixture.CreateMany<Employee>(10).ToList();
			await _employeeRepository.AddRangeAsync(mockData);
			await _dbContext.SaveChangesAsync();

			// Act
			var result = await _employeeRepository.SearchEmployeeByVisaAsync(visaToSearch);

			// Assert
			Assert.Empty(result);
		}
	}
}
