using EcommerceOrders.Application.Dtos.Requests;
using EcommerceOrders.Application.Dtos.Responses;
using EcommerceOrders.Domain.Common;
using EcommerceOrders.Domain.Entities;

namespace EcommerceOrders.Application.Services;

public interface IOrderService
{
    Task<Result<OrderResponse>> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken = default);
    Task<Result<OrderResponse>> UpdateOrderAsync(Guid id, UpdateOrderRequest request, CancellationToken cancellationToken = default);
    Task<Result> CancelOrderAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<OrderResponse>> GetOrderByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<OrderResponse>>> GetOrdersAsync(OrderStatus? statusFilter, CancellationToken cancellationToken = default);
}