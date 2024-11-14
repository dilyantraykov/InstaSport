using InstaSport.Data.Common;
using InstaSport.Data.Models;
using InstaSport.Services.Data;
using InstaSport.Services.Data.Constants;
using InstaSport.Services.Data.Exceptions;
using InstaSport.Services.Data.Localization;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace InstaSport.Tests;

[TestFixture]
public class AuthenticationServiceTests
{
    private Mock<IDbRepository<User>> mockUserRepository;
    private Mock<IPasswordHasher<User>> mockPasswordHasher;
    private AuthenticationService authService;

    [SetUp]
    public void SetUp()
    {
        mockUserRepository = new Mock<IDbRepository<User>>();
        mockPasswordHasher = new Mock<IPasswordHasher<User>>();
        authService = new AuthenticationService(mockUserRepository.Object, mockPasswordHasher.Object);
    }

    [Test]
    public void Register_ShouldThrowException_WhenPasswordsDoNotMatch()
    {
        // Arrange
        string password = "password";
        string confirmPassword = "differentPassword";

        // Act & Assert
        var exception = Assert.Throws<InvalidPropertyException>(() => authService.Register("username", "email@example.com", "FirstName", "LastName", password, confirmPassword));
        Assert.AreEqual("Passwords do not match!", exception.Message);
    }

    [Test]
    public void Register_ShouldThrowException_WhenUsernameAlreadyExists()
    {
        // Arrange
        var existingUserName = "existingUser";
        var existingUser = new User { UserName = existingUserName };
        mockUserRepository
            .Setup(repo => repo.All())
            .Returns(new[] { existingUser }
            .AsQueryable());

        // Act & Assert
        var exception = Assert.Throws<InvalidPropertyException>(() => 
        authService.Register(existingUserName, "email@example.com", "FirstName", 
        "LastName", "password", "password"));

        Assert.AreEqual(Strings.ExistingUserNameExceptionMessage, exception.Message);
    }

    [Test]
    public void Register_ShouldThrowException_WhenEmailAlreadyExists()
    {
        // Arrange
        var existingUser = new User { Email = "email@example.com" };
        mockUserRepository.Setup(repo => repo.All()).Returns(new[] { existingUser }.AsQueryable());

        // Act & Assert
        var exception = Assert.Throws<InvalidPropertyException>(() => authService.Register("username", "email@example.com", "FirstName", "LastName", "password", "password"));
        Assert.AreEqual("This email is already registered!", exception.Message);
    }

    [Test]
    public void Register_ShouldAddUser_WhenValidData()
    {
        // Arrange
        mockUserRepository.Setup(repo => repo.All()).Returns(Enumerable.Empty<User>().AsQueryable());
        mockPasswordHasher.Setup(hasher => hasher.HashPassword(It.IsAny<User>(), It.IsAny<string>())).Returns("hashedPassword");

        // Act
        var user = authService.Register("username", "email@example.com", "FirstName", "LastName", "password", "password");

        // Assert
        mockUserRepository.Verify(repo => repo.Add(It.IsAny<User>()), Times.Exactly(2)); // need to register admin user
        mockUserRepository.Verify(repo => repo.Save(), Times.Exactly(2));
        Assert.AreEqual("username", user.UserName);
        Assert.AreEqual("email@example.com", user.Email);
        Assert.AreEqual("hashedPassword", user.PasswordHash);
    }

    [Test]
    public void Login_ShouldThrowException_WhenUserNotFound()
    {
        // Arrange
        mockUserRepository.Setup(repo => repo.All()).Returns(Enumerable.Empty<User>().AsQueryable());

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => authService.Login("nonexistentUser", "password"));
        Assert.AreEqual("User not found!", exception.Message);
    }

    [Test]
    public void Login_ShouldThrowException_WhenPasswordIsInvalid()
    {
        // Arrange
        var user = new User { UserName = "username", PasswordHash = "hashedPassword" };
        mockUserRepository.Setup(repo => repo.All()).Returns(new[] { user }.AsQueryable());
        mockPasswordHasher.Setup(hasher => hasher.VerifyHashedPassword(user, "hashedPassword", "wrongPassword")).Returns(PasswordVerificationResult.Failed);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => authService.Login("username", "wrongPassword"));
        Assert.AreEqual("Password is invalid!", exception.Message);
    }

    [Test]
    public void Login_ShouldReturnUser_WhenCredentialsAreValid()
    {
        // Arrange
        var user = new User
        { 
            UserName = "username",
            PasswordHash = "hashedPassword"
        };

        mockUserRepository
            .Setup(repo => repo.All())
            .Returns(new[] { user }
            .AsQueryable());

        mockPasswordHasher
            .Setup(hasher => hasher.VerifyHashedPassword(user, 
            "hashedPassword", "password"))
            .Returns(PasswordVerificationResult.Success);

        // Act
        var result = authService.Login("username", "password");

        // Assert
        Assert.AreEqual(user, result);
    }
}