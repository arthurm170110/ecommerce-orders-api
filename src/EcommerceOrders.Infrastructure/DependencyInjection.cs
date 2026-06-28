using EcommerceOrders.Domain.Repositories;
using EcommerceOrders.Infrastructure.Persistence.Mongo;
using EcommerceOrders.Infrastructure.Persistence.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace EcommerceOrders.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<MongoOptions>(options =>
        {
            options.DatabaseName = configuration["MongoOptions:DatabaseName"] ?? "EcommerceOrdersReadDb";
        });
        
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("SqlServer")));
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddSingleton<IMongoClient>(_ =>
            new MongoClient(configuration.GetConnectionString("Mongo")));

        services.AddSingleton<MongoDbContext>();
        
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderReadOnlyRepository, OrderReadOnlyRepository>();

        return services;
    }
}