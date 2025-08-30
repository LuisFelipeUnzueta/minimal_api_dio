using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Minimal.Api.Domain.Interfaces;
using Minimal.Api.Domain.ModelViews;
using Minimal.Api.Domain.Service;
using Minimal.Api.Infra.Db;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddScoped<IAdminService, AdminService>()
    .AddScoped<IVehicleService, VehicleService>();

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), 
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")));

});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.MapGet("/", () => Results.Json(new Home()));

app.Run();
