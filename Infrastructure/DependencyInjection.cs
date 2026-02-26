using Daco.Domain.Users.Constants;
using Daco.Infrastructure.Persistence;

namespace Daco.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Database Session
            //services.AddScoped<IDbSession>(sp =>
            //{
            //    var connectionString = configuration.GetConnectionString(ConnectionStringNames.Ecommerce)
            //        ?? throw new InvalidOperationException("Connection string not found");

            //    return new NpgsqlDbSession(connectionString);
            //});


            var connectionString = configuration.GetConnectionString(ConnectionStringNames.Ecommerce)
                    ?? throw new InvalidOperationException("Connection string not found");

            services.AddNpgsqlDataSource(connectionString, builder =>  builder
                .MapEnum<UserGender>("user_gender"));

            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString)
                //options.UseNpgsql(connectionString, o =>
                    //o.MapEnum<UserStatus>("user_status")
                    // .MapEnum<UserGender>("user_gender")
                    // .MapEnum<VerificationStatus>("verification_status")
                    // .MapEnum<VerificationTokenType>("token_types")
                //)
            );

            services.AddScoped<IDbSession, NpgsqlDbSession>();

            // Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Dapper Executor
            services.AddScoped<DapperExecutor>();

            // Domain Event Dispatcher
            services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();

            // External Services
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ISmsService, SmsService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IVerificationTokenService, VerificationTokenService>();
            services.AddScoped<IGoogleAuthService, GoogleAuthService>();

            return services;
        }
    }
}
