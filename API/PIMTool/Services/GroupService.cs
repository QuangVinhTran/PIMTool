using Microsoft.EntityFrameworkCore;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Exceptions;
using PIMTool.Core.Interfaces.Repositories;
using PIMTool.Core.Interfaces.Services;
using PIMTool.Dtos;

namespace PIMTool.Services
{
    public class GroupService:IGroupService
    {
        private readonly IRepository<Group> _repository;

        public GroupService(IRepository<Group> repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(Group group, CancellationToken cancellationToken = default)
        {
            await _repository.AddAsync(group, cancellationToken);
            await _repository.SaveChangesAsync();
        }

        public IQueryable<Group> Get() 
        {
            return _repository.Get().Include(g => g.Employees);
        }

        public async Task<Group?> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                Group group = await _repository.GetAsync(id);
                if (group == null)
                {
                    throw new BusinessException($"Group with ID {id} not found.");
                }

                return group;
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Business error: {ex.Message}", ex);
            }
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _repository.SaveChangesAsync();
        }


    }
}
