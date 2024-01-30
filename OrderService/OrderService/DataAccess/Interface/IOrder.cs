using OrderService.Models;

namespace OrderService.DataAccess.Interface
{
    public interface IOrder
    {
        Task<IEnumerable<Order>> GetAllOrders();
        void addOrder(Order order);
    }
}
