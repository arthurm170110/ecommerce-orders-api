using EcommerceOrders.Domain.Entities;
using EcommerceOrders.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EcommerceOrders.Infrastructure.Persistence.SqlServer;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _dbContext;
    
    public OrderRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task AddAsync(Order order, CancellationToken cancellationToken)
    {
        await _dbContext.Orders.AddAsync(order, cancellationToken);
    }
}