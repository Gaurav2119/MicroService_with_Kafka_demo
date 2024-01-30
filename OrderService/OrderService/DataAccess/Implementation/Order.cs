using Microsoft.EntityFrameworkCore;
using OrderService.AppDbContext;
using OrderService.DataAccess.Interface;

namespace OrderService.DataAccess.Implementation
{
    public class Order : IOrder
    {
        private readonly OrderDbContext _orderdbcontext;
        public Order(OrderDbContext orderdBContext)
        {
           _orderdbcontext = orderdBContext;
        }
        public void addOrder(Models.Order order)
        {
            try
            {
                if (order == null) throw new ArgumentNullException(nameof(order));
                _orderdbcontext.orders.Add(order);
                _orderdbcontext.SaveChanges();
            }

            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<IEnumerable<Models.Order>> GetAllOrders()
        {
            return await _orderdbcontext.orders.ToListAsync();
        }
    }
}
