using InventoryService.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.AppDbContext
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options) { }

        public DbSet<Inventory> inventory { get; set; }
    }
}
