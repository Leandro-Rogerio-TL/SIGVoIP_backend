using PainelIntegraTelefoniaIP.Services.Interfaces;

namespace PainelIntegraTelefoniaIP.Services.Configuration;

public static class ConfigServices
{
    public static IServiceCollection AddMeusServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IService<,>), typeof(GenericService<,>));
        return services;
    }
} 