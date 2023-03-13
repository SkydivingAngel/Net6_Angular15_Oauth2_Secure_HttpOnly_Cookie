using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Net6AngularOauth2.Controllers
{
    [ApiController]
    [Route("api")]
    public class LoginController : ControllerBase
    {
        private readonly JwtHandler jwtHandler;

        public LoginController(JwtHandler jwtHandler)
        {
            this.jwtHandler = jwtHandler;
        }

        [HttpPost, Route("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            string? jwt = await jwtHandler.GetTokenAsync(loginRequest);

            Response.Cookies.Append("X-Access-Token", jwt, new CookieOptions()
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Path = "/",
                //MaxAge = TimeSpan.FromMinutes(60)
            });

            return Ok(new LoginResult()
            {
                Success = true,
                Message = "Login successful",
                Token = jwt,
                Url = loginRequest.Url
            });
        }

        [Authorize]
        [HttpGet, Route("isloggedin")]
        public async Task<IActionResult> IsLoggedIn()
        {
            return await Task.FromResult(Ok());
        }
    }
}