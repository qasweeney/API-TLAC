using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Threading.Tasks;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authServ;

        public AuthController(IAuthService authService)
        {
            authServ = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginReq request)
        {
            var user = await authServ.AuthenticateAsync(request.UserName, request.Password, request.UserType);
            if (user.IsSuccess == false)
            {
                return Unauthorized();
            }

            string? token = await authServ.GenerateTokenAsync(user.UserId, user.UserType);
            if (token == null)
                return StatusCode(500, "Failed to generate session token.");

            Response.Cookies.Append("sessionToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(2)
            });

            return Ok(new { user.UserId, user.UserType });
        }

        [HttpGet("validate")]
        public async Task<IActionResult> ValidateToken()
        {
            if (!Request.Cookies.ContainsKey("sessionToken"))
                return Unauthorized();

            string token = Request.Cookies["sessionToken"];

            var user = await authServ.ValidateTokenAsync(token);
            if (user == null)
                return Unauthorized();

            return Ok(new { userId = user.Value.userId, userType = user.Value.userType });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            if (!Request.Cookies.ContainsKey("sessionToken"))
                return BadRequest("No session token exists");

            string token = Request.Cookies["sessionToken"];
            bool loggedOut = await authServ.LogoutAsync(token);

            if (!loggedOut)
                return StatusCode(500, "Failed to logout.");

            Response.Cookies.Delete("sessionToken");
            return Ok(new { loggedOut = true });
        }
    }
}