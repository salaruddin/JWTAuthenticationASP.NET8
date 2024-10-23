using JwtAuthentication_ASP.net8.Core.Dtos;
using JwtAuthentication_ASP.net8.Core.OtherObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtAuthentication_ASP.net8.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }


        [HttpGet]
        [Route("seed-roles")]
        public async Task<IActionResult> SeedRoles()
        {
            if (await _roleManager.RoleExistsAsync(StaticUserRoles.ADMIN))
            {
                return Ok("Roles already exists");
            }
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.USER));
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.ADMIN));
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.OWNER));

            return Ok("Roles seeded successfully");
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var userExists = await _userManager.FindByNameAsync(dto.UserName);
            if (userExists is not null)
                return BadRequest("User is already exists");

            var user = new IdentityUser { UserName = dto.UserName, Email = dto.Email, SecurityStamp = Guid.NewGuid().ToString() };
            var createResult =await _userManager.CreateAsync(user,dto.Password);

            if (!createResult.Succeeded)
            {
                string errors = "Can't Create User, because: ";
                foreach (var item in createResult.Errors)
                {
                    errors += $"# {item.Description}";
                }
                return StatusCode(statusCode: StatusCodes.Status500InternalServerError, errors);
            }

            var addToRoleResult = await _userManager.AddToRoleAsync(user,StaticUserRoles.USER);

            if (!addToRoleResult.Succeeded)
            {
                string errors = "Can't Add User to User Role, because: ";
                foreach (var item in createResult.Errors)
                {
                    errors += $"# {item.Description}";
                }
                return StatusCode(statusCode: StatusCodes.Status500InternalServerError, errors);
            }

            return Ok("User Created Successfully");
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);

            if (user is null)
                return Unauthorized("Invalid Credentials, Username or Password is incorrect");

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if(!isPasswordCorrect)
                return Unauthorized("Invalid Credentials, Username or Password is incorrect");

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.NameIdentifier,user.UserName),
                new Claim("JWTID",Guid.NewGuid().ToString())
            };

            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role,role));
            }

            var token =  GenerateNewJsonWebToken(authClaims);

            return Ok(token);

        }

        private string GenerateNewJsonWebToken(List<Claim> claims)
        {
            var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var tokenObject = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(1),
                    claims: claims,
                    signingCredentials: new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256)
                );

            string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);

            return token;
        }


    }
}
