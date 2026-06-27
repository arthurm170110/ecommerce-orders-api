using EcommerceOrders.Domain.Common;

namespace EcommerceOrders.Domain.Entities;

public class Order
{
    public Guid Id { get; private set; }
    public string Buyer { get; private set; }
    public OrderStatus Status { get; private set; }
    public decimal TotalValue => _items.Sum(item => item.TotalValue);
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private readonly List<OrderItem> _items = new();
    public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();
    
    private Order() {}
    
    private Order(string buyer, List<OrderItem> items)
    {
        Id = Guid.NewGuid();
        Buyer = buyer;
        Status = OrderStatus.Initiated;

        _items.AddRange(items);

        SetCreated();
    }
    public static Result<Order> Create(
        string buyer,
        List<OrderItem> items)
    {
        if (string.IsNullOrWhiteSpace(buyer))
        {
            return Result<Order>.Failure(new Error(ErrorCode.ValidationError,
                    "Buyer is required."));
        }

        if (items is null || !items.Any())
        {
            return Result<Order>.Failure(new Error(ErrorCode.ValidationError,
                    "An order must have at least one item."));
        }

        return Result<Order>.Success(new Order(buyer, items));
    }
    
    private void SetCreated()
    {
        var now = DateTime.UtcNow;

        CreatedAt = now;
        UpdatedAt = now;
    }
    
    private void SetUpdated()
    {
        UpdatedAt = DateTime.UtcNow;
    }

    public Result Update(string buyer, List<OrderItem> newItems)
    {
        if (Status != OrderStatus.Initiated)
            return Result.Failure(new Error(ErrorCode.BusinessRuleViolation,
                "Only unprocessed orders can be modified."));
        
        if (string.IsNullOrWhiteSpace(buyer))
            return Result.Failure(new Error(ErrorCode.ValidationError,
                    "Buyer is required."));

        if (newItems is null || !newItems.Any())
            return Result.Failure(new Error(ErrorCode.ValidationError,
                    "An order must have at least one item."));

        Buyer = buyer;
        _items.Clear();
        _items.AddRange(newItems);
        
        SetUpdated();
        
        return Result.Success();
    }

    public Result Process()
    {
        if (Status != OrderStatus.Initiated)
            return Result.Failure(new Error(ErrorCode.BusinessRuleViolation,
                "Only initiated orders can be processed."));

        Status = OrderStatus.Processed;
        SetUpdated();
        
        return Result.Success();
    }

    public Result Ship()
    {
        if (Status != OrderStatus.Processed)
            return Result.Failure(new Error(ErrorCode.BusinessRuleViolation,
                    "Only processed orders can be shipped."));

        Status = OrderStatus.Sent;

        SetUpdated();

        return Result.Success();
    }

    public Result Cancel()
    {
        if (Status != OrderStatus.Initiated &&
            Status != OrderStatus.Processed)
            return Result.Failure(new Error(ErrorCode.BusinessRuleViolation,
                    "Only initiated or processed orders can be canceled."));

        Status = OrderStatus.Canceled;

        SetUpdated();

        return Result.Success();
    }
}