using DocStorageApi.DTO.Request;
using DocStorageApi.Integration.Tests.Config;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace DocStorageApi.Integration.Tests
{
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class PermissionTests
    {
        private readonly IntegrationTestsFixture<Program> _fixture;

        public PermissionTests(IntegrationTestsFixture<Program> testsFixture)
        {
            _fixture = testsFixture;
        }

        [Fact]
        public async Task RemoveUserFromAccessGroup_ReturnsAcceptedStatusCode()
        {
            // Arrange
            _fixture.Client.SetTokenToRequestHeader(_fixture.AdminToken);
            var userAccessGroup = _fixture.ValidUserAccessGroup.FirstOrDefault();
            var accessGroupId = userAccessGroup.AccessGroups.Select(a => a.AccessGroupId).FirstOrDefault();

            // Act
            var response = await _fixture.Client.DeleteAsync($"{Constants.PermissionsRoute}/RemoveUserFromAccessGroup/{userAccessGroup.UserId}/{accessGroupId}");

            // Assert
            response.EnsureSuccessStatusCode();

            var result = await _fixture.ListUserAccessGroupDataAsync();

            var resultUser = result.Where(u => u.UserId == userAccessGroup.UserId).Select(a => a.AccessGroups.Where(ag => ag.AccessGroupId == accessGroupId)).Any();

            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
            Assert.False(resultUser);
        }

        [Fact]
        public async Task RemoveUserAccessFromDocument_ReturnsAcceptedStatusCode()
        {
            // Arrange
            _fixture.Client.SetTokenToRequestHeader(_fixture.AdminToken);
            var previousUwp = _fixture.ValidDocsWithPermissions.Where(a => a.AccessType == "U").FirstOrDefault();

            // Act
            var response = await _fixture.Client.DeleteAsync($"{Constants.PermissionsRoute}/RemoveUserAccessFromDocument/{previousUwp.UserId}/{previousUwp.DocumentId}");
            var uwp = await _fixture.ListDocumentsWithPermissionsByUser(previousUwp.UserId);

            var result = uwp.Where(a => a.AccessType == "U" && a.UserId == previousUwp.UserId && a.DocumentId == previousUwp.DocumentId).FirstOrDefault();
            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
            Assert.Null(result);
        }

        [Fact]
        public async Task RemoveUserAccessGroupFromDocument_ReturnsAcceptedStatusCode()
        {
            // Arrange
            _fixture.Client.SetTokenToRequestHeader(_fixture.AdminToken);
            var previousUwp = _fixture.ValidDocsWithPermissions.Where(a => a.AccessType == "G").FirstOrDefault();
            var numberOfItems = _fixture.ValidDocsWithPermissions.Count();
            var accessGroupId = previousUwp.AccessGroupId;
            var documentId = previousUwp.DocumentId;
            // Act
            var response = await _fixture.Client.DeleteAsync($"{Constants.PermissionsRoute}/RemoveAccessGroupFromDocument/{accessGroupId}/{documentId}");
            var uwp = await _fixture.ListDocumentsWithPermissionsByUser(null);
            var uwpNumberOfItems = uwp.Count();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
        }

        [Fact]
        public async Task AssignUserToAccessGroups_ReturnsAcceptedStatusCode()
        {
            // Arrange
            _fixture.Client.SetTokenToRequestHeader(_fixture.AdminToken);
            var authModel = new AuthRequest("testUser", "testPassword");
            var signUpResponse = await _fixture.Client.PostAsJsonAsync($"{Constants.AuthenticationRoute}/SignUp", authModel);
            signUpResponse.EnsureSuccessStatusCode();
            Guid userId = await signUpResponse.Content.ReadFromJsonAsync<Guid>();
            Guid accessGroupId = _fixture.ValidAccessGroup.FirstOrDefault().Id;

            //Act
            var response = await _fixture.Client.PostAsJsonAsync($"{Constants.PermissionsRoute}/AssignUserToAccessGroups", new AssignUserToAccessGroupRequest(userId, accessGroupId));

            // Assert
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
        }

        [Fact]
        public async Task AssignUserToAccessGroups_ReturnsBadRequestStatusCode_WhenModelStateInvalid()
        {
            // Arrange
            _fixture.Client.SetTokenToRequestHeader(_fixture.AdminToken);
            Guid userId = Guid.Empty;
            Guid accessGroupId = _fixture.ValidAccessGroup.FirstOrDefault().Id;

            //Act
            var response = await _fixture.Client.PostAsJsonAsync($"{Constants.PermissionsRoute}/AssignUserToAccessGroups", new AssignUserToAccessGroupRequest(userId, accessGroupId));

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task AssignAccessGroupToDocument_ReturnsOkStatusCode()
        {
            // Arrange
            _fixture.Client.SetTokenToRequestHeader(_fixture.AdminToken);
            var accessGroupId = _fixture.ValidAccessGroup.FirstOrDefault().Id;
            var documentId = _fixture.ValidDocuments.FirstOrDefault().Id;

            // Act
            var response = await _fixture.Client.PostAsJsonAsync($"{Constants.PermissionsRoute}/AssignAccessGroupToDocument", new AssignGroupToAccessDocumentRequest(accessGroupId, documentId));

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }


        [Fact]
        public async Task AssignUserToDocument_ReturnsOkStatusCode()
        {
            // Arrange
            _fixture.Client.SetTokenToRequestHeader(_fixture.AdminToken);
            var userId = _fixture.ValidUsers.FirstOrDefault().UserId;
            var documentId = _fixture.ValidDocuments.FirstOrDefault().Id;

            var request = new AssignUserToAccessDocumentRequest(userId, documentId);

            // Act
            var response = await _fixture.Client.PostAsJsonAsync($"{Constants.PermissionsRoute}/AssignUserToDocument", request);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task AssignUserToDocument_ReturnsBadRequestStatusCode_WhenModelStateInvalid()
        {
            // Arrange
            _fixture.Client.SetTokenToRequestHeader(_fixture.AdminToken);
            var request = new AssignUserToAccessDocumentRequest(Guid.Empty, Guid.Empty);

            // Act
            var response = await _fixture.Client.PostAsJsonAsync($"{Constants.PermissionsRoute}/AssignUserToDocument", request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
