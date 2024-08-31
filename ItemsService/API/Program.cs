
using API.Errors;
using API.Extesnions;
using API.MiddleWares;
using Core.interfaces.Repositories;
using Repository;
using Repository.Data;
using Repository.Repositories;
using StackExchange.Redis;
//using Stripe;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSwaggerServices();

/*builder.Services.AddDbContext<StoreContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")*//*, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure();
    });*//*
);*/

builder.Services.AddDbContext<StoreContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


builder.Services.AddSingleton<IConnectionMultiplexer>(serviceProvider =>
{
    var connection = builder.Configuration.GetConnectionString("Redis");
    return ConnectionMultiplexer.Connect(connection);
});

//builder.Services.AddApplicationServices();

builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    // collect the errors in an array
    options.InvalidModelStateResponseFactory = (actionContext) =>
    {
        var errors = actionContext.ModelState.Where(P => P.Value!.Errors.Count > 0)
        
        .SelectMany(P => P.Value.Errors)
       
        .Select(E => E.ErrorMessage)
        .ToArray();
      
        var validationErrorResponse = new ApiValidationErrorResponse()
        {
            Errors = errors
        };
        
        return new BadRequestObjectResult(validationErrorResponse);
    };
});


var app = builder.Build();

using var scope = app.Services.CreateScope();

var services = scope.ServiceProvider;

var _storeContext = services.GetRequiredService<StoreContext>();
  
var loggerFactory = services.GetRequiredService<ILoggerFactory>();

try
{
   
    await _storeContext.Database.MigrateAsync();
    
    await StoreContextSeed.SeedProductDataAsync(_storeContext);

}
catch (Exception ex)
{
    var logger = loggerFactory.CreateLogger<Program>();
    logger.LogError(ex, "an error has been occured during apply the migration");
}

app.UseMiddleware<ExceptionMiddleWare>();

if (app.Environment.IsDevelopment())
{

    app.UseSwaggerMiddleware();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseStatusCodePagesWithReExecute("/error/{0}");

app.MapControllers(); // -> we use this middleware to talk program that: your routing depend on route written on the controller

app.UseAuthentication();

app.UseAuthorization();

app.Run();