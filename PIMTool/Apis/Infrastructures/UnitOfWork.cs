using Application;
using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructures
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PimContext _dbContext;
        private readonly IProjectRepository _projectRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IProjectEmployeeRepository _projectEmployeeRepository;
        private IDbContextTransaction _currentTransaction;

        public UnitOfWork(PimContext dbContext, IProjectRepository projectRepository, IEmployeeRepository employeeRepository, IGroupRepository groupRepository, IProjectEmployeeRepository projectEmployeeRepository)
        {
            _dbContext = dbContext;
            _projectRepository = projectRepository;
            _employeeRepository = employeeRepository;
            _groupRepository = groupRepository;
            _projectEmployeeRepository = projectEmployeeRepository;
        }   
        public bool IsTransactionActive => _currentTransaction != null;

        public IProjectRepository ProjectRepository => _projectRepository;

        public IEmployeeRepository EmployeeRepository => _employeeRepository;

        public IGroupRepository GroupRepository => _groupRepository;

        public IProjectEmployeeRepository ProjectEmployeeRepository => _projectEmployeeRepository;

        public async Task<int> SaveChangeAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            _currentTransaction ??= await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (_currentTransaction != null)
                {
                    await _dbContext.SaveChangesAsync(cancellationToken);
                    await _currentTransaction.CommitAsync(cancellationToken);
                }
            }
            catch
            {
                // Log or handle the exception as needed
                await RollbackTransactionAsync(cancellationToken);
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.RollbackAsync(cancellationToken);
                }
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
    }
}
