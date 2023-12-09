using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Infrastructures.Repositories
{
    public class GroupRepository : Repository<Group>, IGroupRepository
    {
        public GroupRepository(PimContext pimContext) : base(pimContext)
        {
        }
    }
}