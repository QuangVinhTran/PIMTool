using System.Linq.Dynamic.Core;
using Application.Commons;
using Application.Commons.ServiceResponse;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.ViewModels.EmployeeViewModels;
using Application.ViewModels.ProjectViewModels;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProjectService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public bool IsProjectNumberExists(int projectNumber)
        {
            return _unitOfWork.ProjectRepository.IsProjectNumberExists(projectNumber);
        }

        public async Task<ServiceResponse<CreateProjectViewModel>> CreateProject(CreateProjectViewModel model, CancellationToken cancellationToken)
        {
            var response = new ServiceResponse<CreateProjectViewModel>();

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                if (!model.IsEndDateValid())
                    throw new ValidationException("Invalid end date. End date must be greater than start date !");
                if (_unitOfWork.ProjectRepository.IsProjectNumberExists(model.ProjectNumber))
                    throw new ProjectNumberAlreadyExistsException("Project number already exists");

                var project = _mapper.Map<Project>(model);

                await _unitOfWork.ProjectRepository.AddAsync(project, cancellationToken);

                var isAddProject = await _unitOfWork.SaveChangeAsync(cancellationToken) > 0;
                if (!isAddProject) throw new CreateException("Create fail !!!");

                model.Id = project.Id;

                var employeeIdsToRemove = new List<int>();
                var employeeIdAdded = new List<int>();

                if (model.SelectedEmployeeId.Count() != 0)
                {
                    foreach (var employeeId in model.SelectedEmployeeId)
                    {
                        var existingEmployee = await _unitOfWork.EmployeeRepository.GetAsync(employeeId);
                        if (existingEmployee == null)
                        {
                            employeeIdsToRemove.Add(employeeId);
                            continue;
                        }

                        if (employeeIdAdded.Contains(employeeId))
                        {
                            continue;
                        }

                        var projectEmployee = new ProjectEmployee
                        {
                            ProjectId = model.Id,
                            EmployeeId = employeeId
                        };

                        employeeIdAdded.Add(employeeId);

                        await _unitOfWork.ProjectEmployeeRepository.AddAsync(projectEmployee);
                    }
                    // Xóa các employeeId không tồn tại khỏi collection
                    foreach (var employeeIdToRemove in employeeIdsToRemove)
                    {
                        model.SelectedEmployeeId.Remove(employeeIdToRemove);
                    }
                    var isAddProjectEmployee = await _unitOfWork.SaveChangeAsync(cancellationToken) > 0;
                    if (!isAddProjectEmployee) throw new CreateException("Cannot add employee into project");
                }

                await _unitOfWork.CommitTransactionAsync(cancellationToken);
                response = ServiceResponse<CreateProjectViewModel>.SuccessResult(model, "Project created successfully.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                response = ServiceResponse<CreateProjectViewModel>.ErrorResult(ex.Message);
            }
            finally
            {
                // Đảm bảo giao dịch sẽ được đóng ngay cả khi có ngoại lệ hay không
                if (_unitOfWork.IsTransactionActive)
                {
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                }
            }
            return response;
        }

        public async Task<ServiceResponse<bool>> DeleteProject(int id, CancellationToken cancellationToken = default)
        {
            var response = new ServiceResponse<bool>();

            var exist = await _unitOfWork.ProjectRepository.GetAsync(id);

            if (exist is null)
            {
                response = ServiceResponse<bool>.ErrorResult("Not found project");
                return response;
            }

            if (exist.Status != 0)
            {
                response = ServiceResponse<bool>.ErrorResult("Can only delete project has NEW status");
                return response;
            }

            _unitOfWork.ProjectRepository.Delete(exist);

            var isSuccess = false;

            while (!isSuccess)
            {
                try
                {
                    isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                    response.Success = isSuccess;
                    response.Message = "Projects deleted successfully.";
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    response = ServiceResponse<bool>.ErrorResult(ex.Message);
                    return response;
                }
            }

            return response;
        }
        public async Task<ServiceResponse<bool>> DeleteProjects(int[] idProjects, CancellationToken cancellationToken = default)
        {
            var response = new ServiceResponse<bool>();

            foreach (var id in idProjects)
            {
                var exist = await _unitOfWork.ProjectRepository.GetAsync(id);
                if (exist is null)
                {
                    response = ServiceResponse<bool>.ErrorResult("Not found project");
                    return response;
                }
                if (exist.Status != 0)
                {
                    response = ServiceResponse<bool>.ErrorResult("Can only delete project has NEW status");
                    return response;
                }
                _unitOfWork.ProjectRepository.Delete(exist);
            }

            var isSuccess = false;

            while (!isSuccess)
            {
                try
                {
                    isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                    response.Success = isSuccess;
                    response.Message = "Projects deleted successfully.";
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    response = ServiceResponse<bool>.ErrorResult(ex.Message);
                    return response;
                }
            }

            return response;
        }

        public async Task<ServiceResponse<IEnumerable<EmployeeViewModel>>> GetEmployeeInProject(int id, CancellationToken cancellationToken = default)
        {
            var response = new ServiceResponse<IEnumerable<EmployeeViewModel>>();

            var project = await _unitOfWork.ProjectRepository.GetAsync(id);

            if (project is null)
            {
                response = ServiceResponse<IEnumerable<EmployeeViewModel>>.ErrorResult($"Not found project with id {id}");
            }

            var employee = await _unitOfWork.ProjectRepository.GetEmployeInProjectAsync(id);
            var employeeDto = employee.Select(x => _mapper.Map<EmployeeViewModel>(x)).ToList();

            if (!employeeDto.Any())
            {
                response = ServiceResponse<IEnumerable<EmployeeViewModel>>.SuccessResult(employeeDto, "Project not have employees");
                return response;
            }
            response = ServiceResponse<IEnumerable<EmployeeViewModel>>.SuccessResult(employeeDto, "Employee in Project retrieved successfully");

            return response;
        }

        public async Task<ServiceResponse<ProjectViewModel>> GetProjectById(int id, CancellationToken cancellationToken = default)
        {
            var response = new ServiceResponse<ProjectViewModel>();

            var project = await _unitOfWork.ProjectRepository.GetAsync(id);

            if (project is null)
            {
                throw new NotFoundException($"Project with {id} not found !");
            }

            var projectDto = _mapper.Map<ProjectViewModel>(project);
            response = ServiceResponse<ProjectViewModel>.SuccessResult(projectDto, "Project retrieved successfully");

            return response;
        }

        public async Task<ServiceResponse<IEnumerable<ProjectViewModel>>> GetProjects()
        {
            var _response = new ServiceResponse<IEnumerable<ProjectViewModel>>();

            var projects = await _unitOfWork.ProjectRepository.GetAllAsync();

            if (projects is null)
            {
                throw new NotFoundException("Projects not found ");
            }

            var projectDtos = projects.Select(x => _mapper.Map<ProjectViewModel>(x)).ToList();

            _response = ServiceResponse<IEnumerable<ProjectViewModel>>.SuccessResult(projectDtos, "Projects retrieved successfully");

            return _response;
        }

        public async Task<Pagination<ProjectViewModel>> GetProjectsByStatus(StatusEnum status, int pageIndex = 0, int pageSize = 5)
        {
            var filteredProjects = await _unitOfWork.ProjectRepository.GetProjectsByStatusAsync(status);

            if (filteredProjects is null) throw new NotFoundException("Filtered projects is null");

            // Thực hiện phân trang cho danh sách đã lọc
            var paginatedProjects = filteredProjects.Skip(pageIndex * pageSize).Take(pageSize).ToList();

            var result = new Pagination<ProjectViewModel>
            {
                TotalItemsCount = filteredProjects.Count(),
                PageSize = pageSize,
                PageIndex = pageIndex,
                Items = _mapper.Map<List<ProjectViewModel>>(paginatedProjects)
            };

            return result;
        }

        public async Task<Pagination<ProjectViewModel>> SearchProjectAsync(string searchTerm, int pageIndex = 0, int pageSize = 5, CancellationToken cancellationToken = default)
        {
            var filteredProjects = await _unitOfWork.ProjectRepository.SearchProjectAsync(searchTerm);

            if (filteredProjects is null) throw new NotFoundException("Filtered projects is null");

            // Thực hiện phân trang cho danh sách đã lọc
            var paginatedProjects = filteredProjects.Skip(pageIndex * pageSize).Take(pageSize).ToList();

            var result = new Pagination<ProjectViewModel>
            {
                TotalItemsCount = filteredProjects.Count(),
                PageSize = pageSize,
                PageIndex = pageIndex,
                Items = _mapper.Map<List<ProjectViewModel>>(paginatedProjects)
            };

            return result;
        }

        public async Task<Pagination<ProjectViewModel>> FilterProjectsAsync(string searchTerm, StatusEnum? status, int pageIndex, int pageSize = 5, CancellationToken cancellationToken = default)
        {
            var filteredProjects = await _unitOfWork.ProjectRepository.FilterProjectsAsync(searchTerm, status);

            if (filteredProjects is null) throw new NotFoundException("Filtered project is null");

            var paginatedProjects = filteredProjects.Skip(pageIndex * pageSize).Take(pageSize).ToList();

            var result = new Pagination<ProjectViewModel>
            {
                TotalItemsCount = filteredProjects.Count(),
                PageSize = pageSize,
                PageIndex = pageIndex,
                Items = _mapper.Map<List<ProjectViewModel>>(paginatedProjects)
            };

            return result;
        }

        public async Task<ServiceResponse<UpdateProjectViewModel>> UpdateProject(int id, UpdateProjectViewModel model, CancellationToken cancellationToken = default)
        {
            var response = new ServiceResponse<UpdateProjectViewModel>();

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var existingProject = await _unitOfWork.ProjectRepository.GetAsync(id, cancellationToken);

                if (existingProject is null)
                {
                    throw new NotFoundException($"Project with id {id} not found ");
                }

                if (existingProject.ProjectNumber != model.ProjectNumber)
                {
                    response = ServiceResponse<UpdateProjectViewModel>.ErrorResult("Project Number cannot be changed");
                    return response;
                }

                // Lấy danh sách ProjectEmployee hiện tại cho project
                var currentProjectEmployees = await _unitOfWork.ProjectEmployeeRepository.SearchProjectEmployeeById(id);

                var currentEmployeeIds = new List<int>();

                foreach (var projectEmployee in currentProjectEmployees)
                {
                    currentEmployeeIds.Add(projectEmployee.EmployeeId);
                }

                var selectedEmployeeIdToRemove = currentEmployeeIds.Except(model.SelectedEmployeeId).ToList();

                if (selectedEmployeeIdToRemove.Count() > 0)
                {
                    await _unitOfWork.ProjectEmployeeRepository.Delete(selectedEmployeeIdToRemove);
                }

                var newEmployeeId = model.SelectedEmployeeId.Except(currentEmployeeIds).ToList();

                if (newEmployeeId.Count() != 0)
                {
                    foreach (var employeeId in newEmployeeId)
                    {
                        var projectEmployee = new ProjectEmployee
                        {
                            ProjectId = id,
                            EmployeeId = employeeId
                        };
                        await _unitOfWork.ProjectEmployeeRepository.AddAsync(projectEmployee);
                    }
                }

                // Cập nhật thông tin của project (giả sử _mapper.Map cũng được sử dụng ở đây)
                var projectUpdate = _mapper.Map(model, existingProject);
                _unitOfWork.ProjectRepository.Update(projectUpdate, cancellationToken);

                var isSuccess = await _unitOfWork.SaveChangeAsync(cancellationToken) > 0;

                if (!isSuccess)
                {
                    response = ServiceResponse<UpdateProjectViewModel>.ErrorResult("Failed to update project.");
                    return response;
                }

                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                var updated = _mapper.Map<UpdateProjectViewModel>(projectUpdate);

                response = ServiceResponse<UpdateProjectViewModel>.SuccessResult(updated, "Updating project successfully");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                response = ServiceResponse<UpdateProjectViewModel>.ErrorResult(ex.Message);
            }
            finally
            {
                if (_unitOfWork.IsTransactionActive)
                {
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                }
            }
            return response;
        }

        public async Task<Pagination<ProjectViewModel>> GetProjectPagingAsync(int pageIndex = 0, int pageSize = 5, CancellationToken cancellationToken = default)
        {
            var projects = await _unitOfWork.ProjectRepository.ToPagination(pageIndex, pageSize);

            if (projects is null) throw new NotFoundException("Projects is null");

            var result = _mapper.Map<Pagination<ProjectViewModel>>(projects);

            return result;
        }
    }
}


/*

    ban dau currentProjectEmployeeId la 31, 1033, 32
    ban dau co selectedEmployeeId la: 31, 1033, 7 => nghia la xoa 32 roi
                                                 nghia la 7 chua co => add vo database thang 7 => done

    => them 1 list selectedEmployeeIdToRemove => phai add thang 32 vo

*/