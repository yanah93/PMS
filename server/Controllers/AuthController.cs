using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PMS.Data;
using PMS.Dtos;
using PMS.Helpers;
using PMS.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly PMScontext _authContext;
        private readonly IConfiguration _configuration;

        public AuthController(PMScontext authContext, IConfiguration configuration)
        {
            this._authContext = authContext;
            this._configuration = configuration;
        }
        [HttpPost]
        public async Task<ActionResult<LoginDto>> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                if (loginDto == null)
                {
                    return BadRequest(new
                    {
                        StatusCode = 400,
                        Message = "Email and Password fields are empty!"
                    });
                }
                var user = await _authContext.UserAccounts.FirstOrDefaultAsync(x => x.Email == loginDto.Email);
                if (user == null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "User not found"
                    });
                }
                else
                {
                    if (!EncDecPassword.VerifyHashPassword(loginDto.Password, user.PasswordHash, user.PasswordSalt))
                    {
                        return BadRequest(new
                        {
                            StatusCode = 400,
                            Message = "Incorrect Password"
                        });
                    }
                    string token = CreateJwtToken(user);
                    return Ok(new
                    {
                        StatusCode = 200,
                        Message = "Login Success",
                        Token = token,
                    });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //payload, key, credential, expiryTime, token
        private string CreateJwtToken(UserAccountModel user)
        {
            List<Claim> claimsList = new List<Claim>
            {
                new Claim("Email" , user.Email),
                new Claim("Name", user.Username),
                new Claim("UserId", user.Id.ToString()),
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSetting:SecretKey").Value));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var token = new JwtSecurityToken(
                claims: claimsList,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
