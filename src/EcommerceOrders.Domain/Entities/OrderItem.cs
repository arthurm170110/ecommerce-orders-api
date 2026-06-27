namespace EcommerceOrders.Domain.Entities;

public class OrderItem
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    
    private OrderItem() {}

    public OrderItem(string name, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name cannot be empty.");
        
        if (price <= 0)
            throw new ArgumentException("Price must be greater than zero.");
        
        Id = Guid.NewGuid();
        Name = name;
        Price = price;
    }
}