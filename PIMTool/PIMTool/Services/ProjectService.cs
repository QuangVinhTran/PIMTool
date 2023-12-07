using AutoMapper;
using PIMTool.Entities;
using PIMTool.Payload.Request.Paging;
using PIMTool.Repositories;

namespace PIMTool.Services;

public interface IProjectService
{
    public Task<ICollection<ProjectEntity>> GetAllProjects();
    public Task<ICollection<ProjectEntity>> GetAllProjectsWithPaging(PagingParameter pagingParameter);
    public Task<ProjectEntity?> GetProjectById(int id);
    public Task AddNewProject(ProjectEntity entity);
    public Task UpdateProject(ProjectEntity entity);
    public Task DeleteProject(ProjectEntity entity);
}
public class ProjectService : IProjectService
{
    private readonly ProjectRepository _repository;
    private readonly IMapper _mapper;

    public ProjectService(IMapper mapper, ProjectRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }
    public async Task<ICollection<ProjectEntity>> GetAllProjects()
    {
        var projects = await _repository.GetAllEntity();
        return projects.ToList();
    }

    public async Task<ICollection<ProjectEntity>> GetAllProjectsWithPaging(PagingParameter pagingParameter)
    {
        var projects = await _repository.GetAllEntityWithPaging(pagingParameter);
        return projects.ToList();
    }

    public async Task<ProjectEntity?> GetProjectById(int id)
    {
        return await _repository.GetEntityById(id);
    }

    public async Task AddNewProject(ProjectEntity entity)
    {
        await _repository.InsertNewEntity(entity);  
    }

    public async Task UpdateProject(ProjectEntity entity)
    {
        await _repository.UpdateEntity(entity);
    }

    public async Task DeleteProject(ProjectEntity entity)
    {
        await _repository.DeleteEntity(entity);
    }
}