using Microsoft.EntityFrameworkCore;
using PIMTool.Entities;

namespace PIMTool.Repositories;

public class UserRepository : BaseRepository<UserEntity>
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<UserEntity?> GetEmployeeWithAuthentication(UserEntity entity)
    {
        var user = await _context.Users.FirstOrDefaultAsync(employee => employee.Username.Equals(entity.Username) &&
                                                                       employee.Password.Equals(entity.Password));
        return user;
    }
}