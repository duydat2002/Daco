namespace Daco.Infrastructure.Services.Authentication
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _settings;

        public JwtService(IOptions<JwtSettings> settings)
        {
            _settings = settings.Value;
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        }

        public string GenerateToken(
            Guid userId,
            string username,
            string? email,
            string? phone,
            List<string> roles)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub,        userId.ToString()),
                new(JwtRegisteredClaimNames.UniqueName, username),
                new(JwtRegisteredClaimNames.Jti,        Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Iat,        DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            };

            if (!string.IsNullOrEmpty(email))
                claims.Add(new(JwtRegisteredClaimNames.Email, email));

            if (!string.IsNullOrEmpty(phone))
                claims.Add(new("phone", phone));

            foreach (var role in roles)
                claims.Add(new(ClaimTypes.Role, role));

            return BuildToken(claims, _settings.ExpirationHours);
        }

        public string GenerateAdminToken(
            Guid userId,
            Guid adminId,
            string username,
            string? email,
            string? phone,
            List<string> roles)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub,        userId.ToString()),
                new("admin_id",                         adminId.ToString()),  
                new(JwtRegisteredClaimNames.UniqueName, username),
                new(JwtRegisteredClaimNames.Jti,        Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Iat,        DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            };

            if (!string.IsNullOrEmpty(email))
                claims.Add(new(JwtRegisteredClaimNames.Email, email));

            if (!string.IsNullOrEmpty(phone))
                claims.Add(new("phone", phone));

            foreach (var role in roles)
                claims.Add(new(ClaimTypes.Role, role));

            return BuildToken(claims, _settings.ExpirationHours);
        }

        public string GenerateTempToken(Guid userId)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new("token_type",                "temp"),
            };

            return BuildToken(claims, expirationHours: null, expirationMinutes: 10);
        }

        public Guid? ValidateTempToken(string tempToken)
        {
            try
            {
                var principal = ValidateToken(tempToken);
                if (principal is null) return null;

                var tokenType = principal.FindFirstValue("token_type");
                if (tokenType != "temp") return null;

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

        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        public string HashToken(string token)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
            return Convert.ToHexString(bytes).ToLowerInvariant();
        }

        private string BuildToken(
            List<Claim> claims,
            int? expirationHours = null,
            int? expirationMinutes = null)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = expirationMinutes.HasValue
                ? DateTime.UtcNow.AddMinutes(expirationMinutes.Value)
                : DateTime.UtcNow.AddHours(expirationHours ?? _settings.ExpirationHours);

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expires,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private ClaimsPrincipal? ValidateToken(string token)
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

            return new JwtSecurityTokenHandler()
                .ValidateToken(token, validationParams, out _);
        }
    }
}