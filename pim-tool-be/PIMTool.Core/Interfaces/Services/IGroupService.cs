using PIMTool.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMTool.Core.Interfaces.Services
{
    public interface IGroupService
    {
        Task AddAsync(Group group);
        Task DeleteAsync(Group group);
        Task<Group> GetAsync(int id);
        Task<IEnumerable<Group>> GetGroups();
        Task UpdateAsync(Group group);
    }
}
