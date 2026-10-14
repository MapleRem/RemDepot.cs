using Microsoft.EntityFrameworkCore;
using ShopSystem.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// Controllers
// Controllers configured to accept string values for enums (e.g., "Customer", "Dev")
<<<<<<< HEAD
// Controllers with circular reference handling for EF navigation properties
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
=======
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
>>>>>>> 8caedb5eb067c4e0230142c6e64fe678a6dc5727
    });

// Swagger (interactive API docs at /swagger)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// MySQL via Pomelo EF Core provider
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// CORS left wide open so you can test from a separate frontend during development.
// Tighten this before submitting/deploying if your rubric cares about it.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// Always on (not gated behind IsDevelopment) so it works with a plain
// `dotnet run` even without a launchSettings.json setting the environment.
// If you ever deploy this publicly, consider re-adding the Development check.
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

// Automatically create the database and tables if they don't exist
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

app.Run();


app.Run();
