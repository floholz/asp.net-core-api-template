using asp.net_core_api_template.Models.Authentication;
using asp.net_core_api_template.Models.Exceptions;
using asp.net_core_api_template.Models.Responses;
using Microsoft.Extensions.Options;

namespace asp.net_core_api_template.Services.Authentication
{
    public class AuthenticationService
    {
        private readonly UserService _userService;
        private readonly TokenOptions _tokenOptions;
        private readonly TokenService _tokenService;

        public AuthenticationService(UserService userService, IOptions<TokenOptions> tokenOptions, TokenService tokenService)
        {
            _userService = userService;
            _tokenOptions = tokenOptions.Value;
            _tokenService = tokenService;
        }

        public async Task<AuthenticationResponse> Authenticate(string email, string password)
        {
            var authResponse = new AuthenticationResponse();
            var authenticatedUser = await _userService.ValidateUser(email, password);
            if (authenticatedUser == null)
            {
                throw new InvalidCredentialsException("Password for this user is incorrect");
            }
            
            // write access and refresh token into response
            authResponse.AccessToken = _tokenService.CreateAccessToken(authenticatedUser, _tokenOptions);
            authResponse.RefreshToken = _tokenService.CreateRefreshToken(authenticatedUser);
            authenticatedUser.Password = "";
            authResponse.User = authenticatedUser;
            return authResponse;
        }

        /*
        public bool RevokeToken(string email, string refreshTokenValue)
        {
            // make sure user exists
            if (_userService.GetUserByEmail(email) == null)
            {
                return false;
            }

            // delete refresh token, this means there is no way to get a valid access token for this user
            var refreshToken = _tokenService.GetRefreshToken(refreshTokenValue);
            if (refreshToken != null)
            {
                _tokenService.DeleteRefreshToken(refreshToken);
                return true;
            }

            return false;
        }

        public async Task<AuthenticationResponse> RefreshToken(string email, string refreshTokenValue)
        {
            var response = new AuthenticationResponse();
            // make sure user exists
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
            {
                throw new UserNotFoundException("User with this Email doesn't exist");
            }

            // delete token, if it exists
            var refreshToken = _tokenService.GetRefreshToken(refreshTokenValue);
            if (refreshToken == null || refreshToken.IsExpired())
            {
                throw new RefreshTokenExpiredException("This refresh token already expired");
            }
            {
                _tokenService.DeleteRefreshToken(refreshToken);
            }

            // generation of tokens shouldn't give errors, everything is checked beforehand
            response.AccessToken = _tokenService.CreateAccessToken(user, _tokenOptions);
            response.RefreshToken = _tokenService.CreateRefreshToken(user);
            user.Password = "";
            response.User = user;
            return response;
        }
        */
    }
}
