using FluentAssertions;
using NUnit.Framework;
using SmtManager.Application.Helpers;

namespace SmtManager.Application.Tests.Helpers;

[TestFixture]
public class PasswordHasherTests
{
    [Test]
    public void HashPassword_ShouldReturnNonEmptyString()
    {
        var password = "TestPassword123";
        var hash = PasswordHasher.HashPassword(password);
        hash.Should().NotBeNullOrEmpty();
    }

    [Test]
    public void HashPassword_SamePassword_ShouldReturnSameHash()
    {
        var password = "TestPassword123";
        var hash1 = PasswordHasher.HashPassword(password);
        var hash2 = PasswordHasher.HashPassword(password);
        hash1.Should().Be(hash2);
    }

    [Test]
    public void HashPassword_DifferentPasswords_ShouldReturnDifferentHashes()
    {
        var password1 = "TestPassword123";
        var password2 = "DifferentPassword456";
        var hash1 = PasswordHasher.HashPassword(password1);
        var hash2 = PasswordHasher.HashPassword(password2);
        hash1.Should().NotBe(hash2);
    }

    [Test]
    public void VerifyPassword_WithCorrectPassword_ShouldReturnTrue()
    {
        var password = "TestPassword123";
        var hash = PasswordHasher.HashPassword(password);
        var result = PasswordHasher.VerifyPassword(password, hash);
        result.Should().BeTrue();
    }

    [Test]
    public void VerifyPassword_WithIncorrectPassword_ShouldReturnFalse()
    {
        var password = "TestPassword123";
        var wrongPassword = "WrongPassword456";
        var hash = PasswordHasher.HashPassword(password);
        var result = PasswordHasher.VerifyPassword(wrongPassword, hash);
        result.Should().BeFalse();
    }
}
