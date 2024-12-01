using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests
{
	public class PimContextTests : SetupTest, IDisposable
	{
		[Fact]
		public async Task PimContext_EmployeesDbSetShouldReturnCorrectData()
		{

			var mockData = _fixture.Build<Employee>().CreateMany(10).ToList();
			await _dbContext.Employees.AddRangeAsync(mockData);

			await _dbContext.SaveChangesAsync();

			var result = await _dbContext.Employees.ToListAsync();
			result.Should().BeEquivalentTo(mockData);
		}

		[Fact]
		public async Task PimContext_EmployeesDbSetShouldReturnEmptyListWhenNotHavingData()
		{
			var result = await _dbContext.Employees.ToListAsync();
			result.Should().BeEmpty();
		}

	}
}
