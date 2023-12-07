using System;
using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Implementations.Repositories.Base;
using PIMTool.Core.Interfaces.Repositories;
using PIMTool.Core.Models.Dtos;

namespace PIMTool.Core.Implementations.Repositories;

public class ProjectRepository : Repository<Project, Guid>, IProjectRepository
{
    private readonly IMapper _mapper;
    public ProjectRepository(IAppDbContext appDbContext, IMapper mapper) : base(appDbContext)
    {
        _mapper = mapper;
    }

    public void SetModified(Project project)
    {
        _appDbContext.SetModified(project);
    }

    public void UpdateProject(Project project)
    {
        _appDbContext.Update(project);
    }

    public async Task<IQueryable<DtoProject>> GetProjectsOptimizedMappingAsync(Expression<Func<Project, bool>> specification)
    {
        return await Task.FromResult(_appDbContext.CreateSet<Project>().AsNoTracking().Where(specification).ProjectTo<DtoProject>(_mapper.ConfigurationProvider));
    }

    public IEnumerable<DtoProject> CompileQuery(Expression<Func<DtoProject, bool>> whereExpr)
    {
        var compiledQuery =
            EF.CompileQuery((AppDbContext context) => 
                context
                    .CreateSet<Project>()
                    .ProjectTo<DtoProject>(_mapper.ConfigurationProvider)
                    .Where(whereExpr)
                    .AsEnumerable());
        var dtoProjects = compiledQuery((AppDbContext)_appDbContext);
        return dtoProjects;
    }

    public void SoftDelete(Guid id)
    {
        var project = _appDbContext.CreateSet<Project>()
            .First(p => !p.IsDeleted && p.Id == id);
        project.IsDeleted = true;
        SetModified(project);
    }

    public async Task SoftDeleteAsync(Guid id)
    {
        var project = await _appDbContext.CreateSet<Project>()
            .FirstAsync(p => !p.IsDeleted && p.Id == id);
        project.IsDeleted = true;
        SetModified(project);
    }
}