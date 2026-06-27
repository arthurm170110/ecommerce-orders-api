namespace EcommerceOrders.Domain.Entities;

public class OrderItem
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public int Quantity { get; private set; }
    
    public decimal TotalValue => Price * Quantity;
    
    private OrderItem() {}

    public OrderItem(string name, decimal price, int quantity)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name cannot be empty.");
        
        if (price <= 0)
            throw new ArgumentException("Price must be greater than zero.");
        
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.");
        
        Id = Guid.NewGuid();
        Name = name;
        Price = price;
        Quantity = quantity;
    }
}