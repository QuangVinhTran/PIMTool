using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Interfaces.Repositories;
using PIMTool.Core.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using PIMTool.Database;

namespace PIMTool.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IRepository<Project> _repository;
        private readonly PimContext _pimContext;

        public ProjectService(IRepository<Project> repository, PimContext pimContext)
        {
            _repository = repository;
            this._pimContext = pimContext;
        }

        public async Task<Project> AddAsync(Project project)
        {
            try
            {
                await _repository.AddAsync(project);
                await _repository.SaveChangesAsync();
                return project;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteAsync(Project project)
        {
            var projectWithEmployees = await _pimContext.Projects.Include(x => x.ProjectEmployees).FirstOrDefaultAsync(x => x.Id == project.Id);
            _repository.Delete(projectWithEmployees);
            await _repository.SaveChangesAsync();
        }

        public async Task<Project> GetAsync(int id)
        {
            //var entity = await _repository.GetAsync(id, cancellationToken);
            var entity = await _pimContext.Projects.Include(x => x.Group.GroupLeader).FirstOrDefaultAsync(x => x.Id == id);
            return entity;
        }

        public async Task<Project> GetByProjectNumber(int projectNumber)
        {
            return await _pimContext.Projects.Include(x => x.Group.GroupLeader).FirstOrDefaultAsync(x => x.ProjectNumber == projectNumber);
        }

        public async Task<IEnumerable<Project>> GetProjects()
        {
            var entities = await _pimContext.Projects.Include(x => x.Group.GroupLeader).OrderByDescending(y => y.Id).ToListAsync();
            return entities;
        }

        public async Task<IEnumerable<Project>> GetProjectsPagination(int skip, int limit)
        {
            var entities = await _pimContext.Projects
                            .Include(x => x.Group.GroupLeader)
                            .OrderBy(y => y.ProjectNumber)
                            .Skip(skip)
                            .Take(limit)
                            .ToListAsync();
            return entities;
        }

        public async Task<IEnumerable<Project>> Search(string? searchText, int? status)
        {
            if (!string.IsNullOrEmpty(searchText) && status.HasValue)
            {
                return await _pimContext.Projects.Include(x => x.Group.GroupLeader)
                    .Where(x =>
                        (
                            x.Name.ToLower().Contains(searchText.Trim().ToLower())
                            || x.Customer.ToLower().Contains(searchText.Trim().ToLower())
                            || x.ProjectNumber.ToString().Contains(searchText.Trim().ToLower())
                        )
                        && x.Status == (Core.Domain.Enums.Status)status).ToListAsync();
            }
            else if (!string.IsNullOrEmpty(searchText) && !status.HasValue)
            {
                return await _pimContext.Projects.Include(x => x.Group.GroupLeader)
                  .Where(x => x.Name.ToLower().Contains(searchText.Trim().ToLower())
                      || x.Customer.ToLower().Contains(searchText.Trim().ToLower())
                      || x.ProjectNumber.ToString().Contains(searchText.Trim().ToLower())).ToListAsync();
            }
            return await _pimContext.Projects.Include(x => x.Group.GroupLeader).Where(x => x.Status == (Core.Domain.Enums.Status)status).ToListAsync();
        }

        public async Task<IEnumerable<Project>> SearchWithPagination(string searchText, int? status, int skip, int limit)
        {
            IEnumerable<Project> searchResult;
            if (!string.IsNullOrEmpty(searchText) && status.HasValue)
            {
                searchResult = await _pimContext.Projects.Include(x => x.Group.GroupLeader)
                    .Where(x =>
                        (
                            x.Name.ToLower().Contains(searchText.Trim().ToLower())
                            || x.Customer.ToLower().Contains(searchText.Trim().ToLower())
                            || x.ProjectNumber.ToString().Contains(searchText.Trim().ToLower())
                        )
                        && x.Status == (Core.Domain.Enums.Status)status).ToListAsync();
            }
            else if (!string.IsNullOrEmpty(searchText) && !status.HasValue)
            {
                searchResult = await _pimContext.Projects.Include(x => x.Group.GroupLeader)
                  .Where(x => x.Name.ToLower().Contains(searchText.Trim().ToLower())
                      || x.Customer.ToLower().Contains(searchText.Trim().ToLower())
                      || x.ProjectNumber.ToString().Contains(searchText.Trim().ToLower())).ToListAsync();
            }
            else
            {
                searchResult = await _pimContext.Projects.Include(x => x.Group.GroupLeader).Where(x => x.Status == (Core.Domain.Enums.Status)status).ToListAsync();
            }
            var entities = searchResult
                            .OrderBy(y => y.ProjectNumber)
                            .Skip(skip)
                            .Take(limit);
            return entities;
        }

        public async Task UpdateAsync()
        {
            await _repository.SaveChangesAsync();
        }
    }
}