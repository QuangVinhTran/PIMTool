using Microsoft.EntityFrameworkCore;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Exceptions;
using PIMTool.Core.Interfaces.Repositories;
using PIMTool.Core.Interfaces.Services;
using PIMTool.Database;
using PIMTool.Dtos;
using PIMTool.Repositories;

namespace PIMTool.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _repository;
        private readonly IRepository<Group> _groupRepository;
        private readonly IRepository<Employee> _employeeRepository;
        public ProjectService(IProjectRepository repository, IRepository<Group> groupRepository)
        {
            _repository = repository;
            _groupRepository = groupRepository;
        }

        public async Task<Project?> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _repository.GetAsync(id, cancellationToken);
            return entity;
        }

        public async Task AddAsync(Project entity, CancellationToken cancellationToken = default)
        {
            try
            {
                Group group = await _groupRepository.GetAsync(entity.GroupId);
                if (group == null)
                {
                    throw new BusinessException("Group does not exist");
                }

                Project project = await _repository.GetByProjectNumber(entity.ProjectNumber);
                if (project != null)
                {
                    throw new BusinessException($"Project number {project.ProjectNumber} already exist");
                }
                entity.Group = group;

                await _repository.AddAsync(entity, cancellationToken);
                await _repository.SaveChangesAsync(cancellationToken);
            } 
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        public IQueryable<Project> Get()
        {
            return _repository.Get();
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _repository.SaveChangesAsync(cancellationToken);
        }

        public IQueryable<Project> SearchProjectByProjectNumberOrNameOrCustomerAndStatus(string searchValue, string status, CancellationToken cancellationToken = default)
        {
            return _repository.SearchProjectByProjectNumberOrNameOrCustomerAndStatus(searchValue, status);
        }

        public async Task<Project> GetByProjectNumber(int projectNumber, CancellationToken cancellationToken = default)
        {
            return await _repository.GetByProjectNumber(projectNumber);
        }

        public async Task DeleteProjects(Project[] projects)
        {
            _repository.Delete(projects);
        }

        public async Task UpdateProject(int projectNumber, Project newProject)
        {
            try
            {
                Project existingProject = await _repository.GetByProjectNumber(projectNumber);
                if (existingProject == null)
                {
                    throw new BusinessException($"Project {projectNumber} not found.");
                }

                if (!newProject.Version.SequenceEqual(existingProject.Version))
                {
                    throw new BusinessException("Concurrency conflict. The project has been modified by another user.");
                }
                existingProject.ProjectNumber = newProject.ProjectNumber;
                existingProject.Name = newProject.Name;
                existingProject.Customer = newProject.Customer;
                existingProject.StartDate = newProject.StartDate;
                existingProject.EndDate = newProject.EndDate;
                existingProject.Status = newProject.Status;
                existingProject.ProjectEmployees = newProject.ProjectEmployees;

                Group group = await _groupRepository.GetAsync(newProject.GroupId);
                if(group == null)
                {
                    throw new BusinessException($"Group {newProject.GroupId} does not exist");
                }
                existingProject.Group = group;

                await _repository.UpdateProject(existingProject);
                await _repository.SaveChangesAsync();
            } 
            catch (DbUpdateConcurrencyException ex)
            {
                throw new BusinessException("Concurrency conflict. The project has been modified by another user.");
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
            
        }
    }
}