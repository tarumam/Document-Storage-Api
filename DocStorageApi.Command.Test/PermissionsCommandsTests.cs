using DocStorageApi.Data.Commands;
using DocStorageApi.Data.Queries;
using DocStorageApi.DbObjects.Test.Config;
using DocStorageApi.Domain.Repository.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System.Data;

namespace DocStorageApi.Command.Test
{
    public class PermissionsCommandsTests : IClassFixture<DocStorageAppFactory<Program>>
    {
        private readonly DocStorageAppFactory<Program> _factory;
        private readonly IUserRepository _userRepository;
        private readonly IDocumentRepository _docRepository;
        private readonly IDocAccessUserRepository _docAccessUserRepo;
        private readonly IDocAccessGroupRepository _docAccessGroupRepo;
        private readonly IAccessGroupRepository _accessGroupRepo;

        public PermissionsCommandsTests()
        {
            _factory = new DocStorageAppFactory<Program>();
            _userRepository = _factory.Services.GetService<IUserRepository>();
            _docRepository = _factory.Services.GetService<IDocumentRepository>();
            _docAccessUserRepo = _factory.Services.GetService<IDocAccessUserRepository>();
            _docAccessGroupRepo = _factory.Services.GetService<IDocAccessGroupRepository>();
            _accessGroupRepo = _factory.Services.GetService<IAccessGroupRepository>();
        }

        [Fact]
        public async Task GrantDocPermissionForUserAsync_Should_Insert_Into_Database()
        {
            // Arrange
            var users = await _userRepository.ListUsersAsync(new ListAllUsersQuery());
            var user = users.FirstOrDefault();

            var newDocCommand = new InsertDocumentCommand("filepath.jpg", "name", "category", "description", DateTime.UtcNow, true, user.UserId);
            var newDoc = await _docRepository.AddNewDocumentAsync(newDocCommand);

            var documents = await _docRepository.GetAllDocumentsAsync();
            var document = documents.FirstOrDefault();

            var command = new AssignUserToDocumentCommand(user.UserId, (Guid)newDoc.Data, user.UserId);

            // Act
            var result = await _docAccessUserRepo.GrantDocPermissionForUserAsync(command);

            // Assert

            var userDocs = await _docAccessUserRepo.GetAllDocumentsAssociatedToUsersAsync(user.UserId);
            var userDoc = userDocs.FirstOrDefault(d => d.DocumentId == document.Id);

            Assert.True(result.Executed);
            Assert.False(result.Errors.Any());
            Assert.Equal(1, result.Data);
            Assert.NotNull(userDoc);
        }

        [Fact]
        public async Task GrantDocPermissionForUserAsync_Should_Fail_On_UniqueKey_Violation()
        {
            // Arrange
            var userDocs = await _docRepository.ListDocumentPermissionsWithUsers(null);
            var userDoc = userDocs.FirstOrDefault(d => d.AccessType == "U");

            var command = new AssignUserToDocumentCommand(userDoc.UserId, userDoc.DocumentId, userDoc.UserId);

            // Act
            var result = await _docAccessUserRepo.GrantDocPermissionForUserAsync(command);

            // Assert
            Assert.True(result.Executed);
            Assert.False(result.Errors.Any());
            Assert.Equal(0, result.Data);
        }

        [Fact]
        public async Task RemoveDocPermissionForUserAsync_Should_Remove_from_Database()
        {
            // Arrange
            var userDocs = await _docRepository.ListDocumentPermissionsWithUsers(null);
            var userDoc = userDocs.FirstOrDefault(d => d.AccessType == "U");
            var command = new RemoveDocumentAccessUser(userDoc.UserId, userDoc.DocumentId);

            // Act
            var result = await _docAccessUserRepo.RemoveDocPermissionForUserAsync(command);

            // Assert
            var resultUserDocs = await _docRepository.ListDocumentPermissionsWithUsers(null);
            var deleted = resultUserDocs.Where(u => u.DocumentId == userDoc.DocumentId && u.UserId == userDoc.UserId).Any();

            Assert.True(result.Executed);
            Assert.False(result.Errors.Any());
            Assert.Equal(1, result.Data);
            Assert.False(deleted);
        }

        [Fact]
        public async Task GrantDocPermissionForGroupAsync_Should_Insert_Into_Database()
        {
            // Arrange
            var groupDocs = await _docRepository.ListDocumentPermissionsWithUsers(null);
            var groupDoc = groupDocs.LastOrDefault(d => d.AccessType == "G");

            var newDocCommand = new InsertDocumentCommand("filepath.jpg", "name", "category", "description", DateTime.UtcNow, true, groupDoc.UserId);
            var newDoc = await _docRepository.AddNewDocumentAsync(newDocCommand);
            var command = new AssignGroupToDocumentCommand(groupDoc.AccessGroupId, (Guid)newDoc.Data, groupDoc.UserId);

            // Act
            var result = await _docAccessGroupRepo.GrantDocPermissionForGroupAsync(command);

            // Assert
            var groupDocsResult = await _docRepository.ListDocumentPermissionsWithUsers(groupDoc.UserId);
            var groupDocResult = groupDocsResult.FirstOrDefault(g => g.DocumentId == newDoc.Data);

            Assert.True(result.Executed);
            Assert.False(result.Errors.Any());
            Assert.Equal(1, result.Data);
            Assert.NotNull(groupDoc);

        }

        [Fact]
        public async Task GrantDocPermissionForGroupAsync_Should_Throw_UniqueKey_Violation()
        {
            // Arrange
            var groupDocs = await _docRepository.ListDocumentPermissionsWithUsers(null);
            var groupDoc = groupDocs.FirstOrDefault(d => d.AccessType == "U");
            var command = new AssignGroupToDocumentCommand(groupDoc.DocumentId, groupDoc.AccessGroupId, groupDoc.UserId);

            // Act
            Func<Task> act = async () => await _docAccessGroupRepo.GrantDocPermissionForGroupAsync(command);

            // Assert
            await Assert.ThrowsAsync<PostgresException>(act);

        }

        [Fact]
        public async Task RemoveDocPermissionFromGroupAsync_Should_Remove_From_Database()
        {
            // Arrange
            var groupDocs = await _docRepository.ListDocumentPermissionsWithUsers(null);
            var groupDoc = groupDocs.FirstOrDefault(d => d.AccessType == "G");
            var command = new RemoveDocumentAccessGroup(groupDoc.DocumentId, groupDoc.AccessGroupId);

            // Act
            var result = await _docAccessGroupRepo.RemoveDocPermissionFromGroupAsync(command);

            // Assert
            var resultGroupDocs = await _docRepository.ListDocumentPermissionsWithUsers(null);
            var deleted = resultGroupDocs.Where(u => u.DocumentId == groupDoc.DocumentId && u.AccessGroupId == groupDoc.AccessGroupId).Any();

            Assert.True(result.Executed);
            Assert.False(result.Errors.Any());
            Assert.Equal(1, result.Data);
            Assert.False(deleted);
        }
    }
}
