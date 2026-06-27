using EcommerceOrders.Domain.Repositories;

namespace EcommerceOrders.Infrastructure.Persistence.SqlServer;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    
    public UnitOfWork(ApplicationDbContext dbContext) => _dbContext = dbContext;
    
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken) => _dbContext.SaveChangesAsync(cancellationToken);
}