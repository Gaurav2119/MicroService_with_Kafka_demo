using InventoryService.AppDbContext;
using InventoryService.DataAccess.Interface;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.DataAccess.Implementation
{
    public class Inventory : IInventory
    {
        private readonly productDbContext _productDbContext;
        public Inventory(productDbContext context)
        {
            _productDbContext = context;
        }

        public void addProduct(Models.Inventory inventory)
        {
            ArgumentNullException.ThrowIfNull(inventory, nameof(inventory));
            _productDbContext.inventory.Add(inventory);
            _productDbContext.SaveChanges();
        }

        public async Task<IEnumerable<Models.Inventory>> GetAllProduct()
        {
            return await _productDbContext.inventory.ToListAsync();
        }

        public async Task<Models.Inventory?> GetProduct(Guid id)
        {
            return await _productDbContext.inventory.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
