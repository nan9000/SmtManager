using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SmtManager.Core.Entities;
using SmtManager.Infrastructure.Data;

namespace SmtManager.Infrastructure.Tests.Data;

[TestFixture]
public class OrderRepositoryTests
{
    private SmtDbContext _context = null!;
    private OrderRepository _repository = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<SmtDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new SmtDbContext(options);
        _repository = new OrderRepository(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task GetOrderWithDetailsAsync_WithValidId_ShouldReturnOrderWithRelatedData()
    {
        var component = new Component { Name = "Resistor", Description = "100 ohm resistor", Quantity = 100 };
        var board = new Board { Name = "PCB-001", Description = "Main board" };
        var order = new Order { Name = "ORD001", OrderNumber = "ORD001", Description = "Test order", OrderDate = DateTime.UtcNow };

        await _context.Components.AddAsync(component);
        await _context.Boards.AddAsync(board);
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();

        var boardComponent = new BoardComponent 
        { 
            BoardId = board.Id, 
            Board = board,
            ComponentId = component.Id, 
            Component = component,
            PlacementCount = 10 
        };
        var orderBoard = new OrderBoard 
        { 
            OrderId = order.Id, 
            Order = order,
            BoardId = board.Id, 
            Board = board,
            QuantityRequired = 5 
        };

        await _context.BoardComponents.AddAsync(boardComponent);
        await _context.OrderBoards.AddAsync(orderBoard);
        await _context.SaveChangesAsync();

        var result = await _repository.GetOrderWithDetailsAsync(order.Id);

        result.Should().NotBeNull();
        result!.OrderNumber.Should().Be("ORD001");
        result.OrderBoards.Should().HaveCount(1);
        result.OrderBoards.First().Board.BoardComponents.Should().HaveCount(1);
    }

    [Test]
    public async Task GetOrderWithDetailsAsync_WithInvalidId_ShouldReturnNull()
    {
        var result = await _repository.GetOrderWithDetailsAsync(999);

        result.Should().BeNull();
    }

    [Test]
    public async Task GetAllAsync_ShouldReturnAllOrders()
    {
        var order1 = new Order { Name = "ORD001", OrderNumber = "ORD001", Description = "First", OrderDate = DateTime.UtcNow };
        var order2 = new Order { Name = "ORD002", OrderNumber = "ORD002", Description = "Second", OrderDate = DateTime.UtcNow };

        await _context.Orders.AddRangeAsync(order1, order2);
        await _context.SaveChangesAsync();

        var result = await _repository.GetAllAsync();

        result.Should().HaveCount(2);
    }

    [Test]
    public async Task AddAsync_WithValidOrder_ShouldAddToDatabase()
    {
        var order = new Order { Name = "ORD001", OrderNumber = "ORD001", Description = "Test", OrderDate = DateTime.UtcNow };

        await _repository.AddAsync(order);

        var savedOrder = await _context.Orders.FindAsync(order.Id);
        savedOrder.Should().NotBeNull();
        savedOrder!.OrderNumber.Should().Be("ORD001");
    }
}
