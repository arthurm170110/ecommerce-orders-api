using EcommerceOrders.Domain.Common;

namespace EcommerceOrders.Domain.Entities;

public class OrderItem
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public int Quantity { get; private set; }
    
    public decimal TotalValue { get; private set; }
    
    private OrderItem() {}

    private OrderItem(
        string name,
        decimal price,
        int quantity)
    {
        Id = Guid.NewGuid();
        Name = name;
        Price = price;
        Quantity = quantity;
        TotalValue = price * quantity;
    }
    
    public static Result<OrderItem> Create(
        string name,
        decimal price,
        int quantity)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result<OrderItem>.Failure(new Error(ErrorCode.ValidationError,
                    "Product name cannot be empty."));
        }

        if (price <= 0)
        {
            return Result<OrderItem>.Failure(new Error(ErrorCode.ValidationError,
                    "Price must be greater than zero."));
        }

        if (quantity <= 0)
        {
            return Result<OrderItem>.Failure(new Error(ErrorCode.ValidationError,
                    "Quantity must be greater than zero."));
        }

        return Result<OrderItem>.Success(new OrderItem(name, price, quantity));
    }
}