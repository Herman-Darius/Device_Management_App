using Device_Management_App.Server.Data;
using Device_Management_App.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AccountService : IAccountService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public AccountService(AppDbContext context)
    {
        _context = context;
    }
    public AccountService(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    

    public async Task<(bool Success, string Message)> RegisterAsync(User user)
    {
        if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            return (false, "Email already registered.");

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return (true, string.Empty);
    }

    public async Task<(bool Success, string Message, object? Data)> LoginAsync(LoginDto login)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == login.Email && u.Password == login.Password);
        if (user == null) return (false, "Invalid email or password.", null);

        var token = GenerateJwtToken(user);
        return (true, string.Empty, new { Token = token, User = new { user.Id, user.Name, user.Email, user.Role } });
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("UserName", user.Name)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourVeryLongSecretKey_MakeSureItIsAtLeast32Chars!")); //
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "DeviceApp",
            audience: "DeviceAppUsers",
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}