using PIMTool.Entities;

namespace PIMTool.Repositories;

public class ProjectRepository : BaseRepository<ProjectEntity>
{
    public ProjectRepository(AppDbContext context) : base(context)
    {
    }
}