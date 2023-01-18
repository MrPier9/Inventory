using API.Data;
using API.RequestHelpers;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddAutoMapper(typeof(MappingProfile).Assembly);
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddDbContext<InventoryContext>(opt =>
            {
                opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });


            return services;
        }
    }
}