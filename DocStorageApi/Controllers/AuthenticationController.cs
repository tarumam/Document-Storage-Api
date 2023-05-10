using DocStorageApi.DTO.Request;
using DocStorageApi.Identity;
using DocStorageApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DocStorageApi.Api.Controllers
{
    [Route("api/v1/authentication")]
    [AllowAnonymous]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        readonly IAuthenticationService _authService;
        readonly IUserService _userService;
        readonly DocStorageJwtService _jwtService;
        readonly ILogger _logger;

        public AuthenticationController(IAuthenticationService authService, IUserService userService, DocStorageJwtService jwtService, ILogger<AuthenticationController> logger)
        {
            _authService = authService;
            _userService = userService;
            _jwtService = jwtService;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new user with Regular Role
        /// </summary>
        /// <param name="authInfo">The authentication information of the user</param>
        /// <returns>The ID of the created user</returns>
        [HttpPost("SignUp")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SignUp([Required] AuthRequest authInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CreateUserRequest user = new()
            {
                Username = authInfo.userName,
                Password = authInfo.password,
                Role = JwtScopes.JwtScopeRegular
            };

            var userId = await _userService.CreateUserAsync(user);

            if (userId == null)
                return Problem(
               detail: "The userneame provided is unavailable.",
               statusCode: StatusCodes.Status400BadRequest);
            else
                return Ok(userId);
        }


        /// <summary>
        /// Signs in a user and returns a JWT
        /// </summary>
        /// <param name="authInfo">The authentication information of the user</param>
        /// <returns>The JWT token with Regular user role</returns>
        [HttpPost("SignIn")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SignIn([Required] AuthRequest authInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _authService.SignInAsync(authInfo);

            if (user == null)
            {
                return Problem(
                detail: "Wasn't possible to sign up, please review the information provided.",
                statusCode: StatusCodes.Status401Unauthorized);
            }

            var (tokenId, jwtToken) = _jwtService.GenerateJwtFor(user.UserId, user.Role);

            var userUpdated = await _userService.UpdateTokenIdAsync(user.UserId, tokenId);

            if (!userUpdated)
            {
                _logger.LogWarning("Wasn't possible to save the token Id {tokenId} for userId {userId}", tokenId, user.UserId);
            }

            return Ok(jwtToken);
        }
    }
}
