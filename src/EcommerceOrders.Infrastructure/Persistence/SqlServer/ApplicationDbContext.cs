using EcommerceOrders.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcommerceOrders.Infrastructure.Persistence.SqlServer;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(builder =>
        {
            builder.ToTable("Orders");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.Buyer)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(o => o.Status)
                .HasConversion<int>()
                .IsRequired();
            
            builder.Property(o => o.TotalValue)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(o => o.CreatedAt)
                .IsRequired();

            builder.Property(o => o.UpdatedAt)
                .IsRequired();
            
            builder.Metadata
                .FindNavigation(nameof(Order.Items))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasMany(o => o.Items)
                .WithOne()
                .HasForeignKey("OrderId")
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<OrderItem>(builder =>
        {
            builder.ToTable("OrderItems");

            builder.HasKey(oi => oi.Id);

            builder.Property(oi => oi.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(oi => oi.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            
            builder.Property(oi => oi.Quantity)
                .IsRequired();
            
            builder.Property(oi => oi.TotalValue)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            
            builder.Property<Guid>("OrderId");
        });
    }
}