using System.Reflection;
using Autofac;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Helpers;
using PIMTool.Core.Implementations.Repositories;
using PIMTool.Core.Interfaces.Repositories;
using PIMTool.Core.Interfaces.Services;
using PIMTool.Core.Mappings.AutoMapper;

namespace PIMTool.Test;

public abstract class BaseTest
{
    private ILifetimeScope Scope { get; set; }
    protected readonly Guid groupId = Guid.Parse("40939cec-a167-4004-86de-0d7d846ec2b4");
    protected readonly Guid employeeId = Guid.Parse("2250635f-28ce-4f7c-aa70-53e3f3b292c8");
    protected readonly Guid userId = Guid.Parse("c1783e58-94a4-4094-8dcb-08dbdaabe2ab");
    protected readonly Guid projectId = Guid.Parse("a1cfbc96-89f4-4794-9753-00001d4e985d");
    protected readonly Guid plannedProjectId = Guid.Parse("16766777-8e5d-470d-9afd-0000af33884b");
    
    [SetUp]
    public void Setup()
    {
        var containerBuilder = new ContainerBuilder();

        #region Register DI

        #region Register Services
        
        containerBuilder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(IProjectRepository))!)
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        containerBuilder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(IProjectService))!)
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
        
        containerBuilder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(IGroupRepository))!)
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        containerBuilder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(IGroupService))!)
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
        
        containerBuilder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(IPIMUserRepository))!)
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
        
        containerBuilder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(IRefreshTokenRepository))!)
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
        
        containerBuilder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(IJwtService))!)
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        containerBuilder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(IAuthenticationService))!)
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
        
        containerBuilder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(IEmployeeRepository))!)
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
        
        containerBuilder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(IEmployeeService))!)
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Development.json")
            .Build();
        
        
        containerBuilder.RegisterInstance<IConfiguration>(configuration);
        
        var config = new MapperConfiguration(AppModelMapper.MappingDto);
        var mapper = config.CreateMapper();
        containerBuilder.RegisterInstance(mapper).As<IMapper>();
        
        #endregion

        #region Register Dbcontext
        
        var dbOptions = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        var dbContext = new AppDbContext(dbOptions);
        dbContext.Database.EnsureCreated();
        containerBuilder.RegisterInstance(dbContext).As<IAppDbContext>();
        
        #endregion
        
        #endregion

        SeedDataIfNeeded(dbContext);
        var builtContainer = containerBuilder.Build();
        Scope = builtContainer.BeginLifetimeScope();
    }

    [TearDown]
    public void TearDown()
    {
        Scope.Dispose();
    }

    protected T ResolveService<T>() where T : notnull
    {
        return Scope.Resolve<T>();
    }
    
    private void SeedDataIfNeeded(IAppDbContext dbContext)
    {
        var projectSet = dbContext.CreateSet<Project>();
        var groupSet = dbContext.CreateSet<Group>();
        var employeeSet = dbContext.CreateSet<Employee>();
        var userSet = dbContext.CreateSet<PIMUser>();

        if (projectSet.Any()) return;

        #region Seed users

        var user = new PIMUser()
        {
            Id = userId,
            Email = "pimuser@pimtool.com",
            Password = EncryptionHelper.Encrypt("password"),
            FirstName = "Mock",
            LastName = "User",
            Role = "Employee"
        };
        
        userSet.Add(user);

        #endregion
        
        #region Seed employees

        var employee = new Employee()
        {
            Id = employeeId,
            FirstName = "Mock",
            LastName = "Employee",
            BirthDate = DateTime.Now.Subtract(TimeSpan.FromDays(365 * 20)),
            Visa = "MOCK"
        };

        employeeSet.Add(employee);

        #endregion
        
        #region Seed groups

        var group = new Group()
        {
            Id = groupId,
            Name = "Mock group",
            LeaderId = employeeId
        };
        
        groupSet.Add(group);
        
        #endregion
        
        #region Seed projects
        
        for (var i = 1; i < 10; i++)
        {
            var project = new Project
            {
                Id = Guid.NewGuid(),
                GroupId = group.Id,
                ProjectNumber = i,
                Name = "Project " + i,
                Customer = "Customer " + i,
                Status = "NEW",
                StartDate = DateTime.Now.Subtract(TimeSpan.FromDays(7)),
                EndDate = DateTime.Now.AddDays(7),
                Version = 0,
                CreatedAt = DateTime.Now,
                CreatedBy = Guid.Empty,
                IsDeleted = false,
                UpdatedAt = DateTime.Now,
                UpdatedBy = Guid.Empty,
                Employees = new List<Employee> { employee }
            };
            projectSet.Add(project);
        }
        
        var deletedProject = new Project()
        {
            Id = Guid.NewGuid(),
            GroupId = group.Id,
            ProjectNumber = 100,
            Name = "Project 100",
            Customer = "Customer 100",
            Status = "NEW",
            StartDate = DateTime.Now.Subtract(TimeSpan.FromDays(1)),
            EndDate = DateTime.Now,
            Version = 0,
            CreatedAt = DateTime.Now,
            CreatedBy = Guid.Empty,
            IsDeleted = true,
            UpdatedAt = DateTime.Now,
            UpdatedBy = Guid.Empty
        };
        
        var projectToUpdate = new Project()
        {
            Id = projectId,
            GroupId = group.Id,
            ProjectNumber = 200,
            Name = "Project 200",
            Customer = "Customer 200",
            Status = "NEW",
            StartDate = DateTime.Now.Subtract(TimeSpan.FromDays(1)),
            EndDate = DateTime.Now,
            Version = 0,
            CreatedAt = DateTime.Now,
            CreatedBy = Guid.Empty,
            IsDeleted = false,
            UpdatedAt = DateTime.Now,
            UpdatedBy = Guid.Empty
        };

        var plannedProject = new Project()
        {
            Id = plannedProjectId,
            GroupId = group.Id,
            ProjectNumber = 300,
            Name = "Project 300",
            Customer = "Customer 300",
            Status = "PLA",
            StartDate = DateTime.Now.Subtract(TimeSpan.FromDays(1)),
            EndDate = DateTime.Now,
            Version = 0,
            CreatedAt = DateTime.Now,
            CreatedBy = Guid.Empty,
            IsDeleted = false,
            UpdatedAt = DateTime.Now,
            UpdatedBy = Guid.Empty
        };
        
        projectSet.Add(deletedProject);
        projectSet.Add(projectToUpdate);
        projectSet.Add(plannedProject);
        dbContext.SaveChanges();
        
        #endregion
    }
}