using Device_Management_App.Server.Data;
using Device_Management_App.Server.Models;

public interface IAccountService
{
    Task<(bool Success, string Message)> RegisterAsync(User user);
    Task<(bool Success, string Message, object? Data)> LoginAsync(LoginDto login);
}