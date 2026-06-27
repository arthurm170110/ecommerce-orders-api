namespace EcommerceOrders.Application.Dtos.Requests;

public record CreateOrderRequest(
    String Buyer,
    List<OrderItemRequest> Items);