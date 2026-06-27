using EcommerceOrders.Domain.Entities;

namespace EcommerceOrders.Domain.Repositories;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(Order order, CancellationToken cancellationToken);
}