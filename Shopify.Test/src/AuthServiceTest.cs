using AutoMapper;
using Moq;

using Shopify.Core.src.Abstraction;
using Shopify.Core.src.Entity;
using Shopify.Core.src.Shared;
using Shopify.Service.src.Abstraction;
using Shopify.Service.src.DTO;
using Shopify.Service.src.Service;
using Shopify.Service.src.Shared;
using Shopify.Test.Shared;

namespace Shopify.Test.src;

public class AuthServiceTest
{
  private readonly IMapper _mapper = MapperHelper.GetMapper();

  [Fact]
  public async Task LoginAsync_ValidCredentials_ReturnsToken()
  {
    // Arrange
    var credentials = new Credentials { Email = "user@example.com", Password = "password" };
    PasswordService.HashPassword("password", out string salt, out string hashedPassword);

    var user = new User
    {
      Id = Guid.NewGuid(),
      FirstName = "firstName",
      LastName = "lastName",
      Email = credentials.Email,
      Avatar = "avatarURL",
      Password = hashedPassword,
      Salt = salt,
    };

    var repoMock = new Mock<IUserRepo>();
    repoMock
      .Setup(repo => repo.GetByEmailAsync(credentials.Email))
      .ReturnsAsync(user);

    var tokenServiceMock = new Mock<ITokenService>();
    tokenServiceMock.Setup(tokenService => tokenService.GenerateToken(user))
        .Returns("fakeToken");

    var authService = new AuthService(repoMock.Object, tokenServiceMock.Object, _mapper);

    // Act
    var result = await authService.LoginAsync(credentials);

    // Assert
    Assert.Equal("fakeToken", result);
  }

  [Fact]
  public async Task LoginAsync_InvalidCredentials_ThrowsCustomException()
  {
    // Arrange
    var credentials = new Credentials
    {
      Email = "nonexistent@example.com",
      Password = "invalidPassword"
    };

    var repoMock = new Mock<IUserRepo>();
    repoMock
      .Setup(repo => repo.GetByEmailAsync(credentials.Email))
      .ReturnsAsync((User?)null);

    var tokenServiceMock = new Mock<ITokenService>();

    var authService = new AuthService(repoMock.Object, tokenServiceMock.Object, _mapper);

    // Act & Assert
    await Assert.ThrowsAsync<CustomException>(async () => await authService.LoginAsync(credentials));

    // Assert
    repoMock.Verify(repo => repo.GetByEmailAsync(credentials.Email), Times.Once);
  }

  [Fact]
  public async Task RegisterAsync_ValidUser_ReturnsTrue()
  {
    // Arrange
    var createDto = new UserRegisterDTO
    {
      Email = "newuser@example.com",
      Password = "password",
      FirstName = "firstName",
      LastName = "lastName",
      Avatar = "avatarURL",
    };

    var user = _mapper.Map<UserRegisterDTO, User>(createDto);

    var repoMock = new Mock<IUserRepo>();
    repoMock.Setup(repo => repo.GetByEmailAsync(createDto.Email))
      .ReturnsAsync((User?)null);
    repoMock.Setup(repo => repo.CreateOneAsync(user))
      .ReturnsAsync(user);

    var tokenServiceMock = new Mock<ITokenService>();

    var authService = new AuthService(repoMock.Object, tokenServiceMock.Object, _mapper);

    // Act
    var result = await authService.RegisterAsync(createDto);

    // Assert
    Assert.True(result);
    repoMock.Verify(repo => repo.CreateOneAsync(It.IsAny<User>()), Times.Once);
  }

  [Fact]
  public async Task RegisterAsync_DuplicateEmail_ThrowsCustomException()
  {
    // Arrange
    var createDto = new UserRegisterDTO
    {
      Email = "newuser@example.com",
      Password = "password",
      FirstName = "firstName",
      LastName = "lastName",
      Avatar = "avatarURL",
    };

    var user = _mapper.Map<UserRegisterDTO, User>(createDto);

    var repoMock = new Mock<IUserRepo>();
    repoMock.Setup(repo => repo.GetByEmailAsync(createDto.Email))
      .ReturnsAsync(user);

    var tokenServiceMock = new Mock<ITokenService>();

    var authService = new AuthService(repoMock.Object, tokenServiceMock.Object, _mapper);

    // Act
    var exception = await Assert.ThrowsAsync<CustomException>(async () => await authService.RegisterAsync(createDto));

    // Assert
    Assert.Equal("Email is not available.", exception.Message);
    repoMock.Verify(repo => repo.GetByEmailAsync(createDto.Email), Times.Once);
    repoMock.Verify(repo => repo.CreateOneAsync(It.IsAny<User>()), Times.Never);
  }
}