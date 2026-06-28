using EcommerceOrders.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EcommerceOrders.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IOrderService, OrderService>();
        return services;
    }
}