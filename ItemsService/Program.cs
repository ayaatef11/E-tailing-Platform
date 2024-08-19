
using WebApplication1.Data;
using OrdersAndItemsService.MiddleWares;
using OrdersAndItemsService.NewFolder;
using Microsoft.OpenApi.Models;
using OrdersAndItemsService.Filters;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection"), sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure();
        // sqlOptions.TrustServerCertificate(true); // Ignore SSL certificate validation
    });
});

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("HRDepartmentOnly", policy =>
        policy.RequireClaim("Department", "HR"))
    .AddPolicy("AdminPolicy", policy =>
        policy.RequireRole("Admin"))
    .AddPolicy("Over18Policy", policy =>
        policy.RequireClaim("Age", "18"));



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ApiKeyAuthenticationFilter>();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(x =>
{
    x.AddSecurityDefinition(name: "ApiKey", new OpenApiSecurityScheme()
    {
        Description = "Api Key to secure the API",
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

    var requirement = new OpenApiSecurityRequirement()
    {
        { scheme, new List<string>() }
    };
    x.AddSecurityRequirement(requirement);
});

var logger=new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.Configure<DataProtectionTokenProviderOptions>(
    o => o.TokenLifespan = TimeSpan.FromHours(5));
builder.Services.AddSingleton<ConnectionMultiplexer>(c => {
var configuration = ConfigurationOptions.Parse(_config
.GetConnectionString("Redis"N, true);
return ConnectionMultiplexer.Connect(configuration);
var app = builder.Build();

app.UseMiddleware<ExceptionMiddleWare>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();//handles any exception in the page
                                    //it gives you information about the errror we usse it in development not production as the user shouldn't know any informaion about the error 
}
else
{
    //app.UseExceptionHandler("/errror");//it tells him when there is an error go to the api of the errors
}

[HttpGet(Name = "GetWeatherForecast")]
public IEnumerable<WeatherForecast> Get()
{
    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
    {
        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        TemperatureC = Random.Shared.Next(-20, 55),
        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
    })
    .ToArray();
}
app.UseStatusCodePagesWithReExecute("/error/{0}");
app.UseHttpsRedirection();
app.UseAuthentication();
//app.UseAuthorization();
//app.UseMiddleware<ApiKeyMiddleWare>();
app.MapControllers();

app.Run();
