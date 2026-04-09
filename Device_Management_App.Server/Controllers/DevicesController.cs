using Device_Management_App.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace Device_Management_App.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly IDeviceService _deviceService;

        public DevicesController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDevices() => Ok(await _deviceService.GetAllDevicesAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDevice(int id)
        {
            var device = await _deviceService.GetDeviceByIdAsync(id);
            return device == null ? NotFound() : Ok(device);
        }

        [HttpPost]
        public async Task<IActionResult> PostDevice(Device device)
        {
            var result = await _deviceService.CreateDeviceAsync(device);
            if (!result.Success) return BadRequest(result.Message);

            return CreatedAtAction(nameof(GetDevice), new { id = result.Device!.Id }, result.Device);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDevice(int id, Device device)
        {
            var success = await _deviceService.UpdateDeviceAsync(id, device);
            return success ? NoContent() : BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDevice(int id)
        {
            var success = await _deviceService.DeleteDeviceAsync(id);
            return success ? NoContent() : NotFound();
        }

        [HttpPatch("{id}/assign/{userId}")]
        public async Task<IActionResult> Assign(int id, int userId)
        {
            var success = await _deviceService.AssignDeviceAsync(id, userId);
            return success ? NoContent() : NotFound();
        }

        [HttpPatch("{id}/unassign")]
        public async Task<IActionResult> Unassign(int id)
        {
            var success = await _deviceService.UnassignDeviceAsync(id);
            return success ? NoContent() : NotFound();
        }

        [HttpPost("generate-description")]
        public async Task<IActionResult> GenerateDescription([FromBody] Device device)
        {
            var (success, description) = await _deviceService.GenerateAIDescriptionAsync(device);
            return success ? Ok(new { description }) : BadRequest(description);
        }
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Device>>> SearchDevices([FromQuery] string q)
        {
            var results = await _deviceService.SearchDevicesAsync(q);
            return Ok(results);
        }
    }
}