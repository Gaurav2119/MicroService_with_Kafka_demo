using InventoryService.AppDbContext;
using InventoryService.DataAccess.Interface;
using InventoryService.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.DataAccess.Implementation
{
    public class Inventory : IInventory
    {
        private readonly DBContext _dbContext;
        public Inventory(DBContext context)
        {
            _dbContext = context;
        }

        public void addProduct(Models.Inventory inventory)
        {
            if (inventory == null) throw new ArgumentNullException(nameof(inventory));
            _dbContext.inventory.Add(inventory);
            _dbContext.SaveChanges();
        }

        public async Task<IEnumerable<Models.Inventory>> GetAllProduct()
        {
            return await _dbContext.inventory.ToListAsync();
        }

        public IEnumerable<Models.Inventory> GetAllProductsync()
        {
            return _dbContext.inventory.ToList();
        }

        public async Task<Models.Inventory?> GetProduct(int id)
        {
            return await _dbContext.inventory.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
