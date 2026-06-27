using EcommerceOrders.Domain.Entities;

namespace EcommerceOrders.Domain.Repositories;

public interface IOrderReadOnlyRepository
{
    Task<Order?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<Order>> GetAllAsync(OrderStatus? status);
    Task UpsertAsync(Order order);
}