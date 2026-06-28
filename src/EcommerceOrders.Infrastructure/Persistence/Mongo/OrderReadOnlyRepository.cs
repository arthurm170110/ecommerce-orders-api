using EcommerceOrders.Domain.Entities;
using EcommerceOrders.Domain.Repositories;
using MongoDB.Driver;

namespace EcommerceOrders.Infrastructure.Persistence.Mongo;

public class OrderReadOnlyRepository : IOrderReadOnlyRepository
{
    private readonly IMongoCollection<OrderDocument> _collection;

    public OrderReadOnlyRepository(MongoDbContext context)
    {
        _collection = context.GetCollection<OrderDocument>("orders_projections");
    }

    public async Task<OrderReadModel?> GetByIdAsync(Guid id)
    {
        var doc = await _collection.Find(o => o.Id == id).FirstOrDefaultAsync();
        if (doc == null) return null;

        return MapToReadModel(doc);
    }

    public async Task<IReadOnlyList<OrderReadModel>> GetAllAsync(OrderStatus? status)
    {
        var query = status.HasValue
            ? _collection.Find(o => o.Status == (int)status.Value) 
            : _collection.Find(_ => true);

        var docs = await query.ToListAsync();
        return docs.Select(MapToReadModel).ToList();
    }

    public Task UpsertAsync(Order order)
    {
        var document = new OrderDocument
        {
            Id = order.Id,
            Buyer = order.Buyer,
            Status = (int)order.Status,
            TotalValue = order.TotalValue,
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt,
            Items = order.Items.Select(i => new OrderItemDocument
            {
                Id = i.Id,
                Name = i.Name,
                Price = i.Price,
                Quantity = i.Quantity,
                TotalValue = i.TotalValue
            }).ToList()
        };

        var filter = Builders<OrderDocument>.Filter.Eq(o => o.Id, document.Id);
        return _collection.ReplaceOneAsync(filter, document, new ReplaceOptions { IsUpsert = true });
    }
    
    private static OrderReadModel MapToReadModel(OrderDocument doc)
    {
        return new OrderReadModel
        {
            Id = doc.Id,
            Buyer = doc.Buyer,
            Status = ((OrderStatus)doc.Status).ToString(),
            TotalValue = doc.TotalValue,
            Items = doc.Items.Select(i => new OrderItemReadModel
            {
                Id = i.Id,
                Name = i.Name,
                Price = i.Price,
                Quantity = i.Quantity,
                TotalValue = i.TotalValue
            }).ToList()
        };
    }
}