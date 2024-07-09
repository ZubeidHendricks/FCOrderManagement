using FCOrderManagement.Models;
using FCOrderManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class OrdersControllerTests
{
    private readonly Mock<IApiService> _mockApiService;
    private readonly OrdersController _controller;

    public OrdersControllerTests()
    {
        _mockApiService = new Mock<IApiService>();
        _controller = new OrdersController(_mockApiService.Object);
    }

    [Fact]
    public async Task PlaceOrder_ValidOrderWithMultipleItems_ReturnsOkResult()
    {
        // Arrange
        var products = new List<Product>
        {
            new Product { ProductID = 1, Price = 100 },
            new Product { ProductID = 2, Price = 200 }
        };

        _mockApiService.Setup(service => service.GetProducts())
            .ReturnsAsync(products);

        _mockApiService.Setup(service => service.PlaceOrder(It.IsAny<Order>()))
            .ReturnsAsync(true);

        var order = new Order
        {
            UserID = "1",
            CustomerName = "Test Customer",
            OrderDate = DateTime.Now,
            OrderDetails = new List<OrderDetail>
            {
                new OrderDetail { ProductID = 1, Quantity = 2 },
                new OrderDetail { ProductID = 2, Quantity = 1 }
            }
        };

        // Act
        var result = await _controller.PlaceOrder(order);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        dynamic returnValue = okResult.Value;
        Assert.Equal("Order placed successfully", returnValue.message);
    }

    [Fact]
    public async Task PlaceOrder_EmptyOrderDetails_ReturnsBadRequest()
    {
        // Arrange
        var order = new Order
        {
            UserID = "1",
            CustomerName = "Test Customer",
            OrderDate = DateTime.Now,
            OrderDetails = new List<OrderDetail>()
        };

        // Act
        var result = await _controller.PlaceOrder(order);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task PlaceOrder_ApiServiceFails_ReturnsBadRequest()
    {
        // Arrange
        _mockApiService.Setup(service => service.GetProducts())
            .ReturnsAsync(new List<Product>());

        _mockApiService.Setup(service => service.PlaceOrder(It.IsAny<Order>()))
            .ReturnsAsync(false);

        var order = new Order
        {
            UserID = "1",
            CustomerName = "Test Customer",
            OrderDate = DateTime.Now,
            OrderDetails = new List<OrderDetail>
            {
                new OrderDetail { ProductID = 1, Quantity = 1 }
            }
        };

        // Act
        var result = await _controller.PlaceOrder(order);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
}