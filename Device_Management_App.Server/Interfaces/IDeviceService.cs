using Device_Management_App.Server.Models;

public interface IDeviceService
{
    Task<IEnumerable<Device>> GetAllDevicesAsync();
    Task<Device?> GetDeviceByIdAsync(int id);
    Task<(bool Success, string Message, Device? Device)> CreateDeviceAsync(Device device);
    Task<bool> UpdateDeviceAsync(int id, Device device);
    Task<bool> DeleteDeviceAsync(int id);
    Task<bool> AssignDeviceAsync(int id, int userId);
    Task<bool> UnassignDeviceAsync(int id);
    Task<(bool Success, string Description)> GenerateAIDescriptionAsync(Device device);
}