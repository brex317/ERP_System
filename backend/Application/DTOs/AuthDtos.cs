namespace Raras.EMS.API.Models.DTOs;

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
