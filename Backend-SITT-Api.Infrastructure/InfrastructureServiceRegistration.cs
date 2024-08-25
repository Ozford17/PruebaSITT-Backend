using Backend_SITT_Api.Infrastructure.Persistence;
using Backend_SITT_Api.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Backend_SITT_Api.Aplication.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend_SITT_Api.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServiceRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BackendSittDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("connectionString")));

            services.AddScoped(typeof(ITaskRepository), typeof( TaskRepository));
            return services;
        }
    }
}
