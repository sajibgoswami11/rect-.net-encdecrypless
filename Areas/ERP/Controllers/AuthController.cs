using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BizWebAPI.Areas.ERP.Interfaces;
using BizWebAPI.Areas.ERP.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BizWebAPI.Areas.ERP.Controllers
{
    [Area("ERP")]
    [ApiController]
    [Route("ERP/[controller]")]
    public class AuthController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IAuth _auth;

        public AuthController(IConfiguration config, IAuth auth)
        {
            _config = config;
            _auth = auth;
        }
        
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(ErpAuthLoginRequestDto ibAuthDto)
        {
            try
            {
                var user = await _auth.Login(ibAuthDto.Username, ibAuthDto.Password, HttpContext);

                if (user == null)
                {
                    return Unauthorized(new { Message = "Invalid username or password" });
                }

                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.Username)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddHours(7),
                    SigningCredentials = creds
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);

                return Ok(new
                {
                    Message = "Login Successful",
                    token = tokenHandler.WriteToken(token),
                    userId = user.UserId,
                    username = user.Username,
                    userGroup = user.UserGroup,
                    empId = user.EmpId,
                    title = user.Title,
                    name = user.Name,
                    image = user.Image,
                    email = user.Email
                });
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _auth.CommonMessage() });
            }
        }
    }
}