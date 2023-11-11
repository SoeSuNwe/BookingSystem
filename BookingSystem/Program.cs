using BookingSystem.Controllers;
using BookingSystem.Data;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Hangfire.MemoryStorage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});
// or
// services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

// Configure Redis for caching
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
    options.InstanceName = "Instance";
});

// Configure Hangfire
builder.Services.AddHangfire(config => config.UseMemoryStorage());

// Add other services (authentication, swagger, etc.)


builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PackageService>();
builder.Services.AddScoped<ScheduleService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
// Configure Hangfire dashboard
app.UseHangfireDashboard();
// Configure Hangfire recurring jobs 

var backgroundJobService = new BackgroundJobService();
backgroundJobService.ConfigureRecurringJobs();

app.MapControllers();

app.Run();
