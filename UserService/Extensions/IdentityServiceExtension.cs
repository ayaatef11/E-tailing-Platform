using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using WebApplication1.Models;

namespace UserService.Extensions
{
    public class IdentityServiceExtension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {

            var builder = services.AddIdentityCore<AppUser>();

            builder = new IdentityBuilder(builder.UserType, builder.Services);
            builder.AddEntityFrameworkStores<IdentityDbContext>();
            builder.AddSignInManager<SignInManager<AppUser>>();
            services.AddAuthentication();
            return services;
        }
    }
}
