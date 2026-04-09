using Device_Management_App.Server.Data;
using Device_Management_App.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Device_Management.Tests
{
    public class DeviceServiceTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task CreateDeviceAsync_ReturnsFalse_WhenNameExists()
        {
            var context = GetDbContext();
            context.Devices.Add(new Device
            {
                Name = "iPhone 15",
                Manufacturer = "Apple",
                Type = "Mobile",
                OS = "iOS",
                OSVersion = "17.0",
                Processor = "A17",
                RAMAmount = "8GB"
            });
            await context.SaveChangesAsync();

            var service = new DeviceService(context, new HttpClient(), new Mock<IConfiguration>().Object);
            var newDevice = new Device
            {
                Name = "iPhone 15",
                Manufacturer = "Apple",
                Type = "Mobile",
                OS = "iOS",
                OSVersion = "17.0",
                Processor = "A17",
                RAMAmount = "8GB"
            };

            var result = await service.CreateDeviceAsync(newDevice);

            Assert.False(result.Success);
            Assert.Equal("A device with this name already exists.", result.Message);
        }

        [Fact]
        public async Task UnassignDeviceAsync_SetsUserIdToNull()
        {
            var context = GetDbContext();
            var device = new Device
            {
                Id = 1,
                Name = "Laptop",
                Manufacturer = "Dell",
                Type = "Laptop",
                OS = "Windows",
                OSVersion = "11",
                Processor = "i7",
                RAMAmount = "16GB",
                AssignedUserId = 99
            };
            context.Devices.Add(device);
            await context.SaveChangesAsync();

            var service = new DeviceService(context, new HttpClient(), new Mock<IConfiguration>().Object);

            await service.UnassignDeviceAsync(1);

            var updatedDevice = await context.Devices.FindAsync(1);
            Assert.Null(updatedDevice.AssignedUserId);
        }
    }
}
