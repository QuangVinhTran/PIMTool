using Application.ViewModels.EmployeeViewModels;
using Application.ViewModels.GroupViewModels;
using Application.ViewModels.ProjectViewModels;
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
        public void TestMapperEmployee()
        {
            //arrange
            var employeeMock = _fixture.Build<Employee>().Create();

            //act
            var result = _mapperConfig.Map<EmployeeViewModel>(employeeMock);

            //assert
            result.Id.Should().Be(employeeMock.Id);
        }
        [Fact]
        public void TestMapperProject()
        {
            //arrange
            var projectMock = _fixture.Build<Project>().Create();

            //act
            var result = _mapperConfig.Map<ProjectViewModel>(projectMock);

            //assert
            result.Id.Should().Be(projectMock.Id);
        }
        [Fact]
        public void TestMapperGroup()
        {
            //arrange
            var groupMock = _fixture.Build<Group>().Create();

            //act
            var result = _mapperConfig.Map<GroupViewModel>(groupMock);

            //assert
            result.Id.Should().Be(groupMock.Id);
        }
    }
}

