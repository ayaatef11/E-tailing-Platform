﻿
using Models;

namespace UserService.Extensions
{
    public static  class IdentityServiceExtension
    {
        public static IServiceCollection AddIdentity(this IServiceCollection services)
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
