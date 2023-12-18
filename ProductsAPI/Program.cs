using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProductsAPI.Context;
using ProductsAPI.DTOs.Mappings;
using ProductsAPI.Extensions;
using ProductsAPI.Filters;
using ProductsAPI.Logging;
using ProductsAPI.Repository;
using ProductsAPI.Services;
using System.Text;
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
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ProductsAPI", Version = "v1" });

    // Add JWT authentication
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Add database services - builder.Configuration => appsettings.json
var connectionStringMySql = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<EFContext>
    (opts => opts.UseMySql(connectionStringMySql, ServerVersion.AutoDetect(connectionStringMySql)));

// Add AutoMapper services
var mappingConfig = new MapperConfiguration(mc => mc.AddProfile(new MappingProfile()));
IMapper mapper = mappingConfig.CreateMapper();

// Add Identity services
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<EFContext>()
                .AddDefaultTokenProviders();

// Add token JWT services
// Adiciona o manipulador de autenticação e define o esquema de autenticação usado (Bearer)
// Define como será validado o token recebido e as informações que devem ser validadas
builder.Services.AddAuthentication(
    JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opts =>
    opts.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidAudience = builder.Configuration["TokenConfiguration:Audience"],
        ValidIssuer = builder.Configuration["TokenConfiguration:Issuer"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    });

// Add CORS services
// builder.Services.AddCors();
builder.Services.AddCors(opts =>
{
    opts.AddPolicy("AllowGET", builder =>
    {
        builder.AllowAnyOrigin()
        .WithMethods("GET")
        .AllowAnyHeader();
    });
});
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

// Configure CORS services
// app.UseCors(opts => opts.WithOrigins("https://www.apirequest.io/").AllowAnyMethod().AllowAnyHeader());
app.UseCors();

// Configure exception handler middleware
//app.ConfigureExceptionHandler();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

// Middleware for authentication
app.UseAuthentication();

// Middleware for authorization
app.UseAuthorization();

app.MapControllers();

app.Run();
