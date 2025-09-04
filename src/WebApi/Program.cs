using Application.Extensions;
using Infrastructure.Extensions;
using Infrastructure.Data;
using WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add application services
builder.Services.AddApplicationServices();

// Add infrastructure services
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                      ?? "Host=localhost;Port=5432;Database=blogdb;Username=postgres;Password=postgres";
builder.Services.AddInfrastructureServices(connectionString);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Log application startup
app.Logger.LogInformation("Application starting up");

// Initialize database
await DatabaseInitializer.InitializeDatabaseAsync(connectionString);
app.Logger.LogInformation("Database initialized successfully");

app.Run();