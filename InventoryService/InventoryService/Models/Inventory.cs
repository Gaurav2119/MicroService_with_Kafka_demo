using System.ComponentModel.DataAnnotations;

namespace InventoryService.Models
{
    public class Inventory
    {
        public Guid Id { get; set; }

        [Display(Name = "Product Name")]
        public string? productName { get; set; }

        [Display(Name = "Product quantity in stock")]
        public int quantity { get; set; }
    }
}