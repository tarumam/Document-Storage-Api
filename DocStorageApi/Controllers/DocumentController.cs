using DocStorageApi.DTO.Request;
using DocStorageApi.DTO.Response;
using DocStorageApi.Identity;
using DocStorageApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocStorageApi.Api.Controllers
{
    [Authorize]
    [Route("api/v1/Document")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IS3Service _s3Service;
        private readonly IDocumentService _documentService;
        public DocumentController(IS3Service s3Serivce, IDocumentService documentService)
        {
            _s3Service = s3Serivce;
            _documentService = documentService;
        }

        /// <summary>
        /// Download a file from server
        /// </summary>
        /// <param name="fileName">Name(key) of the file to download</param>
        /// <returns>FileStreamResult</returns>
        /// <remarks>Requires RegularAccess</remarks>
        [HttpGet("{fileName}", Name = nameof(Download))]
        [Authorize(Roles = CanAccess.RegularAccess)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(File), StatusCodes.Status200OK)]
        public async Task<IActionResult> Download(string fileName)
        {
            var result = await _s3Service.DownloadWithMetadata(fileName);
            if (result == null)
            {
                return NotFound();
            }

            return File(result, "application/octet-stream", fileName);
        }

        /// <summary>
        /// List of all permissions assigned to a user.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve permissions for.</param>
        /// <returns>A list of permissions assigned to the user.</returns>
        [HttpGet("ListAllDocuments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(DocumentsResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListAllDocuments()
        {
            var status = await _documentService.GetAllDocumentsAsync();

            if (status.Any())
                return Ok(status);

            return NoContent();
        }

        /// <summary>
        /// Upload a file to server
        /// </summary>
        /// <param name="uploadInfo">FileUploadRequest</param>
        /// <returns>Filename(key)</returns>
        /// <remarks>Requires ManagerAccess</remarks>
        [HttpPost("FileUpload")]
        [Authorize(Roles = CanAccess.ManagerAccess)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(CreatedAtRouteResult), StatusCodes.Status201Created)]
        public async Task<IActionResult> Upload([FromForm] FileUploadRequest uploadInfo)
        {
            if (uploadInfo.File == null || uploadInfo.File.Length == 0)
            {
                return BadRequest("Select a file to upload.");
            }

            FileUploadResponse result = await _s3Service.UploadFileAsync(uploadInfo);

            if (!result.Status) return BadRequest();

            var postedAt = result.postedDate.HasValue ? result.postedDate.Value : DateTime.UtcNow;

            var success = await _documentService.AddNewDocumentAsync(new AddNewDocumentRequest(result.Key, uploadInfo.Name, uploadInfo.Category, uploadInfo.Description, postedAt));

            if (success)
                return CreatedAtRoute(nameof(Download), new { fileName = result.Key }, null);
            else
                return BadRequest();

        }
    }
}
