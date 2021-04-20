using DemoApp.Data;
using DemoApp.Interfaces;
using DemoApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DemoApp.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<ITokenService, TokenService>();

            services.AddDbContext<DataContext>(options =>
            {
                options.UseMySQL(config.GetSection("ConnectionStrings")["DefaultConnection"]);
                // short hand
                // options.UseMySQL(this._config.GetConnectionString("DefaultConnection"));
            });

            return services;
        }
    }
}