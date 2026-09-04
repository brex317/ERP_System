using Microsoft.AspNetCore.Mvc;
using Raras.EMS.API.Services;

namespace Raras.EMS.API.Controllers;

public class LoginRequestDto
{
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public string GetIdentifier()
    {
        if (!string.IsNullOrWhiteSpace(Email)) return Email.Trim();
        if (!string.IsNullOrWhiteSpace(Username)) return Username.Trim();
        return string.Empty;
    }
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
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        string identifier = request.GetIdentifier();
        if (string.IsNullOrWhiteSpace(identifier) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new LoginResponseDto
            {
                Success = false,
                Message = "Username or email and password are required."
            });
        }

        var result = await _authService.AuthenticateAsync(identifier, request.Password);
        if (!result.Success)
        {
            return Unauthorized(result);
        }

        return Ok(result);
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        return Ok(new { success = true, message = "Logout successful." });
    }
}
