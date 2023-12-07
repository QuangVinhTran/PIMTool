using System;
using System.Threading.Tasks;

namespace PIMTool.Core.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    void Commit();
    Task CommitAsync();
    void Rollback();
    Task RollbackAsync();
}