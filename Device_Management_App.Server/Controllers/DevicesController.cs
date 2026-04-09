using Device_Management_App.Server.Data;
using Device_Management_App.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

            if (exists)
            {
                return BadRequest("A device with this name already exists.");
            }

            _context.Devices.Add(device);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDevice), new { id = device.Id }, device);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDevice(int id, Device device)
        {
            if (id != device.Id) return BadRequest();

            _context.Entry(device).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
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


        // Auth
        [HttpPatch("{id}/assign/{userId}")]
        public async Task<IActionResult> AssignDevice(int id, int userId)
        {
            var device = await _context.Devices.FindAsync(id);
            if (device == null) return NotFound();
            if (device.AssignedUserId != null) return BadRequest("Device is already assigned.");

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
    }
}
