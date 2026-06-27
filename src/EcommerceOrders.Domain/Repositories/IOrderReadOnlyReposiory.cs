using EcommerceOrders.Domain.Entities;

namespace EcommerceOrders.Domain.Repositories;

public interface IOrderReadOnlyReposiory
{
    Task<Order?> GetByIdAsync(Guid id);
    Task<IEnumerable<Order>> GetAllAsync(OrderStatus? status);
    Task UpsertAsync(Order order);
}