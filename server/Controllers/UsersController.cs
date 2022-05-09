using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Dtos;
using PMS.Helpers;
using PMS.Model;

namespace PMS.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly PMScontext _userContext;
        private readonly IMapper _mapper;
        public UsersController(PMScontext userContext, IMapper mapper)
        {
            this._userContext = userContext;
            this._mapper = mapper;
        }
        [HttpGet("getAllUsers")]
        public async Task<ActionResult<UserAccountDto>>GetAllUsers()
        {
            var userList = await _userContext.UserAccounts.ToListAsync();
            var mappedUserList = _mapper.Map<List<UserAccountDto>>(userList);
            return Ok(new
            {
                StatusCode = 200,
                Message = "Success",
                Result = mappedUserList
            });
        }
        [HttpGet("getUserById")]
        public async Task<ActionResult<UserAccountDto>>GetUserById(int id)
        {
            var user = await _userContext.UserAccounts.FindAsync(id);
            var mappedUser = _mapper.Map<UserAccountDto>(user);
            if(user == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "User Not Found"
                });
            }
            return Ok(new
            {
                StatusCode = 200,
                Message = "Success",
                Result = mappedUser
            });
        }
        [HttpPost("addUser")]
        public async Task<ActionResult<UserAccountDto>> AddUsers([FromBody] UserAccountDto userAccDtoObj)
        {
            if (userAccDtoObj == null)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Please send in data"
                });

            }
            var userAccDto = _mapper.Map<UserAccountModel>(userAccDtoObj);
            EncDecPassword.CreateHashPassword(userAccDtoObj.Password, out byte[] passwordHash, out byte[] passwordSalt);
            userAccDto.PasswordHash = passwordHash;
            userAccDto.PasswordSalt = passwordSalt;
            userAccDto.RegistrationTime = DateTime.Now;
            await _userContext.AddAsync(userAccDto);
            await _userContext.SaveChangesAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "Successfully added"
            });
        }
        [HttpPut("updateUser")]
        public async Task<ActionResult<UserAccountDto>> UpdateUser(UserAccountDto userAccDtoObj)
        {
            if (userAccDtoObj == null)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Please send in data"
                });
            }
            var isUserExist = await _userContext.UserAccounts.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userAccDtoObj.Id);
            if (isUserExist == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "User not found"
                });
            }
            var userAccDto = _mapper.Map<UserAccountModel>(userAccDtoObj);
            userAccDto.PasswordSalt = isUserExist.PasswordSalt;
            userAccDto.PasswordHash = isUserExist.PasswordHash;
            userAccDto.RegistrationTime = isUserExist.RegistrationTime;
            _userContext.Entry(userAccDto).State = EntityState.Modified;
            await _userContext.SaveChangesAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "User Updated"
            });
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserAccountDto>> DeleteUser (int id)
        {
            var user = await _userContext.UserAccounts.FindAsync(id);
            if (user == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "User not found"
                });
            }
            _userContext.UserAccounts.Remove(user);
            await _userContext.SaveChangesAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "User Deleted"
            });
        }
    }
}
