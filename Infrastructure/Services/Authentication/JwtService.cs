namespace Daco.Infrastructure.Services.Authentication
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _settings;

        public JwtService(IOptions<JwtSettings> settings)
        {
            _settings = settings.Value;
        }

        public string GenerateToken(Guid userId, string username, string? email, string? phone, List<string> roles)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub,        userId.ToString()),
                new(JwtRegisteredClaimNames.UniqueName, username),
                new(JwtRegisteredClaimNames.Jti,        Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Iat,        DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            };
            foreach (var role in roles)
                claims.Add(new(ClaimTypes.Role, role));

            if (!string.IsNullOrEmpty(email))
                claims.Add(new Claim(JwtRegisteredClaimNames.Email, email));

            if (!string.IsNullOrEmpty(phone))
                claims.Add(new Claim("phone", phone));

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(_settings.ExpirationHours),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        public string HashToken(string token)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
            return Convert.ToHexString(bytes).ToLowerInvariant();
        }

        public string GenerateTempToken(Guid userId)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new("token_type", "temp"),  // phân biệt với JWT thật
            };

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(5),  // chỉ sống 5 phút
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public Guid? ValidateTempToken(string tempToken)
        {
            try
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));

                var validationParams = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _settings.Issuer,
                    ValidAudience = _settings.Audience,
                    IssuerSigningKey = key,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = new JwtSecurityTokenHandler()
                    .ValidateToken(tempToken, validationParams, out var validatedToken);

                var tokenType = principal.FindFirstValue("token_type");
                if (tokenType != "temp")
                    return null;

                var sub = principal.FindFirstValue(JwtRegisteredClaimNames.Sub);
                if (sub is null || !Guid.TryParse(sub, out var userId))
                    return null;

                return userId;
            }
            catch
            {
                return null;
            }
        }
    }
}
