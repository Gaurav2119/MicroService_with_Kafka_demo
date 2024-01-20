using Microsoft.EntityFrameworkCore;
using OrderService.Models;

namespace OrderService.AppDbContext
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options) { }

        public DbSet<Order> orders { get; set; }
    }
}
