using Microsoft.EntityFrameworkCore;
using OrderService.AppDbContext;
using OrderService.DataAccess.Interface;

namespace OrderService.DataAccess.Implementation
{
    public class Order : IOrder
    {
        private readonly DBContext _dbcontext;
        public Order(DBContext dBContext)
        {
           _dbcontext = dBContext;
        }
        public void addOrder(Models.Order order)
        {
            try
            {
                if (order == null) throw new ArgumentNullException(nameof(order));
                _dbcontext.orders.Add(order);
                _dbcontext.SaveChanges();
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
            return await _dbcontext.orders.ToListAsync();
        }
    }
}
