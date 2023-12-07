using Application.Interfaces.Repositories;

namespace Application
{
    public interface IUnitOfWork
    {
        public IProjectRepository ProjectRepository { get; }
        public IEmployeeRepository EmployeeRepository { get; }
        public IGroupRepository GroupRepository { get; }
        public IProjectEmployeeRepository ProjectEmployeeRepository { get; }

        public bool IsTransactionActive { get; }
        Task<int> SaveChangeAsync(CancellationToken cancellationToken = default);

        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);


    }
}
