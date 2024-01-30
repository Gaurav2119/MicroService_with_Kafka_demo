using FluentValidation;
using FluentValidation.Results;
using InventoryService.DataAccess.Interface;
using InventoryService.KafkaProducer;
using InventoryService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InventoryService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventory _inventory;
        private readonly IProducer _producer;
        public InventoryController(IInventory inventory, IProducer producer)
        {
            _inventory = inventory;
            _producer = producer;
        }

        // GET: api/<InventoryController>
        [HttpGet]
        public async Task<IActionResult> GetAllProduct()
        {
            var products = await _inventory.GetAllProduct();
            return Ok(products);
        }
        
        // GET api/<InventoryController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var product = await _inventory.GetProduct(id);
            if (product != null)
            {
                return Ok(product);
            }
            return BadRequest("Product not found");
        }

        // POST api/<InventoryController>
        [HttpPost]
        public async Task<IActionResult> AddProduct(Inventory inventory, [FromServices] IValidator<Inventory> validator)
        {
            ValidationResult validationResult = validator.Validate(inventory);
            if (!validationResult.IsValid)
            {
                var modelStateDictionary = new ModelStateDictionary();

                foreach (ValidationFailure validationFailure in validationResult.Errors)
                {
                    modelStateDictionary.AddModelError(
                        validationFailure.PropertyName,
                        validationFailure.ErrorMessage);
                }
                return ValidationProblem(modelStateDictionary);
            }

            GenerateId(inventory);
            _inventory.addProduct(inventory);
            return Ok(await GetAllProduct());
        }
        
        [HttpPost("PlaceOrder/{ProductId}")]
        public async Task<IActionResult> PlaceOrder(Guid ProductId)
        {
            var product = await _inventory.GetProduct(ProductId);
            if (product != null)
            {
                var message = new
                {
                    product.quantity
                };
                await _producer.ProduceMessage(product.Id, message);
                
                return Ok("Order Placed");
            }
            return BadRequest("Order cannot be placed for this product");
        }

        private void GenerateId(Inventory inventory)
        {
            inventory.Id = new Guid();
        }
    }
}
