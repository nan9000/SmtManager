using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SmtManager.Core.Entities;
using SmtManager.Infrastructure.Data;

namespace SmtManager.Infrastructure.Tests.Data;

[TestFixture]
public class UserRepositoryTests
{
    private SmtDbContext _context = null!;
    private UserRepository _repository = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<SmtDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new SmtDbContext(options);
        _repository = new UserRepository(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task GetByUsernameAsync_WithExistingUsername_ShouldReturnUser()
    {
        var user = new User
        {
            Username = "testuser",
            Email = "test@example.com",
            PasswordHash = "hash",
            Role = "User"
        };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var result = await _repository.GetByUsernameAsync("testuser");

        result.Should().NotBeNull();
        result!.Username.Should().Be("testuser");
    }

    [Test]
    public async Task GetByEmailAsync_WithExistingEmail_ShouldReturnUser()
    {
        var user = new User
        {
            Username = "testuser",
            Email = "test@example.com",
            PasswordHash = "hash",
            Role = "User"
        };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var result = await _repository.GetByEmailAsync("test@example.com");

        result.Should().NotBeNull();
        result!.Email.Should().Be("test@example.com");
    }

    [Test]
    public async Task UsernameExistsAsync_WithExistingUsername_ShouldReturnTrue()
    {
        var user = new User
        {
            Username = "testuser",
            Email = "test@example.com",
            PasswordHash = "hash",
            Role = "User"
        };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var result = await _repository.UsernameExistsAsync("testuser");

        result.Should().BeTrue();
    }

    [Test]
    public async Task UsernameExistsAsync_WithNonExistingUsername_ShouldReturnFalse()
    {
        var result = await _repository.UsernameExistsAsync("nonexistent");

        result.Should().BeFalse();
    }

    [Test]
    public async Task EmailExistsAsync_WithExistingEmail_ShouldReturnTrue()
    {
        var user = new User
        {
            Username = "testuser",
            Email = "test@example.com",
            PasswordHash = "hash",
            Role = "User"
        };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var result = await _repository.EmailExistsAsync("test@example.com");

        result.Should().BeTrue();
    }

    [Test]
    public async Task EmailExistsAsync_WithNonExistingEmail_ShouldReturnFalse()
    {
        var result = await _repository.EmailExistsAsync("nonexistent@example.com");

        result.Should().BeFalse();
    }

    [Test]
    public async Task AddAsync_WithValidUser_ShouldAddToDatabase()
    {
        var user = new User
        {
            Username = "newuser",
            Email = "new@example.com",
            PasswordHash = "hash",
            Role = "User"
        };

        await _repository.AddAsync(user);

        var savedUser = await _context.Users.FindAsync(user.Id);
        savedUser.Should().NotBeNull();
        savedUser!.Username.Should().Be("newuser");
    }
}
