using Application.Commons.ServiceResponse;
using Application.ViewModels.EmployeeViewModels;

namespace Application.Interfaces.Services
{
    public interface IEmployeeService
    {
        Task<ServiceResponse<IEnumerable<EmployeeViewModel>>> GetEmployees();
        Task<ServiceResponse<EmployeeViewModel>> GetEmployeeById(int id);
        Task<ServiceResponse<CreateEmployeeViewModel>> CreateEmployee(CreateEmployeeViewModel model);
        Task<ServiceResponse<UpdateEmployeeViewModel>> UpdateEmployee(int id, UpdateEmployeeViewModel model);
        Task<ServiceResponse<bool>> DeleteEmployee(int id);
        Task<IEnumerable<EmployeeViewModel>> SearchEmployeeByVisaAsync(string visa, CancellationToken cancellationToken = default);
    }
}