using InventoryService.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.AppDbContext
{
    public class productDbContext : DbContext
    {
        public productDbContext(DbContextOptions<productDbContext> options) : base(options) { }

        public DbSet<Inventory> inventory { get; set; }
    }
}
