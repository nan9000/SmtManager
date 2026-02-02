using FluentAssertions;
using Moq;
using NUnit.Framework;
using SmtManager.Application.DTOs;
using SmtManager.Application.Interfaces;
using SmtManager.Application.Services;
using SmtManager.Core.Entities;

namespace SmtManager.Application.Tests.Services;

[TestFixture]
public class OrderServiceTests
{
    private Mock<IOrderRepository> _mockRepository = null!;
    private OrderService _orderService = null!;

    [SetUp]
    public void Setup()
    {
        _mockRepository = new Mock<IOrderRepository>();
        _orderService = new OrderService(_mockRepository.Object);
    }

    [Test]
    public async Task GetAllOrdersAsync_ShouldReturnAllOrders()
    {
        var orders = new List<Order>
        {
            new Order { Id = 1, OrderNumber = "ORD001", Description = "First order", OrderDate = DateTime.UtcNow },
            new Order { Id = 2, OrderNumber = "ORD002", Description = "Second order", OrderDate = DateTime.UtcNow }
        };

        _mockRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(orders);

        var result = await _orderService.GetAllOrdersAsync();

        result.Should().HaveCount(2);
    }

    [Test]
    public async Task GetOrderByIdAsync_WithValidId_ShouldReturnOrder()
    {
        var order = new Order
        {
            Id = 1,
            OrderNumber = "ORD001",
            Description = "Test order",
            OrderDate = DateTime.UtcNow
        };

        _mockRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(order);

        var result = await _orderService.GetOrderByIdAsync(1);

        result.Should().NotBeNull();
        result!.OrderNumber.Should().Be("ORD001");
    }

    [Test]
    public async Task CreateOrderAsync_WithValidData_ShouldCreateOrder()
    {
        var orderDto = new CreateOrderDto
        {
            OrderNumber = "ORD001",
            Description = "Test order"
        };

        _mockRepository.Setup(x => x.AddAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);

        var result = await _orderService.CreateOrderAsync(orderDto);

        result.Should().NotBeNull();
        result.OrderNumber.Should().Be(orderDto.OrderNumber);
        result.Description.Should().Be(orderDto.Description);
    }

    [Test]
    public void CreateOrderAsync_WithEmptyOrderNumber_ShouldThrowException()
    {
        var orderDto = new CreateOrderDto { OrderNumber = "", Description = "Test" };

        Assert.ThrowsAsync<ArgumentException>(async () => await _orderService.CreateOrderAsync(orderDto));
    }

    [Test]
    public void CreateOrderAsync_WithTooLongOrderNumber_ShouldThrowException()
    {
        var orderDto = new CreateOrderDto { OrderNumber = new string('A', 51), Description = "Test" };

        Assert.ThrowsAsync<ArgumentException>(async () => await _orderService.CreateOrderAsync(orderDto));
    }

    [Test]
    public void CreateOrderAsync_WithEmptyDescription_ShouldThrowException()
    {
        var orderDto = new CreateOrderDto { OrderNumber = "ORD001", Description = "" };

        Assert.ThrowsAsync<ArgumentException>(async () => await _orderService.CreateOrderAsync(orderDto));
    }

    [Test]
    public void CreateOrderAsync_WithTooLongDescription_ShouldThrowException()
    {
        var orderDto = new CreateOrderDto { OrderNumber = "ORD001", Description = new string('A', 501) };

        Assert.ThrowsAsync<ArgumentException>(async () => await _orderService.CreateOrderAsync(orderDto));
    }

    [Test]
    public async Task GetOrderDownloadAsync_WithValidId_ShouldReturnFileContent()
    {
        var order = new Order
        {
            Id = 1,
            OrderNumber = "ORD001",
            Description = "Test order",
            OrderDate = DateTime.UtcNow
        };

        _mockRepository.Setup(x => x.GetOrderWithDetailsAsync(1)).ReturnsAsync(order);

        var result = await _orderService.GetOrderDownloadAsync(1);

        result.Should().NotBeNull();
        result!.Value.FileContent.Should().NotBeEmpty();
        result.Value.FileName.Should().Contain("order_ORD001_");
    }

    [Test]
    public async Task GetOrderDownloadAsync_WithInvalidId_ShouldReturnNull()
    {
        _mockRepository.Setup(x => x.GetOrderWithDetailsAsync(999)).ReturnsAsync((Order?)null);

        var result = await _orderService.GetOrderDownloadAsync(999);

        result.Should().BeNull();
    }
}
