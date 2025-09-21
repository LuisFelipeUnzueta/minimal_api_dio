using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Minimal.Api.Domain.ModelViews;
using Minimal.Api.Infra.Db;
using Minimal.Api.Infra.Ioc;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddJwtAuthentication(builder.Configuration)
    .AddAuthorization()
    .AddCustomServices()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Insert your JWT token like this: {your-token}"
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });
    });

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), 
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")));

});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => Results.Json(new Home()));

app.Run();
