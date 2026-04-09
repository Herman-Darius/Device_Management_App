using Device_Management_App.Server.Data;
using Device_Management_App.Server.Models;
using Microsoft.EntityFrameworkCore;

public class UserService : IUserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<bool> UpdateUserRoleAsync(int id, string newRole)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return false;

        user.Role = newRole;
        await _context.SaveChangesAsync();
        return true;
    }
}