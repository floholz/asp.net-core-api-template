using System.Security.Claims;
using asp.net_core_api_template.Models;
using asp.net_core_api_template.Models.Exceptions;
using asp.net_core_api_template.Models.Requests;
using asp.net_core_api_template.Models.Responses;
using asp.net_core_api_template.Services;
using asp.net_core_api_template.Services.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace asp.net_core_api_template.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {

        private readonly ILogger<AuthenticationController> _logger;
        private readonly AuthenticationService _authenticationService;
        private readonly UserService _userService;

        public AuthenticationController(
            ILogger<AuthenticationController> logger,
            AuthenticationService authenticationService,
            UserService userService)
        {
            _logger = logger;
            _userService = userService;
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Login Route, handles the login request
        /// </summary>
        /// <param name="request"> Login request with email and password </param>
        [HttpPost, Route("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<AuthenticationResponse> Login([FromBody] LoginRequest request)
        {
            try
            {
                var response = _authenticationService.Authenticate(request.Email, request.Password);
                return Ok(response);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (InvalidCredentialsException e)
            {
                return Unauthorized(e.Message);
            }
        }

        /*
        /// <summary>
        /// Refresh tokens when access token was expired
        /// </summary>D:\_git\seedback-backend\SeedbackAPI\SeedbackAPI\Common\
        /// <param name="request">RefreshToken and Email as RefreshTokenRequest</param>
        [HttpPost, Route("refresh")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<AuthenticationResponse>> RefreshToken(string refreshTokenValue)
        {
            try
            {
                var response = await _authenticationService.RefreshToken(User.FindFirst(ClaimTypes.Name).Value, refreshTokenValue);
                return Ok(response);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (RefreshTokenExpiredException e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Logout that deletes the RefreshToken
        /// </summary>
        /// <param name="request">RefreshToken and Email as RefreshTokenRequest</param>
        [HttpPost, Route("logout")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult Logout(string refreshTokenValue)
        {

            // refresh token
            if (_authenticationService.RevokeToken(User.FindFirst(ClaimTypes.Name).Value, refreshTokenValue))
            {
                return Ok();
            }

            return BadRequest("Token already expired");
        }

        [HttpPost, Route("signup")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<AuthenticationResponse>> SignUp([FromBody] SignUpRequest request)
        {
            try
            {
                if (request.Role != "SL" && request.Role != "MA")
                {
                    throw new InvalidOperationException("Current user is not allowed to perform this operation.");
                }

                var userModel = new User()
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email.ToLower(),
                    CompanyID = request.CompanyId,
                    Role = request.Role,
                    Password = request.Password
                };

                await _userService.CreateUser(userModel, request.SecretToken);

                var response = _authenticationService.Authenticate(request.Email, request.Password);

                return Ok(response);
            }
            catch (Models.Exceptions.InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
            catch (UserNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }
        */
    }
}
