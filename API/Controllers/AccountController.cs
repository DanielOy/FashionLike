using API.Dtos;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, ITokenService tokenService, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            var user = new User
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                UserName = registerDto.Email,
                Email = registerDto.Email,
                CreationDate = DateTime.Now,
                Active = true
            };

            if (EmailExists(user.Email).Result.Value)
            {
                return BadRequest("Email in use");
            }

            var creationResult = await _userManager.CreateAsync(user, registerDto.Password);
            await _userManager.AddToRoleAsync(user, "Viewer");

            if (!creationResult.Succeeded)
            {
                return BadRequest("User can not be created, please retry");
            }

            return new UserDto
            {
                Email = user.Email,
                Name = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpGet("EmailExists")]
        public async Task<ActionResult<bool>> EmailExists([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null) return Unauthorized();

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized();

            return new UserDto
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                Name = user.UserName
            };
        }
    }
}
