using Microsoft.EntityFrameworkCore;
using VehicleWebAPI;

string myAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string connectionString = $"Data Source=.;Initial Catalog=VehicleDB;Integrated Security=True;TrustServerCertificate=True";
builder.Services.AddDbContext<DataContext>(opt => opt.UseSqlServer(connectionString));

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
    policy =>
    {
        policy.WithOrigins(
            "http://localhost:4200",
            "https://localhost:4200",
            "http://bayland.com",
            "https://bayland.com")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseAuthorization();

app.UseCors();

app.MapControllers();

app.Run();
