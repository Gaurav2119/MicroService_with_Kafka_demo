using InventoryService.Models;

namespace InventoryService.DataAccess.Interface
{
    public interface IInventory
    {
        Task<IEnumerable<Inventory>> GetAllProduct();

        void addProduct(Inventory inventory);

        Task<Inventory?> GetProduct(int id);

        IEnumerable<Inventory> GetAllProductsync();
    }
}
