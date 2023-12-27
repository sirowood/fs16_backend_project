using AutoMapper;
using Moq;
using Shopify.Core.src.Abstraction;
using Shopify.Core.src.Entity;
using Shopify.Core.src.Shared;
using Shopify.Service.src.DTO;
using Shopify.Service.src.Service;
using Shopify.Service.src.Shared;
using Shopify.Test.Shared;

namespace Shopify.Test.src;

public class UserServiceTest
{
  private readonly IMapper _mapper = MapperHelper.GetMapper();
  [Fact]

  public async Task CreateOneAsync_ValidUser_ReturnsUserReadDTO()
  {
    // Arrange
    var createDto = new UserCreateDTO
    {
      Email = "newuser@example.com",
      Password = "password",
      FirstName = "firstName",
      LastName = "lastName",
      Avatar = "avatarURL",
    };

    var user = _mapper.Map<UserCreateDTO, User>(createDto);

    var repoMock = new Mock<IUserRepo>();
    repoMock
      .Setup(repo => repo.GetByEmailAsync(createDto.Email))
      .ReturnsAsync((User?)null);
    repoMock
      .Setup(repo => repo.CreateOneAsync(It.IsAny<User>()))
      .ReturnsAsync(user);

    var userService = new UserService(repoMock.Object, _mapper);

    // Act
    var result = await userService.CreateOneAsync(createDto);

    // Assert
    Assert.NotNull(result);
    repoMock.Verify(repo => repo.GetByEmailAsync(createDto.Email), Times.Once);
    repoMock.Verify(repo => repo.CreateOneAsync(It.IsAny<User>()), Times.Once);
  }

  [Fact]
  public async Task CreateOneAsync_DuplicateEmail_ThrowsCustomException()
  {
    // Arrange
    var createDto = new UserCreateDTO
    {
      Email = "newuser@example.com",
      Password = "password",
      FirstName = "firstName",
      LastName = "lastName",
      Avatar = "avatarURL",
    };

    var user = _mapper.Map<UserCreateDTO, User>(createDto);

    var repoMock = new Mock<IUserRepo>();
    repoMock.Setup(repo => repo.GetByEmailAsync(createDto.Email))
        .ReturnsAsync(user);

    var userService = new UserService(repoMock.Object, _mapper);

    // Act & Assert
    await Assert.ThrowsAsync<CustomException>(async () => await userService.CreateOneAsync(createDto));

    // Assert
    repoMock.Verify(repo => repo.GetByEmailAsync(createDto.Email), Times.Once);
    repoMock.Verify(repo => repo.CreateOneAsync(It.IsAny<User>()), Times.Never);
  }

  [Fact]
  public async Task GetByEmailAsync_ExistingEmail_ReturnsUserReadDTO()
  {
    // Arrange
    var userEmail = "user@example.com";

    var user = new User
    {
      Id = Guid.NewGuid(),
      Email = userEmail,
      Password = "password",
      FirstName = "firstName",
      LastName = "lastName",
      Avatar = "avatarURL",
      Salt = "salt"
    };

    var repoMock = new Mock<IUserRepo>();
    repoMock
      .Setup(repo => repo.GetByEmailAsync(userEmail))
      .ReturnsAsync(user);

    var userService = new UserService(repoMock.Object, _mapper);

    // Act
    var result = await userService.GetByEmailAsync(userEmail);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(userEmail, result.Email);
    repoMock.Verify(repo => repo.GetByEmailAsync(userEmail), Times.Once);
  }

  [Fact]
  public async Task GetByEmailAsync_NonexistentEmail_ThrowsCustomException()
  {
    // Arrange
    var userEmail = "nonexistent@example.com";

    var repoMock = new Mock<IUserRepo>();
    repoMock
      .Setup(repo => repo.GetByEmailAsync(userEmail))
      .ReturnsAsync((User?)null);

    var userService = new UserService(repoMock.Object, _mapper);

    // Act & Assert
    await Assert.ThrowsAsync<CustomException>(async () => await userService.GetByEmailAsync(userEmail));

    // Assert
    repoMock.Verify(repo => repo.GetByEmailAsync(userEmail), Times.Once);
  }

