using System.ComponentModel.DataAnnotations;

namespace OrderService.Models
{
    public class Order
    {
        public Guid Id { get; set; }

        [Display(Name = "Product Id")]
        public Guid productId { get; set; }

        [Display(Name = "Quantity ordered")]
        public int quantity { get; set; }
    }
}
