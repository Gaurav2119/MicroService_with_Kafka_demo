using InventoryService.AppDbContext;
using InventoryService.DataAccess.Implementation;
using InventoryService.DataAccess.Interface;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NSubstitute.ReceivedExtensions;

namespace Inventory_xUnitTest
{
    public class InventoryTest
    {
        private IInventory GetInMemoryInventoryData(out productDbContext _productDbContext)
        {
            DbContextOptions<productDbContext> options;
            var builder = new DbContextOptionsBuilder<productDbContext>();
            builder.UseInMemoryDatabase("TestInventoryDataBase");
            options = builder.Options;
            _productDbContext = new productDbContext(options);
            _productDbContext.Database.EnsureDeleted();
            _productDbContext.Database.EnsureCreated();
            return new Inventory(_productDbContext);
        }

        [Fact]
        public async Task GetProduct_ReturnProduct_WhenIdIsProvided()
        {
            // Arrange
            productDbContext _productDbContext;
            IInventory _inventory = GetInMemoryInventoryData(out _productDbContext);

            var productId = Guid.NewGuid();

            InventoryService.Models.Inventory inventoryModel = new InventoryService.Models.Inventory
            {
                Id = productId,
                productName = "Salt",
                quantity = 5
            };

            _productDbContext.inventory.Add(inventoryModel);
            _productDbContext.SaveChanges();

            // Act
            var result = await _inventory.GetProduct(productId);

            // Assert
            Assert.Equal(productId.ToString(), result?.Id.ToString());
            Assert.Equal(inventoryModel.productName, result?.productName);

            // Dispose of the in-memory database
            _productDbContext.Dispose();
        }

        [Fact]
        public void AddProduct_ThrowsArgumentNullException_WhenProductIsNull()
        {
            // Arrange
            productDbContext _productDbContext;
            IInventory _inventory = GetInMemoryInventoryData(out _productDbContext);

            // Act
            var result = () => _inventory.addProduct(null);

            // Assert
            Assert.Throws<ArgumentNullException>(result);

            // Dispose of the in-memory database
            _productDbContext.Dispose();
        }

        [Fact]
        public void AddProduct_ShoudAddProduct()
        {
            // Arrange
            productDbContext _productDbContext;
            IInventory _inventory = GetInMemoryInventoryData(out _productDbContext);
            var product = new InventoryService.Models.Inventory
            {
                Id = Guid.NewGuid(),
                productName = "salt",
                quantity = 5
            };

            // Act
            _inventory.addProduct(product);

            // Assert
            Assert.Equal(1, _productDbContext.inventory.Count());

            // Dispose of the in-memory database
            _productDbContext.Dispose();
        }

        [Fact]
        public async Task GetAllProduct_ShouldReturnAllProducts()
        {
            // Arrange
            productDbContext _productDbContext;
            IInventory _inventory = GetInMemoryInventoryData(out _productDbContext);

            var expectedProduct = new List<InventoryService.Models.Inventory>
            {
                new InventoryService.Models.Inventory { Id = Guid.NewGuid(), productName = "product1", quantity = 5 },
                new InventoryService.Models.Inventory { Id = Guid.NewGuid(), productName = "product2", quantity = 10 }
            };

            _productDbContext.inventory.AddRange(expectedProduct);
            _productDbContext.SaveChanges();

            // Act
            var actualResult = await _inventory.GetAllProduct();

            // Assert
            Assert.Equal(expectedProduct.Count(), actualResult.Count());
            Assert.Equal("product1", actualResult.ToList()[0].productName);
        }
    }
}