using Api.Dtos;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IUserService userService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<LoginResponseDto>> Register(CreateUserDto request)
        {
            try
            {
                var user = await userService.CreateUserAsync(request.UserName, request.Password);

                var token = "en jwt token kommer härl";

                return new LoginResponseDto(token, user.Id, user.UserName);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        // TODO auth
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginDto request)
        {
            var user = await userService.AuthenticateAsync(request.UserName, request.Password);

            if (user == null)
                return Unauthorized();

            var token = "en jwt token kommer härl";

            return new LoginResponseDto(token, user.Id, user.UserName);
        }

        // TODO auth
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDto>> GetUserById(int id)
        {
            var user = await userService.GetUserByIdAsync(id);

            if (user == null)
                return NotFound();

            return new UserResponseDto(user.Id, user.UserName, user.ToDonts.Count);
        }

        // TODO auth
        [HttpGet("profile/{username}")]
        public async Task<ActionResult<UserResponseDto>> GetUserProfile(string username)
        {
            var user = await userService.GetUserByUserNameAsync(username);

            if (user == null)
                return NotFound();

            return new UserResponseDto(user.Id, user.UserName, user.ToDonts.Count);
        }
    }
}
