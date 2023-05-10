using DocStorageApi.DTO.Request;
using DocStorageApi.DTO.Response;
using DocStorageApi.Identity;
using DocStorageApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DocStorageApi.Api.Controllers
{
    [Route("api/v1/permission")]
    [Authorize]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        // TODO: Put logger aqui
        readonly IAccessControlService _permissionSvc;
        readonly ILogger _logger;

        public PermissionsController(IAccessControlService permissionSvc, ILogger<PermissionsController> logger)
        {
            _permissionSvc = permissionSvc;
            _logger = logger;
        }

        /// <summary>
        /// list all access groups.
        /// </summary>
        /// <returns>A list of access groups.</returns>
        [HttpGet("ListAccessGroups")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(IEnumerable<AccessGroupResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListAccessGroups()
        {
            var status = await _permissionSvc.GetAllAccessGroupsAsync();

            if (status.Any())
                return Ok(status);

            return NoContent();
        }

        /// <summary>
        /// List of all documents permissions assigned to a user.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve documents permissions for.</param>
        /// <returns>A list of documents permissions assigned to the user.</returns>
        [HttpGet("ListDocumentPermissionsWithUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(IEnumerable<DocumentPermissionsWithUserResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListDocumentPermissionsWithUsers(Guid? userId = null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var status = await _permissionSvc.ListDocumentPermissionsWithUsers(userId);

            if (status.Any())
                return Ok(status);

            return NoContent();
        }

        /// <summary>
        /// List of all users with the access groups assigned to them.
        /// </summary>
        /// <param name="userId">(optional)The ID of the user to retrieve information.</param>
        /// <returns>A list of access groups assigned to the user if the user is informed, if not, returns all.</returns>
        [HttpGet("ListUserAccessGroups")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(IEnumerable<DocumentPermissionsWithUserResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListUserAccessGroups(Guid? userId = null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var status = await _permissionSvc.GetUserAccessGroups(userId);

            if (status.Any())
                return Ok(status);

            return NoContent();
        }

        /// <summary>
        /// Adds a user to an access group.
        /// </summary>
        /// <param name="request">The user and access group to add.</param>
        /// <returns>An HTTP status code indicating the success or failure of the operation.</returns>
        /// <remarks>Requires AdminAccess</remarks>
        [HttpPost("AssignUserToAccessGroups")]
        [Authorize(Roles = CanAccess.AdminAccess)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AssignUserToAccessGroups([Required] AssignUserToAccessGroupRequest request)
        {
            if (!ModelState.IsValid || request.userId == Guid.Empty || request.accessGroupId == Guid.Empty)
            {
                return BadRequest(ModelState);
            }
            var status = await _permissionSvc.AssignUserToAccessGroupsAsync(request);

            if (status)
                return Accepted();
            else
                return BadRequest();
        }

        /// <summary>
        /// Assigns a access group to a document
        /// </summary>
        /// <param name="request">The group id and document id to add.</param>
        /// <returns>An HTTP status code indicating the success or failure of the operation.</returns>
        /// <remarks>Requires AdminAccess</remarks>
        [HttpPost("AssignAccessGroupToDocument")]
        [Authorize(Roles = CanAccess.AdminAccess)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> AssignAccessGroupToDocument([Required] AssignGroupToAccessDocumentRequest request)
        {
            if (!ModelState.IsValid || request.AccessGroupId == Guid.Empty || request.DocumentId == Guid.Empty)
            {
                return BadRequest(ModelState);
            }

            var status = await _permissionSvc.GrantPermissionForGroupAsync(request);

            if (status)
                return Ok();

            return BadRequest();
        }

        /// <summary>
        /// List of all permissions assigned to a user.
        /// </summary>
        /// <param name="request">The user id and document id to add.</param>
        /// <returns>An HTTP status code indicating the success or failure of the operation.</returns>
        /// <remarks>Requires AdminAccess</remarks>
        [HttpPost("AssignUserToDocument")]
        [Authorize(Roles = CanAccess.AdminAccess)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> AssignUserToDocument([Required] AssignUserToAccessDocumentRequest request)
        {
            if (!ModelState.IsValid || request.UserId == Guid.Empty || request.DocumentId == Guid.Empty)
            {
                return BadRequest(ModelState);
            }

            var status = await _permissionSvc.GrantPermissionForUserAsync(request);

            if (status)
                return Ok();

            return BadRequest();
        }

        /// <summary>
        /// Removes a user from an access group.
        /// </summary>
        /// <param name="userId"> Id of the user to remove from the access group.</param>
        /// <param name="accessGroupId"> Id of the access group from which to remove the user.</param>
        /// <returns>An HTTP status code indicating the success or failure of the operation.</returns>
        /// <remarks>Requires AdminAccess</remarks>
        [HttpDelete("RemoveUserFromAccessGroup/{userId}/{accessGroupId}")]
        [Authorize(Roles = CanAccess.AdminAccess)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveUserFromAccessGroup([Required] Guid userId, [Required] Guid accessGroupId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var status = await _permissionSvc.RemoveUserFromAccessGroupsAsync(new RemoveUserFromAccessGroupRequest(userId, accessGroupId));

            if (status)
                return Accepted();
            else
                return BadRequest();
        }

        /// <summary>
        /// Removes an user acess to document.
        /// </summary>
        /// <param name="userId">Id of the user to remove access for.</param>
        /// <param name="documentId">Id of the document to remove access from.</param>
        /// <returns>An HTTP status code indicating the success or failure of the operation.</returns>
        /// <remarks>Requires AdminAccess</remarks>
        [HttpDelete("RemoveUserAccessFromDocument/{userId}/{documentId}")]
        [Authorize(Roles = CanAccess.AdminAccess)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveUserAccessFromDocument(Guid userId, Guid documentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var status = await _permissionSvc.RemoveUserAccessFromDocument(new RemoveUserAccessToDocumentRequest(documentId, userId));

            if (status)
                return Accepted();
            else
                return BadRequest();
        }

        /// <summary>
        /// Removes an access group access to document.
        /// </summary>
        /// <param name="accessGroupId">Access group id.</param>
        /// <param name="documentId">Document id.</param>
        /// <returns>An HTTP status code indicating the success or failure of the operation.</returns>
        /// <remarks>Requires AdminAccess</remarks>
        [HttpDelete("RemoveAccessGroupFromDocument/{accessGroupId}/{documentId}")]
        [Authorize(Roles = CanAccess.AdminAccess)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveUserAccessGroupFromDocument(Guid accessGroupId, Guid documentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var status = await _permissionSvc.RemoveUserAccessGroupFromDocument(new RemoveAccessGroupToDocumentRequest(documentId, accessGroupId));

            if (status)
                return Accepted();
            else
                return BadRequest();
        }
    }
}
