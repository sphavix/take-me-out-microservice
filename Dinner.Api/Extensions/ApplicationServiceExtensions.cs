using Dinner.Api.Persistence;
using Dinner.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace Dinner.Api.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddCors();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

           services.AddScoped<ITokenService, TokenService>();
            return services;
        }
    }
}
