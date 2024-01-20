using System.ComponentModel.DataAnnotations;

namespace OrderService.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Product Id")]
        public int productId { get; set; }

        [Display(Name = "Quantity ordered")]
        public int quantity { get; set; }
    }
}
