using Microsoft.EntityFrameworkCore;
using Serilog;
using StackExchange.Redis;
using UrlShortener.API.Data;
using UrlShortener.API.Repositories;
using UrlShortener.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add Serilog configuration
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day));

// Add services to the container.
builder.Services.AddDbContext<UrlShortenerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUrlMappingRepository, UrlMappingRepository>();
builder.Services.AddScoped<IUrlShortenerService, UrlShortenerService>();

// Configure Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = builder.Configuration.GetConnectionString("RedisConnection");
    var options = ConfigurationOptions.Parse(configuration);
    options.AbortOnConnectFail = false; // Allow retries if Redis is unavailable
    return ConnectionMultiplexer.Connect(options);
});

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.AddControllers();

// configuring Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS globally
app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();