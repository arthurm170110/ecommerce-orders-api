namespace EcommerceOrders.Application.Dtos.Requests;

public record OrderItemRequest(
    string ProductName,
    decimal Price,
    int Quantity);