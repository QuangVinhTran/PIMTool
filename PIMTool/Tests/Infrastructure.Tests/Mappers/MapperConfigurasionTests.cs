using Application.ViewModels.EmployeeViewModels;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;

namespace Infrastructure.Tests.Mappers
{
	public class MapperConfigurasionTests : SetupTest
	{
		public MapperConfigurasionTests()
		{
		}

        [Fact]
        public void TestMapper()
        {
            //arrange
            var employeeMock = _fixture.Build<Employee>().Create();

            //act
            var result = _mapperConfig.Map<EmployeeViewModel>(employeeMock);

            //assert
            result.Id.Should().Be(employeeMock.Id);
        }
    }
}

