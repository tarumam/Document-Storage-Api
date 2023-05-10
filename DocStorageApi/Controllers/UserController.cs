using DocStorageApi.DTO.Request;
using DocStorageApi.DTO.Response;
using DocStorageApi.Identity;
using DocStorageApi.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocStorageApi.Api.Controllers
{
    [ApiController]
    [Route("api/v1/User")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        /// <summary>
        /// Lists all users.
        /// </summary>
        /// <returns>A collection of user objects.</returns>
        /// <response code="200">Returns a collection of user objects.</response>
        /// <response code="204">If there are no users in the system.</response>
        [HttpGet("ListAllUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(IEnumerable<UserResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListUsers()
        {
            IEnumerable<UserResponse> users = await _userService.ListUsersAsync();

            if (users.Any())
                return Ok(users);
            else
                return NoContent();
        }

        /// <summary>
        /// Gets the user by ID.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve.</param>
        /// <returns>The user object with the specified ID.</returns>
        /// <response code="200">Returns the user object with the specified ID.</response>
        /// <response code="204">If the user with the specified ID was not found.</response>
        [HttpGet("GetUserById/{userId}", Name = nameof(GetUserById))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            UserResponse user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="userRequest">The user request object containing the user data.</param>
        /// <returns>The created user ID.</returns>
        /// <response code="201">Returns the created user ID.</response>
        /// <response code="400">If the provided user request object is invalid.</response>
        /// <remarks>Requires Admin Access</remarks>
        [HttpPost("CreateUser")]
        [Authorize(Roles = CanAccess.AdminAccess)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest userRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = await _userService.CreateUserAsync(userRequest);

            if(userId == null)
                return BadRequest();

            return CreatedAtRoute(nameof(GetUserById), new { userId }, null);
        }

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="userRequest">The user request object containing the updated user data.</param>
        /// <returns>No content.</returns>
        /// <response code="204">If the user was updated successfully.</response>
        /// <response code="400">If the provided user request object is invalid.</response>
        /// <response code="404">If the user to be updated was not found.</response>
        /// <remarks>Requires admin access</remarks>
        [HttpPut("UpdateUser")]
        [Authorize(Roles = CanAccess.AdminAccess)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest userRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.UpdateUserAsync(userRequest);

            if(!result)
                return BadRequest();

            return NoContent();
        }

        /// <summary>
        /// Disables the user with the specified ID.
        /// </summary>
        /// <param name="userId">The ID of the user to disable.</param>
        /// <returns>No content.</returns>
        /// <response code="204">If the user was disabled successfully.</response>
        /// <response code="404">If the user to be disabled was not found.</response>
        /// <remarks>Requires Admin access</remarks>
        [HttpPut("DisableUser/{userId}")]
        [Authorize(Roles = CanAccess.AdminAccess)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DisableUser(Guid userId)
        {
            var result = await _userService.DisableUserAsync(userId);

            if(!result)
                return BadRequest();

            return NoContent();
        }

    }
}
