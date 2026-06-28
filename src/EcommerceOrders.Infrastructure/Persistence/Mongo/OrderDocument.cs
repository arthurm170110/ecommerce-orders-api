using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EcommerceOrders.Infrastructure.Persistence.Mongo;

public class OrderDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }

    public string Buyer { get; set; } = string.Empty;
    public int Status { get; set; }
    public decimal TotalValue { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<OrderItemDocument> Items { get; set; } = [];
}

public class OrderItemDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal TotalValue { get; set; }
}