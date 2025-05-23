using Microsoft.EntityFrameworkCore;
using MyRestApi.Data;
using MyRestApi.Models;
using MyRestApi.Services;

var builder = WebApplication.CreateBuilder(args);

var dbUser = Environment.GetEnvironmentVariable("DB_USER");
var dbPass = Environment.GetEnvironmentVariable("DB_PASS");

if (string.IsNullOrEmpty(dbUser) || string.IsNullOrEmpty(dbPass))
    throw new Exception("Database credentials are not set in environment variables.");

var connectionString = $"server=localhost;port=3306;database=myrestapi_db;user={dbUser};password={dbPass}";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

builder.Services.AddScoped<ProductService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();
app.MapControllers();

app.Run();