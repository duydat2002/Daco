using Daco.Infrastructure.Persistence.Repositories.UserAddresses;
using Daco.Infrastructure.Persistence.Repositories.UserBankAccounts;

namespace Daco.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<EmailSettings>(configuration.GetSection("Email"));
            services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
            services.Configure<CloudflareR2Settings>(configuration.GetSection("CloudflareR2"));

            var connectionString = configuration.GetConnectionString(ConnectionStringNames.Ecommerce)
                    ?? throw new InvalidOperationException("Connection string not found");

            services.AddNpgsqlDataSource(connectionString, builder =>  builder
                .MapEnum<UserGender>("user_gender"));

            services.AddScoped<IDbSession, NpgsqlDbSession>();

            services.AddDbContextFactory<AppDbContext>(options =>
                options.UseNpgsql(connectionString));

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            // Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Dapper Executor
            services.AddScoped<DapperExecutor>();

            // Domain Event Dispatcher
            services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IVerificationTokenRepository, VerificationTokenRepository>();
            services.AddScoped<ILoginSessionRepository, LoginSessionRepository>();
            services.AddScoped<IUserAddressRepository, UserAddressRepository>();
            services.AddScoped<IBankAccountRepository, BankAccountRepository>();

            // External Services
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ISmsService, SmsService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IGoogleAuthService, GoogleAuthService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IFileStorageService, FileStorageService>();

            // JWT Authentication
            var jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>()
                ?? throw new InvalidOperationException("JWT settings not configured");

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                        ClockSkew = TimeSpan.Zero 
                    };
                });

            return services;
        }
    }
}
