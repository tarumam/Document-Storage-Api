using DocStorageApi.DTO.Request;
using DocStorageApi.DTO.Response;
using DocStorageApi.Integration.Tests.Config;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace DocStorageApi.Integration.Tests
{
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class DocumentTest
    {
        private readonly IntegrationTestsFixture<Program> _fixture;

        public DocumentTest(IntegrationTestsFixture<Program> testsFixture)
        {
            _fixture = testsFixture;
        }

        [Fact]
        public async Task Upload_Returns201_When_Upload_Succeeds()
        {
            // Arrange  
            _fixture.Client.SetTokenToRequestHeader(_fixture.AdminToken);

            // Creates a file
            byte[] fileContent = Encoding.UTF8.GetBytes("Test file content");
            IFormFile file = new FormFile(new MemoryStream(fileContent), 0, fileContent.Length, "testFile", "test.txt");

            var uploadRequest = new FileUploadRequest(file, "Test file", "Test Description", "Test Category");

            // Act
            var response = await _fixture.Client.PostAsync($"{Constants.DocumentRoute}/FileUpload", new MultipartFormDataContent
            {
            { new StringContent(uploadRequest.Name), "Name" },
            { new StringContent(uploadRequest.Category), "Category" },
            { new StringContent(uploadRequest.Description), "Description" },
            { new StreamContent(uploadRequest.File.OpenReadStream())
                {
                    Headers =
                    {
                        ContentLength = uploadRequest.File.Length,
                        ContentType = new MediaTypeHeaderValue("image/jpeg")
                    }
                }   ,
                "File", uploadRequest.File.FileName
            }
            });

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task Download_Returns_File()
        {
            // Arrange  
            _fixture.Client.SetTokenToRequestHeader(_fixture.AdminToken);
            var fileName = "5df6a2a6-e5c4-4a4b-bb63-9e94b3cc4cf2.txt"; // This file exists on S3 bucket

            // Act
            var response = await _fixture.Client.GetAsync($"{Constants.DocumentRoute}/{fileName}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/octet-stream", response.Content.Headers.ContentType.MediaType);
            Assert.Equal(fileName, response.Content.Headers.ContentDisposition.FileNameStar);
        }

        [Fact]
        public async Task ListAllDocuments_Returns_List_Of_Documents()
        {
            // Arrange  
            _fixture.Client.SetTokenToRequestHeader(_fixture.AdminToken);

            // Act
            var response = await _fixture.Client.GetAsync($"{Constants.DocumentRoute}/ListAllDocuments");
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotEmpty(content);

            var documents = JsonConvert.DeserializeObject<IEnumerable<DocumentsResponse>>(content);
            var file = documents.FirstOrDefault(a => a.Name == "testFileName");

            Assert.Equal("5df6a2a6-e5c4-4a4b-bb63-9e94b3cc4cf2.txt", file.FilePath);
            Assert.Equal("testFileName", file.Name);
            Assert.Equal("category", file.Category);
            Assert.Equal("description", file.Description);
            Assert.True(file.CreatedAt > DateTime.UtcNow.AddSeconds(-10));
            Assert.True(file.Status);
        }
    }
}