using Device_Management_App.Server.Data;
using Device_Management_App.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Device_Management_App.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DevicesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Device>>> GetDevices()
        {
            return await _context.Devices.Include(d => d.AssignedUser).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Device>> GetDevice(int id)
        {
            var device = await _context.Devices.Include(d => d.AssignedUser)
                                               .FirstOrDefaultAsync(d => d.Id == id);
            if (device == null) return NotFound();
            return device;
        }

        [HttpPost]
        public async Task<ActionResult<Device>> PostDevice(Device device)
        {
            var exists = await _context.Devices
                .AnyAsync(d => d.Name.ToLower() == device.Name.ToLower());

            if (exists) return BadRequest("A device with this name already exists.");

            _context.Devices.Add(device);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDevice), new { id = device.Id }, device);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDevice(int id, Device device)
        {
            if (id != device.Id) return BadRequest();
            _context.Entry(device).State = EntityState.Modified;
            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Devices.Any(e => e.Id == id)) return NotFound();
                else throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDevice(int id)
        {
            var device = await _context.Devices.FindAsync(id);
            if (device == null) return NotFound();
            _context.Devices.Remove(device);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id}/assign/{userId}")]
        public async Task<IActionResult> AssignDevice(int id, int userId)
        {
            var device = await _context.Devices.FindAsync(id);
            if (device == null) return NotFound();
            device.AssignedUserId = userId;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id}/unassign")]
        public async Task<IActionResult> UnassignDevice(int id)
        {
            var device = await _context.Devices.FindAsync(id);
            if (device == null) return NotFound();
            device.AssignedUserId = null;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("generate-description")]
        public async Task<IActionResult> GenerateDescription([FromBody] Device device)
        {
            var model = "gemini-3-flash-preview";
            var apiKey = "AIzaSyDqXb_OSc_y2HWxwA7SEtWKgFZw5zWQIDU";
            var apiUrl = $"https://generativelanguage.googleapis.com/v1alpha/models/{model}:generateContent?key={apiKey}";

            using var client = new HttpClient();

            var prompt = $"Generate a concise, 20-word description for a {device.Manufacturer} {device.Name} " +
                         $"with {device.Processor} and {device.RAMAmount} RAM running {device.OS}. " +
                         "Focus on technical performance.";

            var requestBody = new
            {
                contents = new[] {
            new {
                role = "user",
                parts = new[] { new { text = prompt } }
            }
        },
                generationConfig = new
                {
                    thinking_config = new
                    {
                        thinking_level = "HIGH"
                    }
                }
            };

            try
            {
                var response = await client.PostAsJsonAsync(apiUrl, requestBody);

                if (!response.IsSuccessStatusCode)
                {
                    var errorDetail = await response.Content.ReadAsStringAsync();
                    return BadRequest($"AI API Error: {errorDetail}");
                }

                using JsonDocument doc = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
                JsonElement root = doc.RootElement;

                string generatedText = root.GetProperty("candidates")[0]
                                           .GetProperty("content")
                                           .GetProperty("parts")[0]
                                           .GetProperty("text")
                                           .GetString();

                return Ok(new { description = generatedText?.Trim() });
            }
            catch (Exception ex)
            {
                return BadRequest("System Error during AI generation: " + ex.Message);
            }
        }
    }
}