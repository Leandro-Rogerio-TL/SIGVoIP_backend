using PainelIntegraTelefoniaIP.Data.Configuration;
using PainelIntegraTelefoniaIP.infraestructure.Middleware;
using PainelIntegraTelefoniaIP.Repository.Configuration;
using PainelIntegraTelefoniaIP.Services.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDataConfiguration();

builder.Services.AddMySqlRepository(builder.Configuration);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddMeusServices();

builder.Services.AddAuthentication().AddJwtBearer();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMeusMiddleware();

app.MapControllers();

app.Run();