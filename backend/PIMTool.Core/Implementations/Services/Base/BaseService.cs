using System;
using Autofac;
using PIMTool.Core.Interfaces.Services;
using PIMTool.Core.Interfaces.Services.Base;

namespace PIMTool.Core.Implementations.Services.Base;

public abstract class BaseService : IService
{
    protected Guid ServiceId { get; set; }
    private readonly ILifetimeScope _scope;

    protected BaseService(ILifetimeScope scope)
    {
        _scope = scope;
        ServiceId = Guid.NewGuid();
    }

    protected T Resolve<T>() where T : notnull
    {
        return _scope.Resolve<T>();
    }
}