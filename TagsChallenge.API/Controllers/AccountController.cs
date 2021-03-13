using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TagsChallenge.API.Configuration;
using TagsChallenge.API.ViewModels;
using TagsChallenge.Domain.Entities;

namespace TagsChallenge.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtConfig _jwtConfig;

        public AccountController(UserManager<User> userManager, IOptionsMonitor<JwtConfig> optionsMonitor)
        {
            _userManager = userManager;
            _jwtConfig = optionsMonitor.CurrentValue;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterVm user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(user.Email);

                if (existingUser != null)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Errors = "User aleready exist"
                    });
                }

                var newUser = new User() {
                    FirstName = user.FirstName,
                    LastName = user.LasetName,
                    Email = user.Email,
                    UserName = user.Email
                };

                var isCreated = await _userManager.CreateAsync(newUser, user.Password);
                if (isCreated.Succeeded)
                {
                    return Ok(new
                    {
                        Sucess = true,
                        Token = GenerateAuthToken(newUser)
                    });
                }

                return new JsonResult(new
                {
                    Sucess = false,
                    Token = "Something went wrong",
                    StatusCode = 500
                });
            }

            return BadRequest(new
            {
                Success = false,
                Errors = "Invalid requset"
            });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginVm user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(user.Email);

                if (existingUser == null)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Errors = "User doesn't exist"
                    });
                }

                var correctPass = await _userManager.CheckPasswordAsync(existingUser, user.Password);

                if (correctPass)
                {
                    return Ok(new
                    {
                        Success = true,
                        Token = GenerateAuthToken(existingUser)
                    }); ;
                }
                else
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Errors = "Check username and password"
                    });
                }
            }

            return BadRequest(new
            {
                Success = false,
                Errors = "Invalid requset"
            });
        }

        private string GenerateAuthToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim("Id", user.Id),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }
    }
}
