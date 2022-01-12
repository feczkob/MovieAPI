using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MovieApi.Dtos.Authentication;
using Microsoft.AspNetCore.Identity;
using MovieApi.Models.Authentication;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MovieApi.ViewModels.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace MovieApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private IConfiguration _configuration;

        public AuthenticationController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);
            if(user != null && await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Name, user.UserName)
                };

                foreach(var role in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken
                (
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );

                return Ok(new LoginVm
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expires = token.ValidTo
                });
            }
            return Unauthorized();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var existingUser = await _userManager.FindByIdAsync(registerDto.UserName);
            if(existingUser != null)
            {
                throw new HttpResponseException("User already exists.", StatusCodes.Status500InternalServerError);
            }

            var newUser = new ApplicationUser
            {
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(newUser, registerDto.Password);
            if (!result.Succeeded)
                throw new HttpResponseException("Something is wrong during user creation.", StatusCodes.Status500InternalServerError);

            return Ok();
        }

        [HttpPut("addRole")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AddRole([FromQuery] string userName, [FromQuery] string roleName)
        {
            var existingUser = await _userManager.FindByNameAsync(userName);
            if (existingUser == null)
            {
                throw new HttpResponseException("User does not exist.", StatusCodes.Status404NotFound);
            }

            if(roleName != UserRoles.Admin && roleName != UserRoles.User)
                throw new HttpResponseException("The specified role does not exist.", StatusCodes.Status404NotFound);

            var result = await _userManager.AddToRoleAsync(existingUser, roleName);

            if (!result.Succeeded)
                throw new HttpResponseException("Something is wrong during role assignment.", StatusCodes.Status500InternalServerError);

            return Ok();
        }
    }
}
