
using UserService.Exceptions;
using UserService.Data;
using Models;
using UserService.services;
using UserService.DTOs.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Data;
//using Stripe;
//using Stripe;
//using Stripe;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
//builder.Services.AddIdentity();
builder.Services.AddEndpointsApiExplorer();//This method registers services required for discovering and describing API endpoints within the application. It is part of the configuration for setting up API documentation tools, such as Swagger.
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();
builder.Services.AddDbContext<IdentityContext>(options=>options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityContext")));

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 10;
    options.Password.RequiredUniqueChars = 3;

    //options.SignIn.RequireConfirmedEmail = true;
})
.AddEntityFrameworkStores<IdentityContext>()//configures the Identity system to use Entity Framework Core to store and retrieve identity data.
.AddDefaultTokenProviders();
builder.Services.AddSingleton<EmailConfiguration>();
builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<jwtConfig>(builder.Configuration.GetSection("JwtConfig"));
var jwtConfig = builder.Configuration.GetSection("JwtConfig").Get<jwtConfig>();
var key = Encoding.ASCII.GetBytes(jwtConfig.Secret);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = jwtConfig.Issuer,
        ValidAudience = jwtConfig.Audience
    };
});

builder.Services.AddAuthentication()
    .AddGoogle(o =>
    {
        IConfigurationSection gg = builder.Configuration.GetSection("Google");
        o.ClientId = gg["ClientId"];
        o.ClientSecret = gg["ClientSecret"];
    });

// --> Bring Object Of IdentityContext For Update His Migration
//var _identiyContext = builder.Services.GetRequiredService<IdentityContext>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddExceptionHandler<AppExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<ITokenService,TokenService>();
builder.Services.AddSingleton<jwtConfig>();

builder.Services.AddSingleton<TokenValidationParameters>();

builder.Services.AddSingleton<IPhotoService,CloudinaryService> ();


// Migrate IdentityContext
//await _identiyContext.Database.MigrateAsync();
// Seeding Data For IdentityDbContext
// -- but this seeding function create users, so it need to take object from UserManager not IdentityContext
// -- So we will add dependancy injection then ask clr to create object from UserManager
//var _userManager = builder.Services.GetRequiredService<UserManager<AppUser>>();
//await IdentityContextSeed.SeedUsersAsync(_userManager);

builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-XSRF-TOKEN"; 
    options.Cookie.Name = "MyApp.Antiforgery"; 
    options.Cookie.HttpOnly = true; 
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; 
    options.Cookie.MaxAge = TimeSpan.FromMinutes(30); 
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.UseExceptionHandler(_ => { });
app.UseHttpsRedirection();
app.UseAuthentication();//to add claims identity
app.UseAuthorization();

app.MapControllers();

app.Run();
