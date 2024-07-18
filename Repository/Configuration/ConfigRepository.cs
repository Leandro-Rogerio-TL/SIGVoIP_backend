using Microsoft.EntityFrameworkCore;
using PainelIntegraTelefoniaIP.Data.Context;
using PainelIntegraTelefoniaIP.Repository.Interfaces;

namespace PainelIntegraTelefoniaIP.Repository.Configuration;

public static class ConfigRepository
{
    public static IServiceCollection AddMySqlRepository(this IServiceCollection services, IConfiguration config)
    {
        var connString = config.GetConnectionString("MySqlConnection");
        services.AddDbContext<DbContext, PainelDbContext>(options =>
        {
            options.UseMySql(connString, ServerVersion.AutoDetect(connString));
        });
        services.AddScoped(typeof(IRepository<>), typeof(MySqlRepository<>));
        return services;
    }
}