using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SmtManager.Core.Entities;
using SmtManager.Infrastructure.Data;

namespace SmtManager.Infrastructure.Tests.Data;

[TestFixture]
public class GenericRepositoryTests
{
    private SmtDbContext _context = null!;
    private TestOrderRepository _repository = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<SmtDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new SmtDbContext(options);
        _repository = new TestOrderRepository(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task GetAllAsync_ShouldReturnAllEntities()
    {
        var order1 = new Order { OrderNumber = "ORD001", Description = "First order", OrderDate = DateTime.UtcNow };
        var order2 = new Order { OrderNumber = "ORD002", Description = "Second order", OrderDate = DateTime.UtcNow };
        
        await _context.Orders.AddRangeAsync(order1, order2);
        await _context.SaveChangesAsync();

        var result = await _repository.GetAllAsync();

        result.Should().HaveCount(2);
    }

    [Test]
    public async Task GetByIdAsync_WithValidId_ShouldReturnEntity()
    {
        var order = new Order { OrderNumber = "ORD001", Description = "Test order", OrderDate = DateTime.UtcNow };
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();

        var result = await _repository.GetByIdAsync(order.Id);

        result.Should().NotBeNull();
        result!.OrderNumber.Should().Be("ORD001");
    }

    [Test]
    public void GetByIdAsync_WithInvalidId_ShouldThrowException()
    {
        Assert.ThrowsAsync<ArgumentException>(async () => await _repository.GetByIdAsync(0));
    }

    [Test]
    public async Task AddAsync_WithValidEntity_ShouldAddToDatabase()
    {
        var order = new Order { OrderNumber = "ORD001", Description = "Test order", OrderDate = DateTime.UtcNow };

        await _repository.AddAsync(order);

        var savedOrder = await _context.Orders.FindAsync(order.Id);
        savedOrder.Should().NotBeNull();
        savedOrder!.OrderNumber.Should().Be("ORD001");
    }

    [Test]
    public async Task UpdateAsync_WithValidEntity_ShouldUpdateInDatabase()
    {
        var order = new Order { OrderNumber = "ORD001", Description = "Original", OrderDate = DateTime.UtcNow };
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();

        _context.Entry(order).State = EntityState.Detached;
        order.Description = "Updated";

        await _repository.UpdateAsync(order);

        var updatedOrder = await _context.Orders.FindAsync(order.Id);
        updatedOrder!.Description.Should().Be("Updated");
    }

    [Test]
    public async Task DeleteAsync_WithValidId_ShouldRemoveFromDatabase()
    {
        var order = new Order { OrderNumber = "ORD001", Description = "Test order", OrderDate = DateTime.UtcNow };
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
        var orderId = order.Id;

        await _repository.DeleteAsync(orderId);

        var deletedOrder = await _context.Orders.FindAsync(orderId);
        deletedOrder.Should().BeNull();
    }

    private class TestOrderRepository : GenericRepository<Order>
    {
        public TestOrderRepository(SmtDbContext context) : base(context) { }
    }
}
