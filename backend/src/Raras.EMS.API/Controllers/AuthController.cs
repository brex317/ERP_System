using Microsoft.AspNetCore.Mvc;

namespace Raras.EMS.API.Controllers;

public class LoginRequestDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginResponseDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public UserProfileDto? User { get; set; }
}

public class UserProfileDto
{
    public string Name { get; set; } = string.Empty;
    public string Initials { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new LoginResponseDto
            {
                Success = false,
                Message = "Email and password are required."
            });
        }

        // Demo credentials check (can connect to employees table / auth system)
        return Ok(new LoginResponseDto
        {
            Success = true,
            Message = "Login successful.",
            Token = "raras-ems-jwt-token-demo-" + Guid.NewGuid().ToString("N"),
            User = new UserProfileDto
            {
                Name = "Berihu",
                Initials = "BE",
                Email = request.Email,
                Role = "System Administrator"
            }
        });
    }
}
