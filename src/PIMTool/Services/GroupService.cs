using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Interfaces.Repositories;
using PIMTool.Core.Interfaces.Services;

namespace PIMTool.Services
{
    public class GroupService : IGroupService
    {
        private readonly IRepository<Group> _groupRepository;

        public GroupService(IRepository<Group> groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public IQueryable<Group> GetAll()
        {
            return _groupRepository.Get();
        }
    }
}