  [Fact]
  public async Task UpdatePasswordAsync_CorrectPassword_ReturnsTrue()
  {
    // Arrange
    var userId = Guid.NewGuid();
    var originalPassword = "oldPassword";
    PasswordService.HashPassword(originalPassword, out string salt, out string password);
    var newPassword = "newPassword";
    var passwords = new ChangePasswords
    {
      OriginalPassword = originalPassword,
      NewPassword = newPassword
    };

    var user = new User
    {
      Id = userId,
      Email = "example@mail.com",
      Password = password,
      FirstName = "firstName",
      LastName = "lastName",
      Avatar = "avatarURL",
      Salt = salt
    };

    var repoMock = new Mock<IUserRepo>();
    repoMock.Setup(repo => repo.GetByIdAsync(userId))
        .ReturnsAsync(user);
    repoMock.Setup(repo => repo.UpdateOneAsync(It.IsAny<User>()))
        .ReturnsAsync(user);

    var userService = new UserService(repoMock.Object, _mapper);

    // Act
    var result = await userService.UpdatePasswordAsync(userId, passwords);

    // Assert
    Assert.True(result);
    repoMock.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
    repoMock.Verify(repo => repo.UpdateOneAsync(It.IsAny<User>()), Times.Once);
  }

  [Fact]
  public async Task UpdatePasswordAsync_IncorrectPassword_ThrowsCustomException()
  {
    // Arrange
    var userId = Guid.NewGuid();
    var originalPassword = "oldPassword";
    PasswordService.HashPassword(originalPassword, out string salt, out string password);
    var newPassword = "newPassword";
    var passwords = new ChangePasswords
    {
      OriginalPassword = "wrongPassword",
      NewPassword = newPassword
    };

    var user = new User
    {
      Id = userId,
      Email = "example@mail.com",
      Password = password,
      FirstName = "firstName",
      LastName = "lastName",
      Avatar = "avatarURL",
      Salt = salt
    };

    var repoMock = new Mock<IUserRepo>();
    repoMock
      .Setup(repo => repo.GetByIdAsync(userId))
      .ReturnsAsync(user);

    var userService = new UserService(repoMock.Object, _mapper);

    // Act & Assert
    await Assert.ThrowsAsync<CustomException>(async () => await userService.UpdatePasswordAsync(userId, passwords));

    // Assert
    repoMock.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
    repoMock.Verify(repo => repo.UpdateOneAsync(It.IsAny<User>()), Times.Never);
  }

  [Fact]
  public async Task UpdateOneAsync_ValidUser_ReturnsUserReadDTO()
  {
    // Arrange
    PasswordService.HashPassword("oldPassword", out string salt, out string password);
    var userId = Guid.NewGuid();
    var updateDto = new UserUpdateDTO
    {
      Email = "newemail@example.com",
      FirstName = "firstName",
      LastName = "lastName",
      Avatar = "avatarURL",
    };

    var user = new User
    {
      Id = userId,
      Email = "example@mail.com",
      Password = password,
      FirstName = "firstName",
      LastName = "lastName",
      Avatar = "avatarURL",
      Salt = salt
    };

    var repoMock = new Mock<IUserRepo>();
    repoMock.Setup(repo => repo.GetByIdAsync(userId))
        .ReturnsAsync(user); // Simulate existing user
    repoMock.Setup(repo => repo.GetByEmailAsync(updateDto.Email))
        .ReturnsAsync((User?)null);
    repoMock.Setup(repo => repo.UpdateOneAsync(It.IsAny<User>()))
        .ReturnsAsync(user); // Mock the repository update method

    var userService = new UserService(repoMock.Object, _mapper);

    // Act
    var result = await userService.UpdateOneAsync(userId, updateDto);

    // Assert
    Assert.NotNull(result);
    repoMock.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
    repoMock.Verify(repo => repo.GetByEmailAsync(updateDto.Email), Times.Once);
    repoMock.Verify(repo => repo.UpdateOneAsync(It.IsAny<User>()), Times.Once);
  }

  [Fact]
  public async Task UpdateOneAsync_DuplicateEmail_ThrowsCustomException()
  {
    // Arrange
    PasswordService.HashPassword("oldPassword", out string salt, out string password);
    var userId = Guid.NewGuid();
    var updateDto = new UserUpdateDTO
    {
      Email = "existing@example.com",
      FirstName = "firstName",
      LastName = "lastName",
      Avatar = "avatarURL",
    };

    var user = new User
    {
      Id = userId,
      Email = "example@mail.com",
      Password = password,
      FirstName = "firstName",
      LastName = "lastName",
      Avatar = "avatarURL",
      Salt = salt
    };

    var repoMock = new Mock<IUserRepo>();
    repoMock.Setup(repo => repo.GetByIdAsync(userId))
        .ReturnsAsync(user);
    repoMock.Setup(repo => repo.GetByEmailAsync(updateDto.Email))
        .ReturnsAsync(user);

    var userService = new UserService(repoMock.Object, _mapper);

    // Act & Assert
    await Assert.ThrowsAsync<CustomException>(async () => await userService.UpdateOneAsync(userId, updateDto));

    // Assert
    repoMock.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
    repoMock.Verify(repo => repo.GetByEmailAsync(updateDto.Email), Times.Once);
    repoMock.Verify(repo => repo.UpdateOneAsync(It.IsAny<User>()), Times.Never);
  }
}