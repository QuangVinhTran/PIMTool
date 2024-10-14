using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Domain.Objects;

namespace PIMTool.Core.Interfaces.Services
{
    public interface IProjectService
    {
        Task<Project?> GetAsync(int id);
        Task<IEnumerable<Project>> GetAllAsync();
        Task Create(Project project);
        Task Update(Project project);
        Task Delete(int id);
        //Task<IEnumerable<Project>> SearchProject(string? searchText, string searchStatus, string sortNumber, string sortName, string sortStatus, string sortCustomer, string sortStartDate);
        PagingDto SearchProjectV2(int pageSize, int pageIndex, string? searchText, string searchStatus, string sortNumber, string sortName, string sortStatus, string sortCustomer, string sortStartDate);
        //int TotalRecord();
        //IEnumerable<Project> PagingProject(int pageSize, int pageIndex, IEnumerable<Project> list);
        Task<bool> CheckExist(int projectNumber);
        Task RemoveRangeById(List<int> listRemoveId);
        Project GetProjectInclude(int projectId);
        //Task Add10000();
    }
}