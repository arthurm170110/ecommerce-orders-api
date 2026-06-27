namespace EcommerceOrders.Application.Dtos.Responses;

public record OrderItemResponse(
    Guid ItemId,
    string ProductName,
    decimal Price,
    int Quantity,
    decimal TotalPrice);