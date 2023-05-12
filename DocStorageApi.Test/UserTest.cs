using DocStorageApi.DTO.Request;
using DocStorageApi.DTO.Response;
using DocStorageApi.Integration.Tests.Config;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;

namespace DocStorageApi.Integration.Tests
{
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class UserTests
    {
        private readonly IntegrationTestsFixture<Program> _fixture;

        public UserTests(IntegrationTestsFixture<Program> testsFixture)
        {
            _fixture = testsFixture;
        }

        [Fact]
        public async Task GetUserById_Returns200_When_User_Exists()
        {
            // Arrange
            _fixture.Client.SetTokenToRequestHeader(_fixture.AdminToken);
            var userId = _fixture.ValidUsers.FirstOrDefault().UserId;

            // Act
            var response = await _fixture.Client.GetAsync($"{Constants.UserRoute}/getUserById/{userId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<UserResponse>(content);
            Assert.NotNull(user);
            Assert.Equal(userId, user.UserId);
        }

        [Fact]
        public async Task GetUserById_Returns204_When_User_Does_Not_Exist()
        {
            // Arrange
            var userId = new Guid("00000000-0000-0000-0000-000000000000");

            // Act
            var response = await _fixture.Client.GetAsync($"{Constants.UserRoute}/{userId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreateUser_Returns201_When_User_Is_Created()
        {
            // Arrange
            _fixture.Client.SetTokenToRequestHeader(_fixture.AdminToken);

            var userRequest = new CreateUserRequest
            {
                Username = "john@john.com",
                Password = "Doe",
                Role = "Regular"
            };

            // Act
            var response = await _fixture.Client.PostAsJsonAsync($"{Constants.UserRoute}/CreateUser", userRequest);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var location = response.Headers.Location;
            Assert.NotNull(location);
        }

        [Fact]
        public async Task CreateUser_Returns400_When_User_Request_Is_Invalid()
        {
            // Arrange
            _fixture.Client.SetTokenToRequestHeader(_fixture.AdminToken);

            var userRequest = new CreateUserRequest
            {
                Username = "",
                Password = "",
                Role = ""
            };

            // Act
            var response = await _fixture.Client.PostAsJsonAsync($"{Constants.UserRoute}/CreateUser", userRequest);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdateUser_Returns204_WhenUserIsUpdated()
        {
            // Arrange
            _fixture.Client.SetTokenToRequestHeader(_fixture.AdminToken);

            var selectUser = await _fixture.ListUsers();
            var firstUser = selectUser.FirstOrDefault();

            var userRequest = new UpdateUserRequest
            {
                Id = firstUser.UserId,
                Username = "NewUsername@anymail.com",
                Password = "Gonna be encrypted",
                Role = "Invalid",
                Status = false
            };

            // Act
            var response = await _fixture.Client.PutAsJsonAsync($"{Constants.UserRoute}/UpdateUser", userRequest);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            var selectUpdatedUser = await _fixture.ListUsers();
            var updatedUser = selectUpdatedUser.Where(u => u.UserId == firstUser.UserId).FirstOrDefault();

            Assert.NotNull(updatedUser);
            Assert.Equal("NewUsername@anymail.com", updatedUser.Name);
            Assert.NotEqual("Gonna be encrypted", updatedUser.Password);
            Assert.Equal("Invalid", updatedUser.Role);
            Assert.Equal(firstUser.CreatedAt, updatedUser.CreatedAt);
            Assert.NotEqual(firstUser.UpdatedAt, updatedUser.UpdatedAt);
            Assert.False(updatedUser.IsActive);
        }

        [Fact]
        public async Task UpdateUser_Returns400_When_User_Request_Is_Invalid()
        {
            var firstUser = _fixture.ValidUsers.FirstOrDefault();

            // Arrange
            var userRequest = new UpdateUserRequest
            {
                Id = firstUser.UserId,
                Username = "",
                Password = "Gonna be encrypted",
                Role = "Invalid",
                Status = false
            };

            // Act
            var response = await _fixture.Client.PutAsJsonAsync($"{Constants.UserRoute}/UpdateUser", userRequest);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
