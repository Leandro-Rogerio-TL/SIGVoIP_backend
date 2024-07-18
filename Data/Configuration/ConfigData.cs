using Microsoft.AspNetCore.Identity;
using PainelIntegraTelefoniaIP.Data.Context;
using PainelIntegraTelefoniaIP.Entity;

namespace PainelIntegraTelefoniaIP.Data.Configuration;

public static class ConfigData
{
    public static IServiceCollection AddDataConfiguration(this IServiceCollection services)
    {
        services.AddIdentity<Usuario, IdentityRole>()
                .AddEntityFrameworkStores<PainelDbContext>()
                .AddDefaultTokenProviders();
        return services;
    }
}