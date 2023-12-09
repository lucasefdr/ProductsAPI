using Microsoft.EntityFrameworkCore;
using ProductsAPI.Context;
using ProductsAPI.Extensions;
using ProductsAPI.Filters;
using ProductsAPI.Logging;
using ProductsAPI.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// JsonOptions configure
builder.Services.AddControllers().AddJsonOptions(opts =>
{
    opts.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
    opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency injection
builder.Services.AddTransient<IFromService, FromService>();

// Add filters services
builder.Services.AddScoped<ApiLoggingFilter>();

// Add database services - builder.Configuration => appsettings.json
var connectionStringMySql = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>
    (opts => opts.UseMySql(connectionStringMySql, ServerVersion.AutoDetect(connectionStringMySql)));

var app = builder.Build();

// Configure logging services
var loggerFactory = new LoggerFactory();
loggerFactory.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
{
    LogLevel = LogLevel.Information
}));


// Configure exception handler middleware
app.ConfigureExceptionHandler();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
