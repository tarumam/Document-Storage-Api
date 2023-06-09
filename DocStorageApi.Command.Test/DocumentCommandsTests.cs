﻿using DocStorageApi.Data.Commands;
using DocStorageApi.Data.Queries;
using DocStorageApi.DbObjects.Test.Config;
using DocStorageApi.Domain.Repository.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DocStorageApi.Command.Test
{
    public class DocumentCommandsTests : IClassFixture<DocStorageAppFactory<Program>>
    {
        private readonly DocStorageAppFactory<Program> _factory;
        private readonly IUserRepository _userRepository;
        private readonly IDocumentRepository _docRepository;

        public DocumentCommandsTests()
        {
            _factory = new DocStorageAppFactory<Program>();
            _userRepository = _factory.Services.GetService<IUserRepository>();
            _docRepository = _factory.Services.GetService<IDocumentRepository>();

        }

        [Theory]
        [InlineData("document1.txt", "document name 1", "document category 1", "Document description 1")]
        [InlineData("document2.txt", "", "document category 2", "Document description 2")]
        [InlineData("document3.txt", "document name 3", "", "Document description 3")]
        [InlineData("document4.txt", "document name 4", "document category 4", "")]
        [InlineData("document5.txt", null, null, null)]

        public async Task AddDocumentAsync_Should_Insert_Document_Into_Database(string filePath, string name, string category, string description)
        {
            // Arrange
            var users = await _userRepository.ListUsersAsync(new ListAllUsersQuery());
            var userId = users.FirstOrDefault().UserId;
            var command = new InsertDocumentCommand(filePath, name, category, description, DateTime.Now, true, userId);

            // Act
            var result = await _docRepository.AddNewDocumentAsync(command);

            // Assert

            var documents = await _docRepository.GetAllDocumentsAsync();
            var inserted = documents.Where(d => d.FilePath == filePath).FirstOrDefault();

            Assert.True(result.Succeeded);
            Assert.NotNull(result.Data);
            Assert.NotNull(inserted);
            Assert.Equal(filePath, inserted.FilePath);
            Assert.Equal(name, inserted.Name);
            Assert.Equal(category, inserted.Category);
            Assert.Equal(description, inserted.Description);
        }

        [Theory]
        [InlineData("document1.txt", "document name 1", "", "Document description 1")]
        [InlineData("document2.txt", "", "document category 2", "Document description 2")]
        [InlineData("document3.txt", "Name", "Category", "Description")]

        public async Task AddDocumentAsync_Should_Fail_On_UniqueKey_Violation(string filePath, string name, string category, string description)
        {
            // Arrange
            var users = await _userRepository.ListUsersAsync(new ListAllUsersQuery());
            var userId = users.FirstOrDefault().UserId;

            var command = new InsertDocumentCommand(filePath, name, category, description, DateTime.Now, true, userId);
            var result = await _docRepository.AddNewDocumentAsync(command);

            // Act
            var newDocCommand = new InsertDocumentCommand(filePath, "Name", "Category", "Description", DateTime.Now, true, userId);
            var newDoc = await _docRepository.AddNewDocumentAsync(newDocCommand);

            // Assert

            Assert.False(newDoc.Succeeded);
            Assert.Null(newDoc.Data);
            Assert.True(newDoc.Errors.Any());
            Assert.True(newDoc.Errors.Count() == 1);
            Assert.True(newDoc.Errors.First().MemberNames.First() == "P0001");
        }

        [Theory]
        [InlineData("document1.txt", "document name 1", "", "Document description 1")]
        [InlineData("document2.txt", "", "document category 2", "Document description 2")]
        [InlineData("document3.txt", "Name", "Category", "Description")]

        public async Task AddDocumentAsync_Should_Fail_On_User_Constraint_Violation(string filePath, string name, string category, string description)
        {
            // Arrange
            var unknownUserId = Guid.NewGuid();

            // Act
            var command = new InsertDocumentCommand(filePath, name, category, description, DateTime.Now, true, unknownUserId);
            var result = await _docRepository.AddNewDocumentAsync(command);
            // Assert

            var documents = await _docRepository.GetAllDocumentsAsync();
            var inserted = documents.Where(d => d.FilePath == filePath).FirstOrDefault();

            Assert.Null(inserted);
            Assert.False(result.Succeeded);
            Assert.Null(result.Data);
            Assert.True(result.Errors.Any());
            Assert.True(result.Errors.Count() == 1);
            Assert.True(result.Errors.First().MemberNames.First() == "P0001");
        }

        [Theory]
        [InlineData("", "document name 1", "", "Document description 1")]
        [InlineData(null, "", "document category 2", "Document description 2")]

        public async Task AddDocumentAsync_Should_Fail_On_Command_Violation(string filePath, string name, string category, string description)
        {
            // Arrange
            var unknownUserId = Guid.NewGuid();

            // Act
            var command = new InsertDocumentCommand(filePath, name, category, description, DateTime.Now, true, unknownUserId);
            var result = await _docRepository.AddNewDocumentAsync(command);
            // Assert

            var documents = await _docRepository.GetAllDocumentsAsync();
            var inserted = documents.Where(d => d.FilePath == filePath).FirstOrDefault();

            Assert.Null(inserted);
            Assert.False(result.Succeeded);
            Assert.True(result.Errors.Any());
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task DisableDocumentCommand_Should_Fail_When_Invalid_Id()
        {
            // Arrange
            var command = new DisableDocumentCommand(Guid.NewGuid());

            // Act
            var result = await _docRepository.DisableDocumentAsync(command);

            // Assert
            Assert.True(result.Succeeded);
            Assert.False(result.Errors.Any());
            Assert.Equal(0, result.Data);
        }
    }
}
