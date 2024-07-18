using Microsoft.EntityFrameworkCore;

namespace PainelIntegraTelefoniaIP.infraestructure.Middleware;

public class MeusMiddleware
{
    private readonly RequestDelegate _next;
    public MeusMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task Invoke(HttpContext context, DbContext dbContext)
    {
        await _next(context);
        await dbContext.SaveChangesAsync();
    }
}
public static class MeusMiddlewareExtensions
{
    public static IApplicationBuilder UseMeusMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<MeusMiddleware>();
    }
}