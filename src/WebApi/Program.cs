using Application.Extensions;
using Infrastructure.Extensions;
using Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

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

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Initialize database
await DatabaseInitializer.InitializeDatabaseAsync(connectionString);

app.Run();