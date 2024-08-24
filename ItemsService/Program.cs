
//using Serilog;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using OrdersAndItemsService.API.MiddleWares;
using OrdersAndItemsService.Services.Settings;
using Repository.Data;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<StoreContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection"), sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure();
    });
});


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
/*var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);*/

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

var app = builder.Build();///

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


app.UseStatusCodePagesWithReExecute("/error/{0}");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
