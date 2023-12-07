using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Domain.Objects;
using PIMTool.Core.Interfaces.Repositories;
using PIMTool.Core.Interfaces.Services;
using System.Linq;
using System.Linq.Expressions;

namespace PIMTool.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IRepository<Project> _projectRepository;
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<ProjectEmployee> _projectEmployeeRepository;

        public ProjectService(IRepository<Project> projectRepository,
            IRepository<Employee> employeeRepository,
            IRepository<ProjectEmployee> projectEmployeeRepository)
        {
            _projectRepository = projectRepository;
            _employeeRepository = employeeRepository;
            _projectEmployeeRepository = projectEmployeeRepository;
        }

        public async Task AddAsync(Project project, List<string> employeeVisaList, CancellationToken cancellationToken = default)
        {
            var checkedProject = _projectRepository.FindByCondition(p => p.ProjectNumber.Equals(project.ProjectNumber)).FirstOrDefault();
            if (checkedProject != null)
            {
                throw new Exception("Project number already exists!");
            }

            await _projectRepository.AddAsync(project, cancellationToken);
            await _projectRepository.SaveChangesAsync(cancellationToken);

            if (employeeVisaList.Count() > 0)
            {
                List<int> employeeId = new List<int>();

                foreach (var visa in employeeVisaList)
                {
                    Employee employee = _employeeRepository.FindByCondition(e => e.Visa.Equals(visa.ToUpper())).First();
                    employeeId.Add(employee.Id);
                }

                if(employeeId.Count() == 0)
                {
                    throw new Exception("Employees do not exist!");
                }

                List<ProjectEmployee> projectEmployees = new List<ProjectEmployee>();
                for (int i = 0; i < employeeId.Count; i++)
                {
                    ProjectEmployee projectEmployee = new ProjectEmployee
                    {
                        ProjectId = project.Id,
                        EmployeeId = employeeId[i]
                    };
                    projectEmployees.Add(projectEmployee);
                }

                await _projectEmployeeRepository.AddRangeAsync(projectEmployees);
                await _projectEmployeeRepository.SaveChangesAsync();
            }
        }
        public async Task Delete(Project project)
        {
            var projectEmployeeList = _projectEmployeeRepository.FindByCondition(pe => pe.ProjectId == project.Id).ToArray();
            await _projectEmployeeRepository.DeleteRange(projectEmployeeList);
            await _projectEmployeeRepository.SaveChangesAsync();

            await _projectRepository.Delete(project);
            await _projectRepository.SaveChangesAsync();
        }

        public async Task DeleteRange(params Project[] projects)
        {
            await _projectRepository.DeleteRange(projects);
            await _projectRepository.SaveChangesAsync();
        }

        public IQueryable<Project> Get(ProjectParameters projectParameters)
        {
            Expression<Func<Project, bool>>? expression = null;
            Expression<Func<Project, bool>>? filterProject = null;
            Expression<Func<Project, bool>>? filterProjectStatus = null;
            string filterParameters = null;
            if (projectParameters.FilterParameters != null)
            {
                filterParameters = projectParameters.FilterParameters.Trim().ToLower();
            }

            if (filterParameters != null)
            {
                filterProject = p => p.Name.ToLower().Equals(filterParameters) ||
                p.ProjectNumber.ToString().Equals(filterParameters) ||
                p.Customer.ToLower().Equals(filterParameters);
            }

            var status = projectParameters.Status;
            if (status != null)
            {
                filterProjectStatus = p => p.Status.ToString().Equals(status);
            }

            if (filterProject != null && filterProjectStatus != null)
            {
                expression = Expression.Lambda<Func<Project, bool>>(
                            Expression.AndAlso(
                                filterProject.Body,
                                filterProjectStatus.Body
                            ),
                            filterProject.Parameters
                        );
            }
            else if (filterProject != null)
            {
                expression = filterProject;
            }
            else expression = filterProjectStatus;

            if (expression == null) 
            {
                var projects = _projectRepository.Get();
                var tempSize = (int)Math.Ceiling((decimal)projects.Count() / projectParameters.PagingParameters.PageSize);
                projectParameters.PagingParameters.TotalPage = tempSize;
                return projects.Skip((projectParameters.PagingParameters.PageNumber - 1) * projectParameters.PagingParameters.PageSize)
                    .Take(projectParameters.PagingParameters.PageSize);
            }

            var projectLists = _projectRepository.FindByCondition(expression);
            var tempPageSize = (int)Math.Ceiling((decimal)projectLists.Count()/ projectParameters.PagingParameters.PageSize);
            projectParameters.PagingParameters.TotalPage = tempPageSize;
            return projectLists.Skip((projectParameters.PagingParameters.PageNumber - 1) * projectParameters.PagingParameters.PageSize)
                .Take(projectParameters.PagingParameters.PageSize);

        }

        public async Task<Project?> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _projectRepository.GetAsync(id, p => p.Employees, cancellationToken);
            if (entity == null)
            {
                throw new Exception("Project does not exist!");
            }
            return entity;
        }

        public async Task Update(Project project, List<string> employeeVisaList)
        {

            ProjectEmployee[] existedEmployee = _projectEmployeeRepository.FindByCondition(pe => pe.ProjectId == project.Id).ToArray();
            await _projectEmployeeRepository.DeleteRange(existedEmployee);
            await _projectEmployeeRepository.SaveChangesAsync();

            if (employeeVisaList.Count > 0)
            {
                List<int> employeeId = new List<int>();

                foreach (var visa in employeeVisaList)
                {
                    Employee employee = _employeeRepository.FindByCondition(e => e.Visa.Equals(visa)).First();
                    employeeId.Add(employee.Id);
                }


                List<ProjectEmployee> projectEmployees = new List<ProjectEmployee>();
                for (int i = 0; i < employeeId.Count; i++)
                {
                    ProjectEmployee projectEmployee = new ProjectEmployee
                    {
                        ProjectId = project.Id,
                        EmployeeId = employeeId[i]
                    };
                    projectEmployees.Add(projectEmployee);
                }

                await _projectEmployeeRepository.AddRangeAsync(projectEmployees);
                await _projectEmployeeRepository.SaveChangesAsync();
            }

            var updatingProject = _projectRepository.FindByCondition(p => p.Id == project.Id).First();
            List<Project> existedProjectList = _projectRepository.Get().ToList();
            existedProjectList.Remove(updatingProject);
            _projectRepository.ClearTrackers();

            foreach (var existedProject in existedProjectList)
            {
                if (existedProject.ProjectNumber.Equals(project.ProjectNumber))
                    throw new Exception("Project number already existed");
            }

            await _projectRepository.Update(project);
            await _projectRepository.SaveChangesAsync();
        }
    }
}