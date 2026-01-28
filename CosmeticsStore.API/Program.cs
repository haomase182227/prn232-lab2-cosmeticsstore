using CosmeticsStore.API.Filters;
using CosmeticsStore.API.Middleware;
using CosmeticsStore.Repositories;
using CosmeticsStore.Repositories.Interfaces;
using CosmeticsStore.Repositories.Models;
using CosmeticsStore.Services;
using CosmeticsStore.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// =====================================================
// CONFIGURATION (support Docker ENV)
// =====================================================

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

// =====================================================
// DATABASE
// =====================================================

var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");

if (string.IsNullOrEmpty(connectionString))
{
    throw new Exception("❌ Connection string 'DefaultConnectionString' not found");
}

builder.Services.AddDbContext<CosmeticsDbContext>(options =>
{
    options.UseSqlServer(connectionString, sqlServerOptions =>
    {
        sqlServerOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
    });
});

// =====================================================
// ODATA CONFIGURATION
// =====================================================

var modelBuilder = new ODataConventionModelBuilder();
modelBuilder.EntitySet<CosmeticInformation>("CosmeticInformations");
modelBuilder.EntitySet<CosmeticCategory>("CosmeticCategories");

// =====================================================
// DEPENDENCY INJECTION
// =====================================================

// Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Repositories
builder.Services.AddScoped<ISystemAccountRepository, SystemAccountRepository>();
builder.Services.AddScoped<ICosmeticInformationRepository, CosmeticInformationRepository>();

// Services
builder.Services.AddScoped<ISystemAccountService, SystemAccountService>();
builder.Services.AddScoped<ICosmeticInformationService, CosmeticInformationService>();

// =====================================================
// CONTROLLERS + ODATA + SWAGGER
// =====================================================

builder.Services.AddControllers(options =>
    {
        // Add validation filter globally
        options.Filters.Add<ValidationFilter>();
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        
        // Serialize enums as strings for better readability in Swagger
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    })
    .AddOData(options => options
        .Select()
        .Filter()
        .OrderBy()
        .Expand()
        .Count()
        .SetMaxTop(null)
        .AddRouteComponents("odata", modelBuilder.GetEdmModel()));

// Disable automatic 400 response for model validation (let our filter handle it)
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddEndpointsApiExplorer();

// =====================================================
// JWT AUTHENTICATION (Simple - No Role Authorization)
// =====================================================

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"] ?? throw new InvalidOperationException("JWT:SecretKey not configured")))
        };
    });

// No authorization policies - simple authentication only
builder.Services.AddAuthorization();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Cosmetics Store API",
        Version = "1.0",
        Description = @"REST API with:
- Model-based architecture (Repository + Unit of Work)
- Simple authentication (no role-based authorization)
- Request/Response models
- Lowercase URLs with kebab-case query params"
    });

    // JWT Authentication configuration for Swagger
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below. Example: 'Bearer 12345abcdef'",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    options.AddSecurityDefinition("Bearer", jwtSecurityScheme);

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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

    options.CustomSchemaIds(type => type.FullName?.Replace("+", "."));
    
    // Configure Swagger to display enums as strings instead of integers
    options.UseInlineDefinitionsForEnums();
});

// =====================================================
// CORS
// =====================================================

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// =====================================================
// GLOBAL EXCEPTION HANDLER (PRN232 Requirement #8)
// =====================================================

app.UseGlobalExceptionHandler();

// =====================================================
// MIDDLEWARE
// =====================================================

app.UseRouting();

// Swagger (always ON – good for Docker)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CosmeticsStore API v1");
    c.RoutePrefix = "swagger";
    c.DefaultModelsExpandDepth(-1); // Ẩn Schemas section
});

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// =====================================================
// AUTO MIGRATION (SAFE FOR DOCKER)
// =====================================================

using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<CosmeticsDbContext>();
        db.Database.Migrate();
        Console.WriteLine("✅ Database migration completed successfully");
    }
    catch (Exception ex)
    {
        Console.WriteLine("❌ Database migration failed");
        Console.WriteLine(ex.Message);
    }
}

app.Run();

