namespace Daco.Application.Common.Interfaces.Services.Authentication
{
    public interface IJwtService
    {
        string GenerateToken(Guid userId, string username, string? email, string? phone, List<string> roles);
        string GenerateRefreshToken();
        string HashToken(string token);
        string GenerateTempToken(Guid userId);
        Guid? ValidateTempToken(string tempToken);
    }
}
