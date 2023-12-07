using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Interfaces.Repositories;
using PIMTool.Database;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PIMTool.Repositories
{
    public interface IProjectRepository : IRepository<Project>
    {
        IEnumerable<Project> SearchProjectByProjectNumberOrNameOrCustomerAndStatus(string searchValue, string status);
        Task<Project> GetByProjectNumber(int projectNumber);
    }
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        private readonly DbSet<Project> _set;
        private readonly PimContext _pimContext;
        public ProjectRepository(PimContext pimContext) : base(pimContext)
        {
            _pimContext = pimContext;
            _set = _pimContext.Set<Project>();
        }

        public IEnumerable<Project> SearchProjectByProjectNumberOrNameOrCustomerAndStatus(string searchValue, string status)
        {
            var query = _pimContext.Projects.AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                var searchLower = searchValue.ToLower();
                query = query.Where(p =>
                    p.ProjectNumber.ToString().Contains(searchLower) ||
                    p.Name.ToLower().Contains(searchLower) ||
                    p.Customer.ToLower().Contains(searchLower)
                );
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(p => p.Status.ToLower() == status.ToLower());
            }

            return query.ToList();
        }

        public async Task<Project> GetByProjectNumber(int projectNumber)
        {
            var project = _pimContext.Projects.Where(p => p.ProjectNumber == projectNumber).Include(g => g.ProjectEmployees).FirstOrDefault();

            return project;
        }
    }
}
