using System.Runtime.CompilerServices;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PIMTool.Entities;

namespace PIMTool.Repositories;

public class EmployeeRepository : BaseRepository<EmployeeEntity>
{
    private readonly IMapper _mapper;
    public EmployeeRepository(AppDbContext context, IMapper mapper) : base(context)
    {
        _mapper = mapper;
    }

    public async Task UpdateEmployee(EmployeeEntity entity)
    {
        try
        {
            var employee = await _context.Employees.FindAsync(entity.Id);
            if (employee.Version > entity.Version) throw new DbUpdateConcurrencyException("Unable to save change!");
            employee.Version += 1;
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new DbUpdateConcurrencyException("Unable to save changes.");
        }
    }
}