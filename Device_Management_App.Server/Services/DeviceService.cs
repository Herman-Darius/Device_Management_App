using Device_Management_App.Server.Data;
using Device_Management_App.Server.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

public class DeviceService : IDeviceService
{
    private readonly AppDbContext _context;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public DeviceService(AppDbContext context, HttpClient httpClient, IConfiguration config)
    {
        _context = context;
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<IEnumerable<Device>> GetAllDevicesAsync() =>
        await _context.Devices.Include(d => d.AssignedUser).ToListAsync();

    public async Task<Device?> GetDeviceByIdAsync(int id) =>
        await _context.Devices.Include(d => d.AssignedUser).FirstOrDefaultAsync(d => d.Id == id);

    public async Task<(bool Success, string Message, Device? Device)> CreateDeviceAsync(Device device)
    {
        if (await _context.Devices.AnyAsync(d => d.Name.ToLower() == device.Name.ToLower()))
            return (false, "A device with this name already exists.", null);

        _context.Devices.Add(device);
        await _context.SaveChangesAsync();
        return (true, string.Empty, device);
    }

    public async Task<bool> UpdateDeviceAsync(int id, Device device)
    {
        if (id != device.Id) return false;
        _context.Entry(device).State = EntityState.Modified;
        try { await _context.SaveChangesAsync(); return true; }
        catch { return false; }
    }

    public async Task<bool> DeleteDeviceAsync(int id)
    {
        var device = await _context.Devices.FindAsync(id);
        if (device == null) return false;
        _context.Devices.Remove(device);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AssignDeviceAsync(int id, int userId)
    {
        var device = await _context.Devices.FindAsync(id);
        if (device == null) return false;
        device.AssignedUserId = userId;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UnassignDeviceAsync(int id)
    {
        var device = await _context.Devices.FindAsync(id);
        if (device == null) return false;
        device.AssignedUserId = null;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<(bool Success, string Description)> GenerateAIDescriptionAsync(Device device)
    {
        var model = "gemini-3-flash-preview";
        var apiKey = _config["Gemini:ApiKey"];
        var apiUrl = $"https://generativelanguage.googleapis.com/v1alpha/models/{model}:generateContent?key={apiKey}";

        var prompt = $"Generate a concise, 20-word description for a {device.Manufacturer} {device.Name} " +
                     $"with {device.Processor} and {device.RAMAmount} RAM running {device.OS}.";

        var requestBody = new { contents = new[] { new { role = "user", parts = new[] { new { text = prompt } } } } };

        var response = await _httpClient.PostAsJsonAsync(apiUrl, requestBody);
        if (!response.IsSuccessStatusCode) return (false, "AI API Error");

        using JsonDocument doc = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        string text = doc.RootElement.GetProperty("candidates")[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text").GetString();
        return (true, text?.Trim() ?? "");
    }
}