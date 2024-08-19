using Microsoft.EntityFrameworkCore;
using OrdersAndItemsService.MiddleWares;
using OrdersAndItemsService.Filters;
using Serilog;
using Microsoft.OpenApi.Models;
using OrdersAndItemsService.NewFolder;
using WebApplication1.Data;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Configure DbContext for SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection"), sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure();
    });
});

// Authorization Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("HRDepartmentOnly", policy =>
        policy.RequireClaim("Department", "HR"));
    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireRole("Admin"));
    options.AddPolicy("Over18Policy", policy =>
        policy.RequireClaim("Age", "18"));
});

// Add controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme()
    {
        Description = "API Key to secure the API",
        Type = SecuritySchemeType.ApiKey,
        Name = "X-API-KEY",
        In = ParameterLocation.Header,
        Scheme = "ApiKeyScheme"
    });

    var scheme = new OpenApiSecurityScheme()
    {
        Reference = new OpenApiReference()
        {
            Type = ReferenceType.SecurityScheme,
            Id = "ApiKey"
        },
        In = ParameterLocation.Header
    };

    var requirement = new OpenApiSecurityRequirement
    {
        { scheme, new List<string>() }
    };

    x.AddSecurityRequirement(requirement);
});

// Configure Serilog for logging
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

// Cloudinary settings and token lifespan
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.Configure<DataProtectionTokenProviderOptions>(
    o => o.TokenLifespan = TimeSpan.FromHours(5));

// Redis connection
builder.Services.AddSingleton<ConnectionMultiplexer>(c =>
{
    var configuration = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"), true);
    return ConnectionMultiplexer.Connect(configuration);
});

var app = builder.Build();

// Middleware configuration
app.UseMiddleware<ExceptionMiddleWare>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error");
}

// Status code handling for custom error pages
app.UseStatusCodePagesWithReExecute("/error/{0}");

// Enable HTTPS redirection and authentication
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
