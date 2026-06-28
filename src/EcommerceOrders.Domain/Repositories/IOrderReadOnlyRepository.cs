using EcommerceOrders.Domain.Entities;

namespace EcommerceOrders.Domain.Repositories;

public interface IOrderReadOnlyRepository
{
    Task<OrderReadModel?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<OrderReadModel>> GetAllAsync(OrderStatus? status);
    Task UpsertAsync(Order order);
}