using DocStorageApi.Data.Commands;
using DocStorageApi.Data.Queries;
using DocStorageApi.DbObjects.Test.Config;
using DocStorageApi.Domain.Repository.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DocStorageApi.Command.Test
{
    public class UserCommandTests : IClassFixture<DocStorageAppFactory<Program>>
    {
        private readonly DocStorageAppFactory<Program> _factory;
        private readonly IUserRepository _userRepository;

        public UserCommandTests()
        {
            _factory = new DocStorageAppFactory<Program>();
            _userRepository = _factory.Services.GetService<IUserRepository>();
        }

        [Theory]
        [InlineData("User 1", "Encrypted Pass 1", "Role 1", true)]
        [InlineData("User 2", "Encrypted Pass 2", "Role 2", false)]
        [InlineData("User 3", "Encrypted Pass 3", "Role 3", true)]
        public async Task InsertUserAsync_Should_Insert_User_Into_Database(string username, string password, string role, bool isActive)
        {
            // Arrange
            var command = new InsertUserCommand(username, password, role, isActive);

            // Act
            var result = await _userRepository.InsertUserAsync(command);

            // Assert
            Assert.True(result.Succeeded);
            Assert.True(result.Data != null && result.Data != Guid.Empty);

            var inserted = await _userRepository.GetUserByIdAsync(new GetUserByIdQuery((Guid)result.Data));
            Assert.NotNull(inserted);
            Assert.Equal(username, inserted.Name);
            Assert.Equal(password, inserted.Password);
            Assert.Equal(role, inserted.Role);
            Assert.Equal(isActive, inserted.IsActive);
        }

        [Theory]
        [InlineData(null, null, "", true)]
        [InlineData("", "", "Role 2", false)]
        [InlineData("sm", "d", "", false)]
        [InlineData("user", "pw", "Role 1", true)]
        public async Task Insert_User_Async_Should_Fail_On_Command_Validation(string name, string password, string role, bool status)
        {
            // Arrange
            var command = new InsertUserCommand(name, password, role, status);
            // Act
            var result = await _userRepository.InsertUserAsync(command);

            // Assert
            Assert.False(result.Succeeded);
            Assert.True(result.Errors.Any());
        }

        [Theory]
        [InlineData("User 1", "PWD!", "Manager", true)]
        [InlineData("User 2", "PWD!!!", "Admin", false)]
        public async Task Insert_User_Async_Should_Fail_On_UniqueKey_Violation(string name, string password, string role, bool status)
        {
            // Arrange
            var command = new InsertUserCommand(name, password, role, status);

            // Ensure we have the same data on database
            await _userRepository.InsertUserAsync(command);

            // Act
            var result = await _userRepository.InsertUserAsync(command);

            // Assert
            Assert.False(result.Succeeded);
            Assert.True(result.Errors.Any());
            Assert.True(result.Errors.Count() == 1);
            Assert.True(result.Errors.First().MemberNames.First() == "P0001");
        }

        [Theory]
        [InlineData("User 1", "PWD!", "Manager", true)]
        [InlineData("User 2", "PWD!!!", "Admin", false)]
        public async Task Update_User_Async_Should_Update_User_on_Database(string name, string password, string role, bool status)
        {
            // Arrange
            var insertUser = new InsertUserCommand(name, password, role, status);
            var inserted = await _userRepository.InsertUserAsync(insertUser);

            Assert.NotNull(inserted.Data);

            var updateCmd = new UpdateUserCommand((Guid)inserted.Data, name, password, role, status);

            // Act
            var updated = await _userRepository.UpdateUserAsync(updateCmd);

            // Ensure we have the same data on database
            var result = await _userRepository.GetUserByIdAsync(new GetUserByIdQuery((Guid)inserted.Data));

            // Assert
            Assert.True(updated.Succeeded);
            Assert.False(updated.Errors.Any());
            Assert.Equal((Guid)inserted.Data, result.UserId);
            Assert.Equal(name, result.Name);
            Assert.Equal(password, result.Password);
            Assert.Equal(role, result.Role);
        }

        [Theory]
        [InlineData(null, null, null, null, true)]
        [InlineData(null, "sm", "d", "", false)]
        [InlineData("", "user", "pw", "Role 1", true)]
        [InlineData("newGuid", "", "", "Role 2", false)]
        public async Task Update_User_Async_Should_Fail_On_Command_Validation(string userId, string name, string password, string role, bool status)
        {
            // Arrange
            Guid id = string.IsNullOrEmpty(userId) ? Guid.Empty : Guid.NewGuid();
            var command = new UpdateUserCommand(id, name, password, role, status);
            // Act
            var result = await _userRepository.UpdateUserAsync(command);

            // Assert
            Assert.False(result.Succeeded);
            Assert.True(result.Errors.Any());
        }

        [Theory]
        [InlineData("PWD!", "Manager", true)]
        [InlineData("PWD!!!", "Admin", false)]
        public async Task Update_User_Async_Should_Fail_On_UniqueKey_Violation(string password, string role, bool status)
        {
            // Arrange
            var users = await _userRepository.ListUsersAsync(new ListAllUsersQuery());
            Assert.NotNull(users);

            var existentUser = users.FirstOrDefault();
            var secondUser = users.LastOrDefault();
            Assert.NotEqual(existentUser.UserId, secondUser.UserId);

            // Act
            var result = await _userRepository.UpdateUserAsync(new UpdateUserCommand(secondUser.UserId, existentUser.Name, password, role, status));

            // Assert
            Assert.False(result.Succeeded);
            Assert.True(result.Errors.Any());
            Assert.True(result.Errors.Count() == 1);
            Assert.True(result.Errors.First().MemberNames.First() == "P0001");
        }

        [Theory]
        [InlineData("User 1", "PWD!", "Manager", true)]
        [InlineData("User 2", "PWD!!!", "Admin", true)]
        public async Task Disable_User_Async_Should_Update_User_on_Database(string name, string password, string role, bool status)
        {
            // Arrange
            var insertUser = new InsertUserCommand(name, password, role, status);
            var inserted = await _userRepository.InsertUserAsync(insertUser);

            Assert.NotNull(inserted.Data);

            // Act
            var updated = await _userRepository.DisableUserAsync(new DisableUserCommand((Guid)inserted.Data));

            // Ensure we have the same data on database
            var result = await _userRepository.GetUserByIdAsync(new GetUserByIdQuery((Guid)inserted.Data));

            // Assert
            Assert.True(updated.Succeeded);
            Assert.False(updated.Errors.Any());
            Assert.Equal(1, updated.Data);
            Assert.Equal((Guid)inserted.Data, result.UserId);
            Assert.Equal(name, result.Name);
            Assert.Equal(password, result.Password);
            Assert.Equal(role, result.Role);
            Assert.False(result.IsActive);
        }


        [Fact]
        public async Task Disable_User_Async_Should_Not_Affect_Database_On_Invalid_Id()
        {
            // Arrange
            var invalidId = Guid.NewGuid();

            // Act
            var result = await _userRepository.DisableUserAsync(new DisableUserCommand(invalidId));

            // Assert
            Assert.True(result.Succeeded);
            Assert.True(result.Data == 0);
            Assert.False(result.Errors.Any());
        }
    }
}
