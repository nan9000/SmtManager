using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using SmtManager.Application.DTOs.Auth;
using SmtManager.Application.Helpers;
using SmtManager.Application.Interfaces;
using SmtManager.Application.Services;
using SmtManager.Core.Entities;

namespace SmtManager.Application.Tests.Services;

[TestFixture]
public class AuthServiceTests
{
    private Mock<IUserRepository> _mockUserRepository = null!;
    private Mock<IConfiguration> _mockConfiguration = null!;
    private AuthService _authService = null!;

    [SetUp]
    public void Setup()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockConfiguration = new Mock<IConfiguration>();

        var jwtSettingsSection = new Mock<IConfigurationSection>();
        jwtSettingsSection.Setup(x => x["Key"]).Returns("ThisIsASecretKeyForJwtTokenGenerationWithAtLeast32Characters");
        jwtSettingsSection.Setup(x => x["Issuer"]).Returns("SmtManagerAPI");
        jwtSettingsSection.Setup(x => x["Audience"]).Returns("SmtManagerClient");

        _mockConfiguration.Setup(x => x.GetSection("JwtSettings")).Returns(jwtSettingsSection.Object);

        _authService = new AuthService(_mockUserRepository.Object, _mockConfiguration.Object);
    }

    [Test]
    public async Task LoginAsync_WithValidCredentials_ShouldReturnAuthResponse()
    {
        var loginDto = new LoginDto
        {
            Username = "testuser",
            Password = "password123"
        };

        var user = new User
        {
            Id = 1,
            Username = "testuser",
            Email = "test@example.com",
            PasswordHash = PasswordHasher.HashPassword("password123"),
            Role = "User"
        };

        _mockUserRepository.Setup(x => x.GetByUsernameAsync(loginDto.Username)).ReturnsAsync(user);

        var result = await _authService.LoginAsync(loginDto);

        result.Should().NotBeNull();
        result!.Username.Should().Be(user.Username);
        result.Email.Should().Be(user.Email);
        result.Role.Should().Be(user.Role);
        result.Token.Should().NotBeNullOrEmpty();
    }

    [Test]
    public async Task LoginAsync_WithInvalidUsername_ShouldReturnNull()
    {
        var loginDto = new LoginDto { Username = "nonexistent", Password = "password123" };
        _mockUserRepository.Setup(x => x.GetByUsernameAsync(loginDto.Username)).ReturnsAsync((User?)null);

        var result = await _authService.LoginAsync(loginDto);

        result.Should().BeNull();
    }

    [Test]
    public async Task LoginAsync_WithInvalidPassword_ShouldReturnNull()
    {
        var loginDto = new LoginDto { Username = "testuser", Password = "wrongpassword" };
        var user = new User
        {
            Id = 1,
            Username = "testuser",
            Email = "test@example.com",
            PasswordHash = PasswordHasher.HashPassword("password123"),
            Role = "User"
        };

        _mockUserRepository.Setup(x => x.GetByUsernameAsync(loginDto.Username)).ReturnsAsync(user);

        var result = await _authService.LoginAsync(loginDto);

        result.Should().BeNull();
    }

    [Test]
    public async Task RegisterAsync_WithValidData_ShouldReturnAuthResponse()
    {
        var registerDto = new RegisterDto
        {
            Username = "newuser",
            Email = "newuser@example.com",
            Password = "password123",
            Role = "User"
        };

        _mockUserRepository.Setup(x => x.UsernameExistsAsync(registerDto.Username)).ReturnsAsync(false);
        _mockUserRepository.Setup(x => x.EmailExistsAsync(registerDto.Email)).ReturnsAsync(false);
        _mockUserRepository.Setup(x => x.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

        var result = await _authService.RegisterAsync(registerDto);

        result.Should().NotBeNull();
        result!.Username.Should().Be(registerDto.Username);
        result.Email.Should().Be(registerDto.Email);
        result.Token.Should().NotBeNullOrEmpty();
    }

    [Test]
    public async Task RegisterAsync_WithExistingUsername_ShouldReturnNull()
    {
        var registerDto = new RegisterDto
        {
            Username = "existinguser",
            Email = "new@example.com",
            Password = "password123",
            Role = "User"
        };

        _mockUserRepository.Setup(x => x.UsernameExistsAsync(registerDto.Username)).ReturnsAsync(true);

        var result = await _authService.RegisterAsync(registerDto);

        result.Should().BeNull();
    }

    [Test]
    public async Task RegisterAsync_WithExistingEmail_ShouldReturnNull()
    {
        var registerDto = new RegisterDto
        {
            Username = "newuser",
            Email = "existing@example.com",
            Password = "password123",
            Role = "User"
        };

        _mockUserRepository.Setup(x => x.UsernameExistsAsync(registerDto.Username)).ReturnsAsync(false);
        _mockUserRepository.Setup(x => x.EmailExistsAsync(registerDto.Email)).ReturnsAsync(true);

        var result = await _authService.RegisterAsync(registerDto);

        result.Should().BeNull();
    }
}
