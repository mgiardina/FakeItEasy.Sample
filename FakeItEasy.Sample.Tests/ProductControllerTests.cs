using FakeItEasy;
using FakeItEasy.Sample.Controllers;
using FakeItEasy.Sample.Models;
using FakeItEasy.Sample.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace FakeItEasy.Sample.Tests
{
    public class ProductControllerTests
    {
        private IProductService _fakeService;
        private ProductController _controller;

        [SetUp]
        public void SetUp()
        {
            _fakeService = A.Fake<IProductService>();
            _controller = new ProductController(_fakeService);
        }

        [Test]
        public void GetAllProducts_ReturnsAllProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Price = 10.99m },
                new Product { Id = 2, Name = "Product 2", Price = 19.99m },
                new Product { Id = 3, Name = "Product 3", Price = 5.99m }
            };
            A.CallTo(() => _fakeService.GetAllProducts()).Returns(products);

            // Act
            var result = _controller.GetAllProducts() as OkObjectResult;

            // Assert
            A.CallTo(() => _fakeService.GetAllProducts()).MustHaveHappenedOnceExactly();
            result.Value.Should().NotBeNull();
            result.Value.Should().BeEquivalentTo(products);
        }

        [Test]
        public void GetProductById_ReturnsProduct_WhenFound()
        {
            // Arrange
            var fakeService = A.Fake<IProductService>();
            var product = new Product { Id = 1, Name = "Product 1", Price = 10.99m };
            A.CallTo(() => fakeService.GetProductById(1)).Returns(product);
            var controller = new ProductController(fakeService);

            // Act
            var result = controller.GetProductById(1) as OkObjectResult;

            // Assert
            result.Value.Should().NotBeNull();
            result.Value.Should().Be(product);
        }

        [Test]
        public void GetProductById_ReturnsNotFound_WhenNotFound()
        {
            // Arrange
            A.CallTo(() => _fakeService.GetProductById(1)).Returns(null);

            // Act
            var result = _controller.GetProductById(1);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public void CreateProduct_ReturnsCreatedAtAction_WithCreatedProduct()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product 1", Price = 10.99m };

            // Act
            var result = _controller.CreateProduct(product);

            // Assert
            result.Should().BeOfType<CreatedAtActionResult>();
            var createdAtActionResult = (CreatedAtActionResult)result;
            createdAtActionResult.ActionName.Should().Be(nameof(ProductController.CreateProduct));
            createdAtActionResult.Value.Should().Be(product);
        }

        [Test]
        public void UpdateProduct_ReturnsNoContent_WhenIdMatches()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product 1", Price = 10.99m };

            // Act
            var result = _controller.UpdateProduct(1, product);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Test]
        public void UpdateProduct_ReturnsBadRequest_WhenIdDoesNotMatch()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product 1", Price = 10.99m };

            // Act
            var result = _controller.UpdateProduct(2, product);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public void DeleteProduct_ReturnsNoContent()
        {
            // Arrange & Act
            var result = _controller.DeleteProduct(1);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }
    }
}