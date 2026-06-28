using EcommerceOrders.Application.Dtos.Requests;
using EcommerceOrders.Application.Services;
using EcommerceOrders.Domain.Common;
using EcommerceOrders.Domain.Entities;
using EcommerceOrders.Domain.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace EcommerceOrders.UnitTests.Services;

public class OrderServiceTests
{
    private readonly IOrderRepository _writeRepository;
    private readonly IOrderReadOnlyRepository _readRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly OrderService _service;

    public OrderServiceTests()
    {
        _writeRepository = Substitute.For<IOrderRepository>();
        _readRepository = Substitute.For<IOrderReadOnlyRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        
        _service = new OrderService(_writeRepository, _readRepository, _unitOfWork,Substitute.For<IMemoryCache>(), 
            Substitute.For<ILogger<OrderService>>());
    }

    [Fact]
    public async Task CreateOrder_Should_ReturnSuccess_When_DataIsValid()
    {
        var request = new CreateOrderRequest("Comprador Teste", new List<OrderItemRequest> { 
            new OrderItemRequest("Produto 1", 100, 1) 
        });
        
        var result = await _service.CreateOrderAsync(request);
        
        result.IsSuccess.Should().BeTrue();
        await _writeRepository.Received(1).AddAsync(Arg.Any<Order>(), Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task UpdateOrder_Should_ReturnFailure_When_OrderDoesNotExist()
    {
        var nonExistentId = Guid.NewGuid();
        var request = new UpdateOrderRequest("Novo Comprador", new List<OrderItemRequest>());
        
        _writeRepository.GetByIdAsync(nonExistentId, Arg.Any<CancellationToken>())
            .Returns((Order?)null);
        
        var result = await _service.UpdateOrderAsync(nonExistentId, request);
        
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be(ErrorCode.OrderNotFound);
    }

    [Fact]
    public async Task CancelOrder_Should_ReturnFailure_When_OrderDoesNotExist()
    {
        var id = Guid.NewGuid();
        _writeRepository.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns((Order?)null);
        
        var result = await _service.CancelOrderAsync(id);
        
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be(ErrorCode.OrderNotFound);
    }
}