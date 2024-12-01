using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Interfaces.Repositories;
using PIMTool.Core.Interfaces.Services;

namespace PIMTool.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository<Employee> _repository;
        private readonly IRepository<ProjectEmployee> _projectEmployeeRepository;
        private readonly IRepository<Project> _projectRepository;

        public EmployeeService(IRepository<Employee> repository,
            IRepository<ProjectEmployee> projectEmployeeRepository,
            IRepository<Project> projectRepository)
        {
            _repository = repository;
            _projectEmployeeRepository = projectEmployeeRepository;
            _projectRepository = projectRepository;
        }

        public async Task AddAsync(Employee employee, CancellationToken cancellationToken = default)
        {
            var existedEmployeeList = _repository.Get().ToList();
            foreach (var emp in existedEmployeeList)
            {
                if (emp.Visa.Equals(employee.Visa))
                {
                    throw new Exception("Employee visa already in use!");
                }
            }
            await _repository.AddAsync(employee, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

        }

        public List<string> CheckNonExistentVisa(string list)
        {
            List<string> result = new();
            if (list != null && list.Length > 0)
            {
                string[] visaList = list.Split(",");

                foreach (var visa in visaList)
                {
                    var employee = _repository.FindByCondition(e => e.Visa.ToUpper().Equals(visa.ToUpper())).FirstOrDefault();
                    if (employee == null)
                    {
                        result.Add(visa.ToUpper());
                    }
                }
            }

            return result;
        }

        public IQueryable<Employee> Get()
        {
            return _repository.Get(null);
        }

        public async Task<Employee?> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _repository.GetAsync(id, null, cancellationToken);
            return entity;
        }

        public async Task<ICollection<Employee>> SearchEmployeeByProjId(int projectId, CancellationToken cancellationToken = default)
        {
            var employee = await _projectRepository.GetAsync(projectId, p => p.Employees, cancellationToken);
            if (employee == null)
            {
                throw new Exception("Project does not exist");
            }
            return employee.Employees;
        }

        public async Task Update(Employee employee)
        {
            var existedEmployeeList = _repository.Get().ToList();
            var updatingEmployee = _repository.FindByCondition(e => e.Id == employee.Id).First();
            existedEmployeeList.Remove(updatingEmployee);
            foreach (var emp in existedEmployeeList)
            {
                if (emp.Visa.Equals(employee.Visa))
                {
                    throw new Exception("Employee visa already in use!");
                }
            }


            _repository.ClearTrackers();
            await _repository.Update(employee);
            await _repository.SaveChangesAsync();
        }
    }
}
