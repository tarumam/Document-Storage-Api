using DocStorageApi.DTO.Request;
using DocStorageApi.DTO.Response;
using Newtonsoft.Json;
using System.Data;
using System.Net.Http.Json;
using System.Text;

namespace DocStorageApi.Integration.Tests.Config
{
    [CollectionDefinition(nameof(IntegrationApiTestsFixtureCollection))]
    public class IntegrationApiTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixture<Program>> { }
    public class IntegrationTestsFixture<TProgram> : IDisposable where TProgram : class
    {
        public string AdminToken { get; set; }
        public IEnumerable<UserResponse> ValidUsers { get; set; }
        public IEnumerable<DocumentsResponse> ValidDocuments { get; set; }
        public IEnumerable<AccessGroupResponse> ValidAccessGroup { get; set; }
        public IEnumerable<UserAccessGroupsResponse> ValidUserAccessGroup { get; set; }
        public IEnumerable<DocumentPermissionsWithUserResponse> ValidDocsWithPermissions { get; set; }

        public readonly DocStorageAppFactory<TProgram> Factory;
        public HttpClient Client;

        public IntegrationTestsFixture()
        {
            Factory = new DocStorageAppFactory<TProgram>();
            Client = Factory.CreateClient();
            LoadUserDataAsync().Wait();
        }


        // Load Testable data
        private async Task LoadUserDataAsync()
        {
            AdminToken = await SignInAs("Admin");
            ValidUsers = await ListUsers();
            ValidDocuments = await ListDocumentDataAsync();
            ValidAccessGroup = await ListAccessGroupDataAsync();
            ValidUserAccessGroup = await ListUserAccessGroupDataAsync();
            ValidDocsWithPermissions = await ListDocumentsWithPermissionsByUser(null);
        }

        public async Task<string> SignInAs(string role)
        {
            var authModel = new AuthRequest(role, "string");
            var response = await Client.PostAsJsonAsync($"{Constants.AuthenticationRoute}/SignIn", authModel);

            response.EnsureSuccessStatusCode();
            var token = await response.Content.ReadAsStringAsync();
            return token;
        }

        public async Task<IEnumerable<UserResponse>> ListUsers()
        {
            var response = await Client.GetAsync($"{Constants.UserRoute}/ListAllUsers");

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<IEnumerable<UserResponse>>(content);

            return users;
        }

        public async Task<IEnumerable<DocumentsResponse>> ListDocumentDataAsync()
        {
            Client.SetTokenToRequestHeader(AdminToken);
            var response = await Client.GetAsync($"{Constants.DocumentRoute}/ListAllDocuments");
            var content = await response.Content.ReadAsStringAsync();
            var docs = JsonConvert.DeserializeObject<IEnumerable<DocumentsResponse>>(content);
            return docs;
        }

        public async Task<IEnumerable<AccessGroupResponse>> ListAccessGroupDataAsync()
        {
            Client.SetTokenToRequestHeader(AdminToken);
            var response = await Client.GetAsync($"{Constants.PermissionsRoute}/ListAccessGroups");
            var content = await response.Content.ReadAsStringAsync();
            var ag = JsonConvert.DeserializeObject<IEnumerable<AccessGroupResponse>>(content);
            return ag;
        }

        public async Task<IEnumerable<UserAccessGroupsResponse>> ListUserAccessGroupDataAsync()
        {
            Client.SetTokenToRequestHeader(AdminToken);
            var response = await Client.GetAsync($"{Constants.PermissionsRoute}/ListUserAccessGroups");
            var content = await response.Content.ReadAsStringAsync();
            var uag = JsonConvert.DeserializeObject<IEnumerable<UserAccessGroupsResponse>>(content);
            return uag;
        }

        public async Task<IEnumerable<DocumentPermissionsWithUserResponse>> ListDocumentsWithPermissionsByUser(Guid? userId)
        {
            Client.SetTokenToRequestHeader(AdminToken);
            StringBuilder url = new StringBuilder($"{Constants.PermissionsRoute}/ListDocumentPermissionsWithUsers");
            if (userId != null) url.Append($"?userId={userId}");
            var response = await Client.GetAsync(url.ToString());
            var content = await response.Content.ReadAsStringAsync();
            var dpu = JsonConvert.DeserializeObject<IEnumerable<DocumentPermissionsWithUserResponse>>(content);
            return dpu;
        }
        public void Dispose()
        {
            Client.Dispose();
            Factory.Dispose();
        }
    }
}
