using Microsoft.EntityFrameworkCore;
using Minimal.Api.Domain.Interfaces;
using Minimal.Api.Domain.Service;
using Minimal.Api.Infra.Db;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdminService, AdminService>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), 
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")));

});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
