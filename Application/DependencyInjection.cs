namespace Daco.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<OtpSettings>(configuration.GetSection("Otp"));
            services.Configure<SessionSettings>(configuration.GetSection("Session"));

            var assembly = Assembly.GetExecutingAssembly();

            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(assembly);

                cfg.AddOpenBehavior(typeof(LoggingBehaviour<,>));
                cfg.AddOpenBehavior(typeof(ValidationBehaviour<,>));
                cfg.AddOpenBehavior(typeof(TransactionBehaviour<,>));
                cfg.AddOpenBehavior(typeof(DomainEventBehaviour<,>));
            });

            // FluentValidation
            services.AddValidatorsFromAssembly(assembly);

            return services;
        }
    }
}
