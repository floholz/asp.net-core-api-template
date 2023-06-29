using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using asp.net_core_api_template.Database;
using asp.net_core_api_template.Models;
using asp.net_core_api_template.Models.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace asp.net_core_api_template.Services.Authentication
{
    public class TokenService
    {
        private readonly PostgresContext _postgresContext;
        private readonly TokenOptions _tokenOptions;
        private readonly UserService _userService;


        public TokenService(PostgresContext postgresContext, IOptions<TokenOptions> tokenOptions, UserService userService)
        {
            _postgresContext = postgresContext;
            _tokenOptions = tokenOptions.Value;
            _userService = userService;
        }

        public TokenService() { }
        

        /// <summary>
        /// generate jwt access tokens by options in appsettings.json
        /// access tokens are stored in ram
        /// </summary>
        /// <param name="user"></param>
        /// <param name="tokenOptions"></param>
        /// <returns></returns>
        public string CreateAccessToken(User user, TokenOptions tokenOptions)
        {
            var claim = new[]
            {
                new Claim(ClaimTypes.Name, user.Email)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                tokenOptions.Issuer,
                tokenOptions.Audience,
                claim,
                expires: DateTime.Now.AddMinutes(tokenOptions.AccessExpiration),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

        /// <summary>
        /// creates a random refresh token
        /// refresh tokens are stored in database
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string CreateRefreshToken(User user)
        {
            var expirationTime = DateTime.Now.AddMinutes(this._tokenOptions.RefreshExpiration);

            var refreshToken = new RefreshToken()
            {
                TokenValue = GenerateRandomString(25),
                ExpiryDate = expirationTime.ToString("yyyy-MM-dd HH:mm:ss"),
                UserId = _userService.GetUserByEmail(user.Email).Id
            };
            _postgresContext.RefreshTokens.Add(refreshToken);
            _postgresContext.SaveChanges();
            return refreshToken.TokenValue;
        }

        /// <summary>
        /// gets a refresh token from the database with provided tokenvalue
        /// </summary>
        /// <param name="tokenValue"></param>
        /// <returns></returns>
        public RefreshToken GetRefreshToken(string tokenValue)
        {
            return _postgresContext.RefreshTokens.SingleOrDefault(t => t.TokenValue.Equals(tokenValue));
        }

        /// <summary>
        /// deletes a refresh token
        /// </summary>
        /// <param name="token"></param>
        public void DeleteRefreshToken(RefreshToken token)
        {
            _postgresContext.RefreshTokens.Remove(token);
            _postgresContext.SaveChanges();
        }

        private static readonly Random RandomGen = new Random();
        /// <summary>
        /// generates a random refresh token value
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[RandomGen.Next(s.Length)]).ToArray());
        }
    }
}
