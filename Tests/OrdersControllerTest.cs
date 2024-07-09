using FCOrderManagement.Controllers;
using FCOrderManagement.Models;
using FCOrderManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace FCOrderManagement.Tests
{
    public class OrdersControllerTest
    {
        private readonly Mock<IApiService> _mockApiService;
        private readonly Mock<OrderCalculationService> _mockCalculationService;
        private readonly OrdersController _controller;

        public OrdersControllerTest()
        {
            _mockApiService = new Mock<IApiService>();
            _mockCalculationService = new Mock<OrderCalculationService>();
            _controller = new OrdersController(_mockApiService.Object, _mockCalculationService.Object);
        }

        [Fact]
        public async Task PlaceOrder_ValidOrder_ReturnsOkResult()
        {
            // Arrange
            var order = new Order(); // Create a valid order object
            _mockApiService.Setup(service => service.GetProducts()).ReturnsAsync(new List<Product>());
            _mockApiService.Setup(service => service.PlaceOrder(It.IsAny<Order>())).ReturnsAsync(true);

            // Act
            var result = await _controller.PlaceOrder(order);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

    }
}