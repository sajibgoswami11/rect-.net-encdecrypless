using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BizWebAPI.Areas.xEmptyArea.Interfaces;
using BizWebAPI.Areas.xEmptyArea.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BizWebAPI.Areas.xEmptyArea.Controllers
{
    [Area("xEmptyArea")]
    [ApiController]
    [Route("xEmptyArea/[controller]")]
    public class AuthController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IAuth _auth;

        public AuthController(IConfiguration config, IAuth auth)
        {
            _config = config;
            _auth = auth;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(xEmptyAuthDto ibAuthDto)
        {
            try
            {
                var user = await _auth.Login(ibAuthDto.Username, ibAuthDto.Password, HttpContext);
                
                if (user == null)
                {
                    return Unauthorized("Invalid username or password");
                }

                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddHours(1),
                    SigningCredentials = creds
                };

                var tokenHandler = new JwtSecurityTokenHandler();

                var token = tokenHandler.CreateToken(tokenDescriptor);

                return Ok(new
                {
                    token = tokenHandler.WriteToken(token),
                    statusCode = StatusCode(200),
                    username = user.Username
                });
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(await _auth.CommonMessage());
            }
        }
    }
}