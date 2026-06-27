using EcommerceOrders.Domain.Entities;
using EcommerceOrders.Domain.Repositories;
using MongoDB.Driver;

namespace EcommerceOrders.Infrastructure.Persistence.Mongo;

public class OrderReadOnlyRepository : IOrderReadOnlyRepository
{
    private readonly IMongoCollection<Order> _collection;

    public OrderReadOnlyRepository(MongoDbContext context)
    {
        _collection = context.GetCollection<Order>("orders_projections");
    }

    public Task<Order?> GetByIdAsync(Guid id) => _collection.Find(o => o.Id == id).FirstOrDefaultAsync();

    public async Task<IReadOnlyList<Order>> GetAllAsync(OrderStatus? status)
    {
        var query = status.HasValue
            ? _collection.Find(o => o.Status == status.Value)
            : _collection.Find(_ => true);

        return await query.ToListAsync();
    }

    public Task UpsertAsync(Order order)
    {
        var filter = Builders<Order>.Filter.Eq(o => o.Id, order.Id);

        return _collection.ReplaceOneAsync(
            filter,
            order,
            new ReplaceOptions { IsUpsert = true });
    }
}