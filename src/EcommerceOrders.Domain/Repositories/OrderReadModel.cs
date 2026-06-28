namespace EcommerceOrders.Domain.Repositories;

public class OrderReadModel
{
    public Guid Id { get; set; }
    public string Buyer { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal TotalValue { get; set; }
    public List<OrderItemReadModel> Items { get; set; } = [];
}

public class OrderItemReadModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal TotalValue { get; set; }
}