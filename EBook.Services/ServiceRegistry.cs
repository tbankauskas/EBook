using EBook.Data.Entities;
using EBook.Services.Identity;
using EBook.Services.Jwt;
using EBook.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EBook.Services
{
    public static class ServiceRegistry
    {
        public static void Configure(IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<SignInManager<User>>();
            services.AddTransient<IUserStore<User>, UserStore>();
            services.AddIdentityCore<User>().AddDefaultTokenProviders();
            services.AddScoped<JwtTokenFactory>();

            services.AddScoped<AuthenticationService>();
            services.AddScoped<BooksService>();
            services.AddScoped<SubscriptionsService>();
        }
    }
}
