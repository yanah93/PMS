using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Dtos;
using PMS.Helpers;
using PMS.Model;
using PMS.Services;

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
        [HttpGet("getUserById{id}")]
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
            var userAcc = _mapper.Map<UserAccountModel>(userAccDtoObj);
            userAccDtoObj.Password = "Accenture2022";
            EncDecPassword.CreateHashPassword(userAccDtoObj.Password, out byte[] passwordHash, out byte[] passwordSalt);
            userAcc.PasswordHash = passwordHash;
            userAcc.PasswordSalt = passwordSalt;
            userAcc.FirstName = EmployeeController.CapitalizeFirstLetter(userAccDtoObj.FirstName);
            userAcc.LastName = EmployeeController.CapitalizeFirstLetter(userAccDtoObj.LastName);
            userAcc.RegistrationTime = DateTime.Now;
            await _userContext.AddAsync(userAcc);
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
            var userAcc = _mapper.Map<UserAccountModel>(userAccDtoObj);
            //Keeps value unchanged
            userAcc.PasswordSalt = isUserExist.PasswordSalt;
            userAcc.PasswordHash = isUserExist.PasswordHash;
            userAcc.RegistrationTime = isUserExist.RegistrationTime;
            //Capitalized first letter of the word
            userAcc.FirstName = EmployeeController.CapitalizeFirstLetter(userAccDtoObj.FirstName);
            userAcc.LastName = EmployeeController.CapitalizeFirstLetter(userAccDtoObj.LastName);
            //updates the database
            _userContext.Entry(userAcc).State = EntityState.Modified;
            //saves the changes
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
