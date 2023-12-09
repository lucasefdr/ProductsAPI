using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.Context;
using ProductsAPI.DTOs.Mappings;
using ProductsAPI.Extensions;
using ProductsAPI.Filters;
using ProductsAPI.Logging;
using ProductsAPI.Repository;
using ProductsAPI.Services;
using System.Text.Json.Serialization;

// Configure the app's services (ConfigureServices method)
var builder = WebApplication.CreateBuilder(args);

#region Add Services

// Add Controllers services and configure JSON serialization
builder.Services.AddControllers().AddJsonOptions(opts =>
{
    opts.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add database services - builder.Configuration => appsettings.json
var connectionStringMySql = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>
    (opts => opts.UseMySql(connectionStringMySql, ServerVersion.AutoDetect(connectionStringMySql)));

// Add AutoMapper services
var mappingConfig = new MapperConfiguration(mc => mc.AddProfile(new MappingProfile()));
IMapper mapper = mappingConfig.CreateMapper();
#endregion Add Services


#region Dependency Injection
// Dependency injection
builder.Services.AddTransient<IFromService, FromService>();

// Add filters services
builder.Services.AddScoped<ApiLoggingFilter>();

// Add unit of work services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Add AutoMapper services
builder.Services.AddSingleton(mapper);
#endregion Dependency Injection

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
