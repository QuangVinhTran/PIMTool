using System;
using Application;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoFixture;
using AutoMapper;
using Infrastructures;
using Infrastructures.Mappers;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Domain.Tests
{
    public class SetupTest : IDisposable
    {
        protected readonly IMapper _mapperConfig;
        protected readonly Fixture _fixture;
        protected readonly Mock<IUnitOfWork> _unitOfWorkMock;

        protected readonly Mock<IProjectRepository> _projectRepositoryMock;
        protected readonly Mock<IProjectService> _projectServiceMock;

        protected readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
        protected readonly Mock<IEmployeeService> _employeeServiceMock;

        protected readonly Mock<IGroupRepository> _groupRepositoryMock;
        protected readonly Mock<IGroupService> _groupServiceMock;

        protected readonly Mock<IProjectEmployeeRepository> _projectEmployeeRepositoryMock;
        protected readonly Mock<IProjectEmployeeService> _projectEmployeeServiceMock;

        protected readonly PimContext _dbContext;

        public SetupTest()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapperConfigurationsProfile());
            });
            _mapperConfig = mappingConfig.CreateMapper();

            _fixture = new Fixture();
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _projectRepositoryMock = new Mock<IProjectRepository>();
            _projectServiceMock = new Mock<IProjectService>();

            _employeeRepositoryMock = new Mock<IEmployeeRepository>();
            _employeeServiceMock = new Mock<IEmployeeService>();

            _groupRepositoryMock = new Mock<IGroupRepository>();
            _groupServiceMock = new Mock<IGroupService>();

            _projectEmployeeRepositoryMock = new Mock<IProjectEmployeeRepository>();
            _projectEmployeeServiceMock = new Mock<IProjectEmployeeService>();

            var options = new DbContextOptionsBuilder<PimContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _dbContext = new PimContext(options);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
