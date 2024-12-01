using Microsoft.EntityFrameworkCore;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Interfaces.Repositories;
using PIMTool.Core.Interfaces.Services;
using PIMTool.Database;

namespace PIMTool.Services
{
    public class ProjectEmployeeService : IProjectEmployeeService
    {
        private readonly PimContext _context;

        public ProjectEmployeeService(PimContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ProjectEmployee projectEmployee)
        {
            _context.ProjectEmployees.Add(projectEmployee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ProjectEmployee projectEmployee)
        {
            _context.ProjectEmployees.Remove(projectEmployee);
            await _context.SaveChangesAsync();
        }

        public async Task<ProjectEmployee> GetAsync(int EmployeeId, int ProjectId)
        {
            return await _context.ProjectEmployees.SingleOrDefaultAsync(x => x.EmployeeId == EmployeeId && x.ProjectId == ProjectId);
        }

        public async Task<IEnumerable<ProjectEmployee>> GetProjectEmployees()
        {
            return await _context.ProjectEmployees.ToListAsync();
        }

        public async Task<int[]> GetProjectMembers(int id)
        {
            return await _context.ProjectEmployees.Where(x => x.ProjectId == id).Select(x => x.EmployeeId).ToArrayAsync();
        }
    }
}
