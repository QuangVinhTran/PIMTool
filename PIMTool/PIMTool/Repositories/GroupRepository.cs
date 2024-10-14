using PIMTool.Entities;

namespace PIMTool.Repositories;

public class GroupRepository : BaseRepository<GroupEntity>
{
    public GroupRepository(AppDbContext context) : base(context)
    {
    }
}