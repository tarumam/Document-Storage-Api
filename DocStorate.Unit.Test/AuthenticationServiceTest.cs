using DocStorageApi.Data.Repository.Interfaces;
using DocStorageApi.DTO.Request;
using DocStorageApi.DTO.Response;
using DocStorageApi.Services;
using Moq;

namespace DocStorate.Unit.Test
{
    public class AuthenticationServiceTest
    {
        private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();

        [Theory]
        [InlineData("john.doe", "password")]
        [InlineData("jane.doe", "password123")]
        public async Task SignInAsync_ReturnsUserCredentialsResult(string username, string password)
        {
            // Arrange
            var authService = new AuthenticationService(_userRepository);
            var authRequest = new AuthRequest(username, password);

            var userId = Guid.NewGuid();
            var tokenId = Guid.NewGuid().ToString();
            var role = "user";

            var userCredentials = new UserCredentialsResult
            {
                UserId = userId,
                TokenId = tokenId,
                Role = role
            };

            _userRepository.GetUserByCredentials(Arg.Any<UserCredentialsRequest>())
                .Returns(userCredentials);

            // Act
            var result = await authService.SignInAsync(authRequest);

            // Assert
            result.Should().NotBeNull();
            result.UserId.Should().Be(userId);
            result.TokenId.Should().Be(tokenId);
            result.Role.Should().Be(role);
        }
    }
