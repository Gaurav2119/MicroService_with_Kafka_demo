using Microsoft.EntityFrameworkCore;
using OrderService.Models;

namespace OrderService.AppDbContext
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }

        public DbSet<Order> orders { get; set; }
    }
}
