using Device_Management_App.Server.Data;
using Device_Management_App.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Device_Management.Tests
{
    public class AccountServiceTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task RegisterAsync_ReturnsFailure_IfEmailTaken()
        {
            var context = GetDbContext();

            context.Users.Add(new User
            {
                Name = "Test User",
                Email = "test@test.com",
                Password = "hashedpassword",
                Role = "Employee",
                Location = "New York"
            });
            await context.SaveChangesAsync();

            var mockConfig = new Mock<IConfiguration>();
            var service = new AccountService(context, mockConfig.Object);

            var newUser = new User
            {
                Name = "Another User",
                Email = "test@test.com",
                Password = "password123",
                Role = "Employee",
                Location = "London"
            };

            var result = await service.RegisterAsync(newUser);

            Assert.False(result.Success);
            Assert.Equal("Email already registered.", result.Message);
        }
    }
}
