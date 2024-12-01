using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(PimContext pimContext) : base(pimContext)
        {
        }

        public async Task<IEnumerable<Employee>> SearchEmployeeByVisaAsync(string visa, CancellationToken cancellationToken = default)
        {
            IQueryable<Employee> query = base.Get();

            if (!string.IsNullOrEmpty(visa))
            {
                query = query.Where(x => x.Visa.Contains(visa));
            }

            return await query.ToListAsync(cancellationToken);
        }
    }
}