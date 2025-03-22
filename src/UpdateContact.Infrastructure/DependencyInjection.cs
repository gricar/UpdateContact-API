using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UpdateContact.Application.Persistence;
using UpdateContact.Infrastructure.Repositories;

namespace UpdateContact.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection ConfigurePersistenceServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("Database")));

        services.AddScoped<IContactRepository, ContactRepository>();

        return services;
    }
}
