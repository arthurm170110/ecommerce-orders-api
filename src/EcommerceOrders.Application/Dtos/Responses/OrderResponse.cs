namespace EcommerceOrders.Application.Dtos.Responses;

public record OrderResponse(
    Guid OrderId,
    string Buyer,
    string Status,
    decimal TotalAmount,
    List<OrderItemResponse> Items);