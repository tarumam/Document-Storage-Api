using DocStorageApi.DTO.Request;
using DocStorageApi.Integration.Tests.Config;
using System.Net;
using System.Net.Http.Json;

namespace DocStorageApi.Integration.Tests
{
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class AuthenticationTests
    {
        private readonly IntegrationTestsFixture<Program> _testsFixture;

        public AuthenticationTests(IntegrationTestsFixture<Program> testsFixture)
        {
            _testsFixture = testsFixture;
        }

        public async Task SignUp_Returns_UserId_When_Valid_Info_Is_Provided()
        {
            // Arrange
            var authModel = new AuthRequest("testUser@testuser.com", "testPassword");

            // Act
            var response = await _testsFixture.Client.PostAsJsonAsync($"{Constants.AuthenticationRoute}/SignUp", authModel);

            // Assert
            response.EnsureSuccessStatusCode();
            var userId = await response.Content.ReadFromJsonAsync<string>();
            Assert.NotNull(userId);
        }

        [Fact]
        public async Task SignIn_Returns_JwtToken_When_Valid_Info_Is_Provided()
        {
            // Arrange
            var authModel = new AuthRequest("admin@admin.com", "string");

            // Act
            var response = await _testsFixture.Client.PostAsJsonAsync($"{Constants.AuthenticationRoute}/SignIn", authModel);

            // Assert
            response.EnsureSuccessStatusCode();
            var jwtToken = await response.Content.ReadAsStringAsync();
            Assert.NotNull(jwtToken);
        }

        [Fact]
        public async Task SignUp_Returns_BadRequest_When_Invalid_Info_Is_Provided()
        {
            // Arrange
            var authModel = new AuthRequest(null, null);

            // Act
            var response = await _testsFixture.Client.PostAsJsonAsync($"{Constants.AuthenticationRoute}/SignUp", authModel);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task SignIn_Returns_Unauthorized_When_Invalid_Info_Is_Provided()
        {
            // Arrange
            var authModel = new AuthRequest("admin@admin.com", "wrongPassword");

            // Act
            var response = await _testsFixture.Client.PostAsJsonAsync($"{Constants.AuthenticationRoute}/SignIn", authModel);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
