namespace EcommerceOrders.Application.Dtos.Requests;

public record UpdateOrderRequest(
    String Buyer,
    List<OrderItemRequest> Items);