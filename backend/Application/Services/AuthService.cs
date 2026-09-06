using Microsoft.EntityFrameworkCore;
using Raras.EMS.API.Controllers;
using Raras.EMS.API.Data;

namespace Raras.EMS.API.Services;

public class AuthService : IAuthService
{
    private readonly EmsDbContext _db;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public AuthService(
        EmsDbContext db,
        IPasswordHasher passwordHasher,
        ITokenService tokenService)
    {
        _db = db;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<LoginResponseDto> AuthenticateAsync(string identifier, string password)
    {
        if (string.IsNullOrWhiteSpace(identifier) || string.IsNullOrWhiteSpace(password))
        {
            return new LoginResponseDto
            {
                Success = false,
                Message = "Username/email and password are required."
            };
        }

        var normalizedIdentifier = identifier.Trim().ToLowerInvariant();

        var user = await _db.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email.ToLower() == normalizedIdentifier || u.Username.ToLower() == normalizedIdentifier);

        if (user == null)
        {
            return new LoginResponseDto
            {
                Success = false,
                Message = "Invalid username/email or password."
            };
        }

        if (!user.IsActive)
        {
            return new LoginResponseDto
            {
                Success = false,
                Message = "Your account is deactivated. Please contact your system administrator."
            };
        }

        bool isPasswordValid = _passwordHasher.VerifyPassword(password, user.PasswordHash);
        if (!isPasswordValid)
        {
            return new LoginResponseDto
            {
                Success = false,
                Message = "Invalid username/email or password."
            };
        }

        string token = _tokenService.GenerateToken(user);
        string fullName = $"{user.FirstName} {user.LastName}".Trim();
        string initials = GetInitials(user.FirstName, user.LastName);
        string roleName = user.Role?.Name ?? "Admin";

        return new LoginResponseDto
        {
            Success = true,
            Message = "Login successful.",
            Token = token,
            User = new UserProfileDto
            {
                Name = fullName,
                Initials = initials,
                Email = user.Email,
                Role = roleName
            }
        };
    }

    private static string GetInitials(string firstName, string lastName)
    {
        string fn = string.IsNullOrWhiteSpace(firstName) ? "" : firstName.Trim().Substring(0, 1).ToUpperInvariant();
        string ln = string.IsNullOrWhiteSpace(lastName) ? "" : lastName.Trim().Substring(0, 1).ToUpperInvariant();
        string initials = $"{fn}{ln}";
        return string.IsNullOrEmpty(initials) ? "AD" : initials;
    }
}
