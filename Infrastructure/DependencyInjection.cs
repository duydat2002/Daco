namespace Daco.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Database Session
            services.AddScoped<IDbSession>(sp =>
            {
                var connectionString = configuration.GetConnectionString(ConnectionStringNames.Ecommerce)
                    ?? throw new InvalidOperationException("Connection string not found");

                return new NpgsqlDbSession(connectionString);
            });

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

            return services;
        }
    }
}
