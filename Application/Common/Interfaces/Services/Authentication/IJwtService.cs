namespace Daco.Application.Common.Interfaces.Services.Authentication
{
    public interface IJwtService
    {
        string GenerateToken(Guid userId, string username, string? email, string? phone);
        string GenerateRefreshToken();
        string HashToken(string token);
    }
}
