using System.Data.Common;
using Application.Commons.ServiceResponse;
using Application.Interfaces.Services;
using Application.ViewModels.GroupViewModels;
using AutoMapper;
using Domain.Entities;

namespace Application.Services
{
    public class GroupService : IGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GroupService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<CreateGroupViewModel>> CreateGroup(CreateGroupViewModel model)
        {
            var response = new ServiceResponse<CreateGroupViewModel>();

            var group = _mapper.Map<Group>(model);

            //group.Version = _unitOfWork.GetTimeStamp();

            await _unitOfWork.GroupRepository.AddAsync(group);

            var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
            if (isSuccess)
            {
                model.Id = group.Id;
                response = ServiceResponse<CreateGroupViewModel>.SuccessResult(model, "Group created successfully.");
            }
            else
            {
                response = ServiceResponse<CreateGroupViewModel>.ErrorResult("Cannot create group");
            }

            return response;
        }

        public async Task<ServiceResponse<bool>> DeleteGroup(int id)
        {
            var response = new ServiceResponse<bool>();

            var exist = await _unitOfWork.GroupRepository.GetAsync(id);
            if (exist == null)
            {
                response = ServiceResponse<bool>.NotFoundResult("Group");
                return response;
            }


            _unitOfWork.GroupRepository.Delete(exist);

            var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
            if (isSuccess)
            {
                response.Success = isSuccess;
                response.Message = "Group deleted successfully.";
            }
            else
            {
                response = ServiceResponse<bool>.ErrorResult("Group cannot be deleted");
            }

            return response;
        }

        public async Task<ServiceResponse<IEnumerable<GroupViewModel>>> GetAllGroup()
        {
            var _response = new ServiceResponse<IEnumerable<GroupViewModel>>();

            var groups = await _unitOfWork.GroupRepository.GetAllAsync();

            if (groups == null)
            {
                _response = ServiceResponse<IEnumerable<GroupViewModel>>.NotFoundResult("Groups");
                return _response;
            }

            var groupDtos = groups.Select(x => _mapper.Map<GroupViewModel>(x)).ToList();

            _response = ServiceResponse<IEnumerable<GroupViewModel>>.SuccessResult(groupDtos, "Groups retrieved successfully");

            return _response;
        }

        public async Task<ServiceResponse<GroupViewModel>> GetGroupById(int id)
        {
            var response = new ServiceResponse<GroupViewModel>();

            var group = await _unitOfWork.GroupRepository.GetAsync(id);

            if (group == null)
            {
                response = ServiceResponse<GroupViewModel>.NotFoundResult($"Group with id {id}");
                return response;
            }

            var groupDto = _mapper.Map<GroupViewModel>(group);
            response = ServiceResponse<GroupViewModel>.SuccessResult(groupDto, "Group retrieved successfully");

            return response;
        }

        public async Task<ServiceResponse<UpdateGroupViewModel>> UpdateGroup(int id, UpdateGroupViewModel model)
        {
            var response = new ServiceResponse<UpdateGroupViewModel>();

            var existingGroup = await _unitOfWork.GroupRepository.GetAsync(id);

            if (existingGroup == null)
            {
                response = ServiceResponse<UpdateGroupViewModel>.ErrorResult($"Group with ID {id} not found.");
                return response;
            }

            // Kiểm tra Version trước khi cập nhật
            // if (!model.Version.SequenceEqual(existingGroup.Version))
            // {
            //     response = ServiceResponse<UpdateGroupViewModel>.ErrorResult("Conflict: Group data has been updated by another user.");
            //     return response;
            // }

            var groupUpdate = _mapper.Map(model, existingGroup);
            //groupUpdate.Version = _unitOfWork.GetTimeStamp();
            _unitOfWork.GroupRepository.Update(groupUpdate);

            var updated = _mapper.Map<UpdateGroupViewModel>(groupUpdate);

            var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
            if (isSuccess)
            {
                response = ServiceResponse<UpdateGroupViewModel>.SuccessResult(updated, "Updating group successfully");
            }
            else
            {
                response = ServiceResponse<UpdateGroupViewModel>.ErrorResult("Cannot save update information into database.");
            }

            return response;
        }
    }
}