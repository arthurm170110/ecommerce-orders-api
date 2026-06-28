using Microsoft.AspNetCore.Mvc;
using EcommerceOrders.Application.Services;
using EcommerceOrders.Application.Dtos.Requests;
using EcommerceOrders.Domain.Entities;
using EcommerceOrders.Domain.Common;

namespace EcommerceOrders.API.Endpoints;

public static class OrderEndpoints
{
    public static void MapOrderEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/orders").WithOpenApi();

        // POST - Criar Pedido
        group.MapPost("/", async (
            [FromBody] CreateOrderRequest request, 
            [FromServices] IOrderService orderService,
            CancellationToken cancellationToken) =>
        {
            var result = await orderService.CreateOrderAsync(request, cancellationToken);

            if (result.IsFailure)
                return HandleFailure(result.Error);

            return Results.Created($"/api/v1/orders/{result.Value.OrderId}", result.Value);
        })
        .WithName("CreateOrder")
        .WithSummary("Cria um novo pedido.");

        // GET - Listar com Filtro Opcional de Status
        group.MapGet("/", async (
            [FromQuery] OrderStatus? status, 
            [FromServices] IOrderService orderService,
            CancellationToken cancellationToken) =>
        {
            var result = await orderService.GetOrdersAsync(status, cancellationToken);

            if (result.IsFailure)
                return HandleFailure(result.Error);

            return Results.Ok(result.Value);
        })
        .WithName("GetOrders")
        .WithSummary("Lista pedidos cadastrados.");

        // GET - Buscar por ID
        group.MapGet("/{id:guid}", async (
            Guid id, 
            [FromServices] IOrderService orderService,
            CancellationToken cancellationToken) =>
        {
            var result = await orderService.GetOrderByIdAsync(id, cancellationToken);

            if (result.IsFailure)
                return HandleFailure(result.Error);

            return Results.Ok(result.Value);
        })
        .WithName("GetOrderById");

        // PUT - Atualizar Pedido
        group.MapPut("/{id:guid}", async (
            Guid id, 
            [FromBody] UpdateOrderRequest request, 
            [FromServices] IOrderService orderService,
            CancellationToken cancellationToken) =>
        {
            var result = await orderService.UpdateOrderAsync(id, request, cancellationToken);

            if (result.IsFailure)
                return HandleFailure(result.Error);

            return Results.Ok(result.Value);
        })
        .WithName("UpdateOrder");

        // PATCH - Cancelar Pedido
        group.MapPatch("/{id:guid}/cancel", async (
            Guid id, 
            [FromServices] IOrderService orderService,
            CancellationToken cancellationToken) =>
        {
            var result = await orderService.CancelOrderAsync(id, cancellationToken);

            if (result.IsFailure)
                return HandleFailure(result.Error);

            return Results.NoContent();
        })
        .WithName("CancelOrder");
    }
    
    private static IResult HandleFailure(Error error)
    {
        return error.Code switch
        {
            ErrorCode.OrderNotFound => Results.Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: error.Code.ToString(),
                detail: error.Message),

            ErrorCode.InternalError => Results.Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: error.Code.ToString(),
                detail: error.Message),
            
            _ => Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: error.Code.ToString(),
                detail: error.Message)
        };
    }
}