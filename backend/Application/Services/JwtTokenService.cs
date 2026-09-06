using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Raras.EMS.API.Models.Entities;

namespace Raras.EMS.API.Services;

public class JwtTokenService : ITokenService
{
    private readonly string _secretKey;

    public JwtTokenService(IConfiguration configuration)
    {
        _secretKey = configuration["Jwt:SecretKey"] ?? "Raras_EMS_Super_Secret_Jwt_Security_Key_2026_Admin_Auth!";
    }

    public string GenerateToken(User user)
    {
        var header = new { alg = "HS256", typ = "JWT" };

        var roleName = user.Role?.Name ?? "Admin";
        var now = DateTimeOffset.UtcNow;
        var exp = now.AddDays(7).ToUnixTimeSeconds();
        var iat = now.ToUnixTimeSeconds();

        var payload = new Dictionary<string, object>
        {
            { "sub", user.Id.ToString() },
            { "email", user.Email },
            { "username", user.Username },
            { "name", $"{user.FirstName} {user.LastName}".Trim() },
            { "role", roleName },
            { "iat", iat },
            { "exp", exp }
        };

        string headerJson = JsonSerializer.Serialize(header);
        string payloadJson = JsonSerializer.Serialize(payload);

        string encodedHeader = Base64UrlEncode(Encoding.UTF8.GetBytes(headerJson));
        string encodedPayload = Base64UrlEncode(Encoding.UTF8.GetBytes(payloadJson));

        string unsignedToken = $"{encodedHeader}.{encodedPayload}";

        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_secretKey));
        byte[] signatureBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(unsignedToken));
        string encodedSignature = Base64UrlEncode(signatureBytes);

        return $"{unsignedToken}.{encodedSignature}";
    }

    private static string Base64UrlEncode(byte[] input)
    {
        return Convert.ToBase64String(input)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }
}
