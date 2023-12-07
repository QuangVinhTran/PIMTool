using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Interfaces.Repositories.Base;
using PIMTool.Core.Models.Dtos;

namespace PIMTool.Core.Interfaces.Repositories;

public interface IProjectRepository : IRepository<Project, Guid>
{
    void SetModified(Project project);
    void UpdateProject(Project project);
    Task<IQueryable<DtoProject>> GetProjectsOptimizedMappingAsync(Expression<Func<Project, bool>> specification);
    IEnumerable<DtoProject> CompileQuery(Expression<Func<
        DtoProject, bool>> whereExpr);

    void SoftDelete(Guid id);
    Task SoftDeleteAsync(Guid id);
}