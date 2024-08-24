using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using WebApplication1.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using WebApplication1.Models;
using UserService.Exceptions;
using UserService.Data;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var jwtConfig=builder.Configuration.GetSection("JwtConfig").Get<jwtConfig>();
builder.Services.AddSingleton(jwtConfig);

var key = Encoding.ASCII.GetBytes(jwtConfig.Secret);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtConfig.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtConfig.Audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 10;
    options.Password.RequiredUniqueChars = 3;

    options.SignIn.RequireConfirmedEmail = true;
})
.AddEntityFrameworkStores<IdentityContext>()
.AddDefaultTokenProviders();

builder.Services.Configure<DataProtectionTokenProviderOptions>(o =>
o.TokenLifespan = TimeSpan.FromHours(5));
builder.Services.AddIdentity<AppUser, IdentityRole>(o =>
{
    o.Password.RequiredLength = 10;
    o.Password.RequiredUniqueChars = 3;
    o.SignIn.RequireConfirmedEmail = true;
}).AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();

//builder.Services.Configure<jwtConfig>(builder.Configuration.GetSection("jwtConfig"));

//var key = Encoding.ASCII.GetBytes(builder.Configuration["jwtConfig:Secret"]);
/*builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserPolicy", policy => policy.RequireRole("User"));
    options.AddPolicy("ViewReportsPolicy", policy => policy.RequireClaim("Permission", "ViewReports"));
    options.AddPolicy("EditReportsPolicy", policy => policy.RequireClaim("Permission", "EditReports"));
});*/

builder.Services.AddExceptionHandler<AppExceptionHandler>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseExceptionHandler(_ => { });
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
