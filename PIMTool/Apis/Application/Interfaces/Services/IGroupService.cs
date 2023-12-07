using Application.Commons.ServiceResponse;
using Application.ViewModels.GroupViewModels;

namespace Application.Interfaces.Services
{
    public interface IGroupService
    {
        Task<ServiceResponse<IEnumerable<GroupViewModel>>> GetAllGroup();
        Task<ServiceResponse<GroupViewModel>> GetGroupById(int id);
        Task<ServiceResponse<CreateGroupViewModel>> CreateGroup(CreateGroupViewModel model);
        Task<ServiceResponse<UpdateGroupViewModel>> UpdateGroup(int id, UpdateGroupViewModel model);
        Task<ServiceResponse<bool>> DeleteGroup(int id);
    }
}