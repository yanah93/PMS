using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PMS.Data;
using PMS.Dtos;
using PMS.Helpers;
using PMS.Model;
using PMS.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PMS.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly PMScontext _authContext;
        private readonly IConfiguration _configuration;
        private readonly IMailService _mailService;

        public AuthController(PMScontext authContext, IConfiguration configuration, IMailService mailService)
        {
            this._authContext = authContext;
            this._configuration = configuration;
            this._mailService = mailService;
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
                new Claim("ProjectManager", user.IsProjectManager.ToString()),
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
        [HttpPut("updatePassword")]
        public async Task<ActionResult<ResetPasswordDto>> updatePassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            try
            {
                //Check if fields are filled up
                if (resetPasswordDto == null)
                {
                    return BadRequest(new
                    {
                        StatusCode = 400,
                        Message = "Enter email to reset password"
                    });
                }
                else
                {
                    var user = await _authContext.UserAccounts.FirstOrDefaultAsync(a => a.Email == resetPasswordDto.Email);
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
                        if (!EncDecPassword.VerifyHashPassword(resetPasswordDto.OldPassword, user.PasswordHash, user.PasswordSalt))
                        {
                            return BadRequest(new
                            {
                                StatusCode = 400,
                                Message = "Incorrect password"
                            });
                        }
                        EncDecPassword.CreateHashPassword(resetPasswordDto.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);
                        user.PasswordHash = passwordHash;
                        user.PasswordSalt = passwordSalt;

                        _authContext.Entry(user).State = EntityState.Modified;
                        await _authContext.SaveChangesAsync();

                        MailRequest mailRequest = new MailRequest();
                        mailRequest.ToEmail = user.Email;
                        mailRequest.Subject = "Password updated!";
                        mailRequest.Body = "Hi " + user.FirstName + " " + user.LastName + "," + "<br>" +
                                            "<p>Your Password has been updated as requested.</p>"

                                            + "If you did not request to change password, kindly go on the below link to reset your password!<br>" +
                                            "<a href='http://localhost:4200/blog/usersettings/" + user.Id + "'>Reset Password!</a>" +
                                            "<br><br>Kind Regards,<br>Management Department";

                        await _mailService.SendEmailAsync(mailRequest);
                        return Ok(new
                        {
                            Status = 200,
                            Message = "Password Changed"
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPut("resetPassword")]
        public async Task<ActionResult<ResetPasswordDto>> ForgetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            try
            {
                //Check if fields are filled up
                if (resetPasswordDto == null)
                {
                    return BadRequest(new
                    {
                        StatusCode = 400,
                        Message = "Enter email to reset password"
                    });
                }
                else
                {
                    var user = await _authContext.UserAccounts.FirstOrDefaultAsync(a => a.Email == resetPasswordDto.Email);
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
                        /*if (!EncDecPassword.VerifyHashPassword(resetPasswordDto.OldPassword, user.PasswordHash, user.PasswordSalt))
                        {
                            return BadRequest(new
                            {
                                StatusCode = 400,
                                Message = "Incorrect password"
                            });
                        }*/

                        int length = 5;

                        // creating a StringBuilder object()
                        StringBuilder str_build = new StringBuilder();
                        Random random = new Random();

                        char letter;

                        for (int i = 0; i < length; i++)
                        {
                            double flt = random.NextDouble(); //generates a float between 0.0 and 1.0 inclusive
                            int shift = Convert.ToInt32(Math.Floor(25 * flt)); // returns an interger between 0 and 25 inclusive
                            letter = Convert.ToChar(shift + 97); //97 which is small a ASCII value. So this converts the value to a lowercase character
                            str_build.Append(letter); //prints the letter
                        }
                        int randomData = random.Next(1000, 9999); //chooses a random number between 1000 and 9999 inclusive

                        resetPasswordDto.NewPassword = str_build.ToString() + randomData; //puts the random characters and numbers together
                        EncDecPassword.CreateHashPassword(resetPasswordDto.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);
                        user.PasswordHash = passwordHash;
                        user.PasswordSalt = passwordSalt;

                        _authContext.Entry(user).State = EntityState.Modified;
                        await _authContext.SaveChangesAsync();

                        MailRequest mailRequest = new MailRequest();
                        mailRequest.ToEmail = user.Email;
                        mailRequest.Subject = "Reset Password";
                        mailRequest.Body = "Hi " + user.FirstName + " " + user.LastName + "," + "<br>" +
                                            "<p>Your Password has been reset as requested.</p>"
                                            + "Below is your Login Credentials for accessing the Portal<br>"
                                            + "Your Email : " + "<strong>" + user.Email + "</strong><br>"
                                            + "Your Password : " + "<strong>" + resetPasswordDto.NewPassword + "</strong><br>"
                                            + "<br><br>"
                                            + "If you did not request to change password, kindly go on the below link to reset your password!<br>" +
                                            "<a href='http://localhost:4200/blog/usersettings/" + user.Id + "'>Reset Password!</a>" +
                                            "<br><br>Kind Regards,<br>Management Department";

                        await _mailService.SendEmailAsync(mailRequest);

                        return Ok(new
                        {
                            Status = 200,
                            Message = "Kindly check your email inbox for the new password.",
                            Result = resetPasswordDto.NewPassword
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
