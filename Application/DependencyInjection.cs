namespace Daco.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(assembly);

                cfg.AddOpenBehavior(typeof(LoggingBehaviour<,>));
                cfg.AddOpenBehavior(typeof(ValidationBehaviour<,>));
                cfg.AddOpenBehavior(typeof(DomainEventBehaviour<,>));
                cfg.AddOpenBehavior(typeof(TransactionBehaviour<,>));
            });

            // FluentValidation
            services.AddValidatorsFromAssembly(assembly);

            return services;
        }
    }
}
