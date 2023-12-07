using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        public Task<IEnumerable<Employee>> SearchEmployeeByVisaAsync(string visa, CancellationToken cancellationToken = default);
    }
}