using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GamesGlobal.Infrastructure.Interfaces;
using GamesGlobal.Infrastructure.Models;

namespace GamesGlobal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLoginController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserLoginController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            var authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("Callback"),
                Items =
                {
                    { "prompt", "consent" }
                }
            };

            return Challenge(authenticationProperties, OpenIdConnectDefaults.AuthenticationScheme);
        }

        [HttpGet("Callback")]
        public async Task<IActionResult> Callback()
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.Identity.Name);

            if (user == null)
            {
                // User doesn't exist in the Users table, so add them
                await _userRepository.CreateUserAsync(new User
                {
                    Username = User.Identity.Name, Email = User.FindFirst("email")?.Value
                });
            }

            // This action will be called after successful login or consent
            // You can implement your own logic here, e.g., redirect to a specific route
            return Ok("User logged in or consent granted.");
        }
    }
}
