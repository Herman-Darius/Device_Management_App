using Device_Management_App.Server.Models;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<bool> UpdateUserRoleAsync(int id, string newRole);
}