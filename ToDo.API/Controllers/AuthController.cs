using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToDo.API.Dtos;
using ToDo.API.Models;
using ToDo.API.Utils;

namespace ToDo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly CustomSignInManager _customSignInManager;
        private readonly ILogger<AuthController> _logger;

        public AuthController(UserManager<User> userManager, CustomSignInManager customSignInManager, 
            ILogger<AuthController> logger)
        {
            this._userManager = userManager;
            this._customSignInManager = customSignInManager;
            this._logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for registration: {@ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                var user = new User
                {
                    UserName = model.Username,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User registered successfully: {Username}", user.UserName);
                    return Ok();
                }

                _logger.LogWarning("User registration failed: {@Errors}", result.Errors);
                return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while registering a user.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for login: {@ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    _logger.LogWarning("Invalid login attempt for user: {Username}", model.Username);
                    return Unauthorized();
                }

                var token = await _customSignInManager.GenerateJwtTokenAsync(user);
                _logger.LogInformation("User logged in successfully: {Username}", user.UserName);
                return Ok(new { user = new UserDto(user), Token = token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while logging in.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
