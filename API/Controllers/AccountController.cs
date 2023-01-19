using System.Security.Claims;
using API.DTOs;
using API.Models;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly TokenService _tokenService;
        public AccountController(UserManager<User> userManager, IMapper mapper, TokenService tokenService)
        {
            _tokenService = tokenService;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await _userManager.Users.AnyAsync(x => x.UserName == registerDto.Username))
            {
                ModelState.AddModelError("username", "---> Username already taken");
                return ValidationProblem();
            }

            if (await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email))
            {
                ModelState.AddModelError("email", "Email already taken");
                return ValidationProblem();
            }

            var user = new User();

            user = _mapper.Map(registerDto, user);

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            var userDto = new UserDto();
            userDto.Token = _tokenService.CreateToken(user);

            if (result.Succeeded)
            {
                return userDto = _mapper.Map(user, userDto);
            }

            return BadRequest("---> Problem registering user");
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == loginDto.Email);

            if (user != null)
            {
                var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

                if (result)
                {
                    var userDto = new UserDto();
                    userDto.Token = _tokenService.CreateToken(user);
                    return userDto = _mapper.Map(user, userDto);
                }
            }

            return Unauthorized();
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == User.FindFirstValue(ClaimTypes.Email));

            var userDto = new UserDto();
            userDto.Token = _tokenService.CreateToken(user);
            return userDto = _mapper.Map(user, userDto);
        }
    }
}