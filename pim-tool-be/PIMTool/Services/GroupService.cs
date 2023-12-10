using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Interfaces.Repositories;
using PIMTool.Core.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using PIMTool.Database;

namespace PIMTool.Services
{
    public class GroupService : IGroupService
    {
        private readonly IRepository<Group> _repository;
        private readonly PimContext _pimContext;

        public GroupService(IRepository<Group> repository, PimContext pimContext)
        {
            this._repository = repository;
            this._pimContext = pimContext;
        }
        public async Task AddAsync(Group group)
        {
            await _repository.AddAsync(group);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(Group group)
        {
            _repository.Delete(group);
            await _repository.SaveChangesAsync();
        }

        public async Task<Group> GetAsync(int id)
        {
            //return await _repository.GetAsync(id);
            return await _pimContext.Groups.Include(x=>x.GroupLeader).FirstOrDefaultAsync(x=>x.Id==id);
        }

        public async Task<IEnumerable<Group>> GetGroups()
        {
            //var entities = await _repository.Get().ToListAsync();
            var entities = await _pimContext.Groups.Include(x=>x.GroupLeader).ToListAsync();
            return entities;
        }

        public async Task UpdateAsync()
        {
            await _repository.SaveChangesAsync();
        }
    }
}
