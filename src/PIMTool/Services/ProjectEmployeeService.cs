using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Interfaces.Repositories;
using PIMTool.Core.Interfaces.Services;

namespace PIMTool.Services
{
    public class ProjectEmployeeService : IProjectEmployeeService
    {
        private readonly IRepository<ProjectEmployee> _repository;
        private readonly IRepository<Project> _projectRepository;
        private readonly IRepository<Employee> _employeeRepository;
        public ProjectEmployeeService(IRepository<ProjectEmployee> repository,
            IRepository<Project> projectRepository,
            IRepository<Employee> employeeRepository)
        {
            _repository = repository;
            _projectRepository = projectRepository;
            _employeeRepository = employeeRepository;
        }
        public async Task AddAsync(ProjectEmployee projectEmployee, CancellationToken cancellationToken = default)
        {
            var project = await _projectRepository.GetAsync(projectEmployee.ProjectId,null, cancellationToken);
            if (project == null)
            {
                throw new Exception("Project does not exist");
            }

            var employee = await _employeeRepository.GetAsync(projectEmployee.EmployeeId,null, cancellationToken);
            if (employee == null)
            {
                throw new Exception("Employee does not exist");
            }

            await _repository.AddAsync(projectEmployee);
            await _repository.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<ProjectEmployee> projectEmployee, CancellationToken cancellationToken = default)
        {
            await _repository.AddRangeAsync(projectEmployee);
            await _repository.SaveChangesAsync();   
        }

        public async void DeleteRange(params ProjectEmployee[] projectEmployees)
        {
            await _repository.DeleteRange(projectEmployees);
        }

        public IQueryable<ProjectEmployee> GetAll()
        {
            return _repository.Get();
        }

        public IQueryable<ProjectEmployee?> GetAllEmployeesByProjectId(int projectId)
        {
            var project =  _projectRepository.GetAsync(projectId);
            if (project.Result == null)
            {
                throw new Exception("Project does not exist");
            }

            return _repository.FindByCondition(pe => pe.ProjectId == projectId);
        }
    }

    }

