using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "DotNetAPI.";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
// Expose OpenAPI and Scalar in all environments (IIS/Production included)
app.MapOpenApi();
app.MapScalarApiReference(options =>
{
    options.Title = "Distributed Caching API";
    options.Theme = ScalarTheme.BluePlanet;
    options.HideModels = false;
});

app.UseHttpsRedirection();

app.MapControllers();

// Redirect root to Scalar UI so the app opens there by default
app.MapGet("/", (HttpContext context) =>
{
    context.Response.Redirect("/scalar/v1");
    return Task.CompletedTask;
});

app.Run();
