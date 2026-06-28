using EcommerceOrders.Application.Dtos.Requests;
using EcommerceOrders.Application.Dtos.Responses;
using EcommerceOrders.Domain.Common;
using EcommerceOrders.Domain.Entities;
using EcommerceOrders.Domain.Repositories;

namespace EcommerceOrders.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _writeRepository;
    private readonly IOrderReadOnlyRepository _readRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(IOrderRepository writeRepository, IOrderReadOnlyRepository readRepository, IUnitOfWork unitOfWork)
    {
        _writeRepository = writeRepository;
        _readRepository = readRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<OrderResponse>> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var itemResults = request.Items
                .Select(i => OrderItem.Create(
                    i.ProductName,
                    i.Price,
                    i.Quantity))
                .ToList();

            if (itemResults.Any(r => r.IsFailure))
            {
                var error = itemResults.First(r => r.IsFailure).Error;
                return Result<OrderResponse>.Failure(error);
            }

            var items = itemResults
                .Select(r => r.Value)
                .ToList();

            var orderResult = Order.Create(request.Buyer, items);

            if (orderResult.IsFailure)
                return Result<OrderResponse>.Failure(orderResult.Error);

            var order = orderResult.Value;

            await _writeRepository.AddAsync(order, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _readRepository.UpsertAsync(order);

            return Result<OrderResponse>.Success(
                MapToResponse(order));
        }
        catch (Exception)
        {
            return Result<OrderResponse>.Failure(new Error(ErrorCode.InternalError,
                    "Unexpected error occurred while creating order."));
        }
    }

    public async Task<Result<OrderResponse>> UpdateOrderAsync(Guid id, UpdateOrderRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var order = await _writeRepository.GetByIdAsync(id, cancellationToken);

            if (order is null)
                return Result<OrderResponse>.Failure(new Error(ErrorCode.OrderNotFound, "Order not found."));

            var itemResults = request.Items
                .Select(i => OrderItem.Create(
                    i.ProductName,
                    i.Price,
                    i.Quantity))
                .ToList();

            if (itemResults.Any(r => r.IsFailure))
            {
                var error = itemResults.First(r => r.IsFailure).Error;
                return Result<OrderResponse>.Failure(error);
            }

            var items = itemResults
                .Select(r => r.Value)
                .ToList();

            var result = order.Update(request.Buyer, items);

            if (result.IsFailure)
                return Result<OrderResponse>.Failure(result.Error);
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _readRepository.UpsertAsync(order);

            return Result<OrderResponse>.Success(MapToResponse(order));
        }
        catch (Exception)
        {
            return Result<OrderResponse>.Failure(new Error(ErrorCode.InternalError,
                    "Unexpected error occurred while updating order."));
        }
    }
    
    public async Task<Result> CancelOrderAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var order = await _writeRepository.GetByIdAsync(id, cancellationToken);

            if (order is null)
                return Result.Failure(new Error(ErrorCode.OrderNotFound, "Order not found."));

            var result = order.Cancel();

            if (result.IsFailure)
                return Result.Failure(result.Error);
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _readRepository.UpsertAsync(order);

            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure(new Error(ErrorCode.InternalError,
                    "Unexpected error occurred while canceling order."));
        }
    }

    public async Task<Result<OrderResponse>> GetOrderByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var order = await _readRepository.GetByIdAsync(id);

        if (order is null)
            return Result<OrderResponse>.Failure(new Error(ErrorCode.OrderNotFound, "Order not found."));

        return Result<OrderResponse>.Success(
            MapToResponse(order));
    }
    
    public async Task<Result<IReadOnlyList<OrderResponse>>> GetOrdersAsync(OrderStatus? statusFilter, CancellationToken cancellationToken = default)
    {
        var orders = await _readRepository.GetAllAsync(statusFilter);

        var result = orders
            .Select(model => MapToResponse(model))
            .ToList();

        return Result<IReadOnlyList<OrderResponse>>.Success(result);
    }
    
    private static OrderResponse MapToResponse(Order order)
    {
        return new OrderResponse(
            order.Id,
            order.Buyer,
            order.Status.ToString(),
            order.TotalValue,
            order.Items
                .Select(i => new OrderItemResponse(
                    i.Id,
                    i.Name,
                    i.Price,
                    i.Quantity,
                    i.TotalValue))
                .ToList());
    }
    
    private static OrderResponse MapToResponse(OrderReadModel model)
    {
        return new OrderResponse(
            model.Id,
            model.Buyer,
            model.Status, 
            model.TotalValue,
            model.Items
                .Select(i => new OrderItemResponse(
                    i.Id,
                    i.Name,
                    i.Price,
                    i.Quantity,
                    i.TotalValue))
                .ToList());
    }
}