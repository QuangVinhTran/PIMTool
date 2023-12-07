using System.Data.Common;
using Application.Commons.ServiceResponse;
using Application.Exceptions;
using Application.Interfaces.Services;
using Application.ViewModels.EmployeeViewModels;
using AutoMapper;
using Domain.Entities;

namespace Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public EmployeeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<CreateEmployeeViewModel>> CreateEmployee(CreateEmployeeViewModel model)
        {
            var response = new ServiceResponse<CreateEmployeeViewModel>();

            var employee = _mapper.Map<Employee>(model);

            //employee.Version = _unitOfWork.GetTimeStamp();

            await _unitOfWork.EmployeeRepository.AddAsync(employee);

            var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
            if (!isSuccess)
            {
                throw new CreateException("Create fail !!!");
            }
            else
            {
                model.Id = employee.Id;
                response = ServiceResponse<CreateEmployeeViewModel>.SuccessResult(model, "Employee created successfully.");
            }
            return response;
        }

        public async Task<ServiceResponse<bool>> DeleteEmployee(int id)
        {
            var response = new ServiceResponse<bool>();

            var exist = await _unitOfWork.EmployeeRepository.GetAsync(id);
            if (exist is null)
            {
                throw new NotFoundException("Employee not found");
            }

            _unitOfWork.EmployeeRepository.Delete(exist);

            var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
            if (!isSuccess)
                throw new Exception();
            else
            {
                response.Success = isSuccess;
                response.Message = "Employee deleted successfully.";
            }
            return response;
        }

        public async Task<ServiceResponse<EmployeeViewModel>> GetEmployeeById(int id)
        {
            var response = new ServiceResponse<EmployeeViewModel>();

            var employee = await _unitOfWork.EmployeeRepository.GetAsync(id);

            if (employee is null)
            {
                throw new NotFoundException($"Employe with id {id} not found");
            }

            var employeeDto = _mapper.Map<EmployeeViewModel>(employee);
            response = ServiceResponse<EmployeeViewModel>.SuccessResult(employeeDto, "Employee retrieved successfully");

            return response;
        }

        public async Task<ServiceResponse<IEnumerable<EmployeeViewModel>>> GetEmployees()
        {
            var _response = new ServiceResponse<IEnumerable<EmployeeViewModel>>();

            var employees = await _unitOfWork.EmployeeRepository.GetAllAsync();

            if (employees is null)
            {
                throw new NotFoundException("Employees not found");
            }

            var entityDtos = employees.Select(x => _mapper.Map<EmployeeViewModel>(x)).ToList();

            _response = ServiceResponse<IEnumerable<EmployeeViewModel>>.SuccessResult(entityDtos, "Employees retrieved successfully");

            return _response;
        }

        public async Task<IEnumerable<EmployeeViewModel>> SearchEmployeeByVisaAsync(string visa, CancellationToken cancellationToken = default)
        {
            var employees = await _unitOfWork.EmployeeRepository.SearchEmployeeByVisaAsync(visa);

            var employeeDtos = employees.Select(x => _mapper.Map<EmployeeViewModel>(x)).ToList();

            return employeeDtos;
        }

        public async Task<ServiceResponse<UpdateEmployeeViewModel>> UpdateEmployee(int id, UpdateEmployeeViewModel model)
        {
            var response = new ServiceResponse<UpdateEmployeeViewModel>();

            var existingEmployee = await _unitOfWork.EmployeeRepository.GetAsync(id);

            if (existingEmployee is null)
            {
                throw new NotFoundException($"Employe with id {id} not found");
            }

            // Kiểm tra Version trước khi cập nhật
            // if (!model.Version.SequenceEqual(existingEmployee.Version))
            // {
            //     response = ServiceResponse<UpdateEmployeeViewModel>.ErrorResult("Conflict: Employee data has been updated by another user.");
            //     return response;
            // }

            var employeeUpdate = _mapper.Map(model, existingEmployee);
            //employeeUpdate.Version = _unitOfWork.GetTimeStamp();
            _unitOfWork.EmployeeRepository.Update(employeeUpdate);

            var updated = _mapper.Map<UpdateEmployeeViewModel>(employeeUpdate);

            var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
            if (!isSuccess)
            {
                throw new Exception();
            }
            else
            {
                response = ServiceResponse<UpdateEmployeeViewModel>.SuccessResult(updated, "Updating employee successfully");
            }

            return response;
        }
    }
}