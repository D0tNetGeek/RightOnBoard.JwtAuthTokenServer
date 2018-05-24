using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using RightOnBoard.JwtAuthTokenServer.API.Models;
using RightOnBoard.JwtAuthTokenServer.Service.Interfaces;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace RightOnBoard.JwtAuthTokenServer.API.Controllers
{
    [Route("api/Token")]
    [EnableCors("CorsPolicy")]
    public class TokenController : Controller
    {
        private readonly IUserService _userService;
        private readonly ITokenStoreService _tokenService;
        private readonly IAntiForgeryCookieService _antiForgeryCookieService;

        public TokenController(
            IUserService userService,
            ITokenStoreService tokenService,
            IAntiForgeryCookieService antiForgeryCookieService)
        {
            _userService = userService;
            _tokenService = tokenService;
            _antiForgeryCookieService = antiForgeryCookieService;
        }

        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateToken([FromForm] User loginUser)
        {
            if(loginUser == null)
            {
                return BadRequest("user is not set !!");
            }

            var user = await _userService.FindUserAsync(loginUser.Username, loginUser.Password);

            if(user?.IsActive != null && ((bool) !user?.IsActive))
            {
                return Unauthorized();
            }

            var (accessToken, refreshToken, claims) = await _tokenService.CreateJwtToken(user, refreshTokenSource: null);

            _antiForgeryCookieService.RegenerateAntiForgeryCookies(claims);

            return Ok(new { access_token = accessToken, refresh_token = refreshToken });
        }

        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        //public async Task<IActionResult> RefreshToken([FromBody] JToken jsonBody)
        public async Task<IActionResult> RefreshToken(string refreshToken)
        {
            //var refreshToken = jsonBody.Value<string>("refreshToken");

            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return BadRequest("JWT Auth Server : RefreshToken is not set.");
            }

            var token = await _tokenService.FindTokenAsync(refreshToken);

            token.User.SerialNumber = token.User.Id;
            token.User.Issuer = "Insypher";

            var (accessToken, newRefreshToken, claims) = await _tokenService.CreateJwtToken(token.User, refreshToken);

            _antiForgeryCookieService.RegenerateAntiForgeryCookies(claims);

            return Ok(new {access_token = accessToken, refresh_token = newRefreshToken});
        }

        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        [HttpGet("[action]")]
        public async Task<bool> Logout(string userId, string refreshToken)
        {
            await _tokenService.RevokeUserBearerTokensAsync(userId, refreshToken);

            _antiForgeryCookieService.DeleteAntiForgeryCookies();

            return true;
        }
    }
}