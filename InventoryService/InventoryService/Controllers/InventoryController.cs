using InventoryService.DataAccess.Interface;
using InventoryService.KafkaProducer;
using InventoryService.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InventoryService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventory inventory;
        private readonly IProducer producer;
        public InventoryController(IInventory inventory, IProducer producer)
        {
            this.inventory = inventory;
            this.producer = producer;
        }

        // GET: api/<InventoryController>
        [HttpGet]
        public async Task<IActionResult> GetAllProduct()
        {
            var products = await this.inventory.GetAllProduct();
            return Ok(products);
        }

        [HttpGet("sync")]
        public IActionResult GetAllProductsync()
        {
            var products = this.inventory.GetAllProductsync();
            return Ok(products);
        }

        // GET api/<InventoryController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var product = await this.inventory.GetProduct(id);
            if (product != null)
            {
                return Ok(product);
            }
            return BadRequest("Product not found");
        }

        // POST api/<InventoryController>
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] Inventory inventory)
        {
            try
            {
                this.inventory.addProduct(inventory);

                return Ok(await GetAllProduct());
            }

            catch (ArgumentNullException ex) { return BadRequest($"Error: { ex.Message}"); }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        // PUT api/<InventoryController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<InventoryController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpPost("PlaceOrder/{ProductId}")]
        public async Task<IActionResult> PlaceOrder(int ProductId)
        {
            var product = await this.inventory.GetProduct(ProductId);
            if (product != null)
            {
                var message = new
                {
                    product.quantity
                };
                await this.producer.ProduceMessage(JsonConvert.SerializeObject(product.Id), 
                    "inventory-topic", 
                    JsonConvert.SerializeObject(message));
                
                return Ok("Order Placed");
            }
            return BadRequest("Order cannot be placed for this product");
        }
    }
}
