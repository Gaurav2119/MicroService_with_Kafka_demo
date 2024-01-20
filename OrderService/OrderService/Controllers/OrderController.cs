using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.DataAccess.Interface;
using OrderService.KafkaConsumer;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrder _order;
        public OrderController(IOrder order)
        {
            _order = order;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            return Ok(await _order.GetAllOrders());
        }
    }
}
