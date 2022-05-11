using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Dtos;
using PMS.Model;
using PMS.Services;
using System.Globalization;

namespace PMS.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly PMScontext _employeeContext;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;

        public EmployeeController(PMScontext employeeContext, IMapper mapper, IMailService mailService)
        {
            this._employeeContext = employeeContext;
            this._mapper = mapper;
            this._mailService = mailService;
        }
        //retrieve all employees in database
        [HttpGet]
        public async Task<ActionResult<EmployeeModel>>GetAllEmployee()
        {
            var employeeList = await _employeeContext.EmployeeModels.Include(x => x.UserAccount).ToListAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "Success",
                Result = employeeList
            });
        }

        //search by employeeCode need to be an exact match to find.
        [HttpGet("getEmployeeByCode")]
        public async Task<ActionResult<EmployeeModel>> GetEmployeeByCode(string Code)
        {
            var employee = await _employeeContext.EmployeeModels.Include(x => x.UserAccount).FirstOrDefaultAsync(x => x.EmployeeeCode == Code);
            if (employee == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Employee not found"
                });
            }
            return Ok(new
            {
                StatusCode = 200,
                Message = "Success",
                Results = employee
            });
        }

        //only loads the first record with the search field. 
        //NEED TO FIND BETTER WAY TO SEARCH.
        //PENDING
        [HttpGet("getEmployeeByName")]
        public async Task<ActionResult<EmployeeModel>> GetEmployeeByName(string Name)
        {
            var employee = await _employeeContext.EmployeeModels.FirstOrDefaultAsync(x => x.EmployeeName.Contains(Name));
            if (employee == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Employee not found"
                });
            }
            return Ok(new
            {
                StatusCode = 200,
                Message = "Success",
                Results = employee
            });
        }

        //Create new employee record - employeeCode for new employee
        [HttpPost("addEmployee")]
        public async Task<ActionResult<EmployeeDto>> AddEmployee ([FromBody]EmployeeDto employeeDtoObj)
        {
            if (employeeDtoObj == null)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Please send in data"
                });
            }
            var employee = await _employeeContext.EmployeeModels.FirstOrDefaultAsync(x => x.EmployeeeCode == employeeDtoObj.EmployeeeCode);
            var emply = await _employeeContext.EmployeeModels.FirstOrDefaultAsync(x => x.UserAccountId == employeeDtoObj.UserAccountId);
            if(employee != null && emply != null)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "EmployeeCode and User already assigned!"
                });

            }
            if (employee != null)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "EmployeeCode already Exist"
                });

            }
            if (emply != null)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "User already assigned to an employeeCode"
                });

            }
            
            var employeeObj = _mapper.Map<EmployeeModel>(employeeDtoObj);
            //auto-capitalize the alphabet in the employeeCode
            employeeObj.EmployeeeCode = CapitalizeFirstLetter(employeeObj.EmployeeeCode);

            //Takes the value of the first name and last name from the user account.
            var userAcc = await _employeeContext.UserAccounts.FirstOrDefaultAsync(x =>x.Id == employeeDtoObj.UserAccountId);
            var userAccount = _mapper.Map<UserAccountDto>(userAcc);
            userAccount.Password = "Accenture2022";
            employeeObj.EmployeeName = userAcc.FirstName + " " + userAcc.LastName;

            employeeObj.UserAccountId = employeeDtoObj.UserAccountId;
            await _employeeContext.EmployeeModels.AddAsync(employeeObj);
            await _employeeContext.SaveChangesAsync();

            MailRequest mailRequest = new MailRequest();
            mailRequest.ToEmail = userAcc.Email;
            mailRequest.Subject = "Account Created!";
            mailRequest.Body = "Hi " + userAcc.FirstName + " " + userAcc.LastName + "," + "<br>" +
                                "<p>Your Profile has been created in our Company</p>"
                                + "Below is your Login Credentials for accessing the Portal<br>"
                                + "Your Email : " + "<strong>" + userAcc.Email + "</strong><br>"
                                + "Your Password : " + "<strong>" + userAccount.Password + "</strong><br>"
                                + "<br><br>"
                                + "Kindly change your password after initial " + "<a href='http://localhost:4200/login'" + ">Log in.</a>"
                                + "<br><br>Kind Regards,<br>Maryanah Yusoff";

            await _mailService.SendEmailAsync(mailRequest);

            return Ok(new
            {
                StatusCode = 200,
                Message = "Success"
            });
        }

        public static string CapitalizeFirstLetter(string value)
        {
            //In Case if the entire string is in UpperCase then convert it into lower
            value = value.ToLower();
            char[] array = value.ToCharArray();
            // Handle the first letter in the string.
            if (array.Length >= 1)
            {
                if (char.IsLower(array[0]))
                {
                    array[0] = char.ToUpper(array[0]);
                }
            }
            // Scan through the letters, checking for spaces.
            // ... Uppercase the lowercase letters following spaces.
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == ' ')
                {
                    if (char.IsLower(array[i]))
                    {
                        array[i] = char.ToUpper(array[i]);
                    }
                }
            }
            return new string(array);
        }

        //Update any changes for existing employee detail
        [HttpPut("updateEmployee")]
        public async Task<ActionResult<EmployeeDto>> UpdateEmployee ([FromBody] EmployeeDto employeeDtoObj)
        {
            if(employeeDtoObj == null)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Please send in data"
                });
            }
            var isEmployeeExist = await _employeeContext.EmployeeModels.AsNoTracking().FirstOrDefaultAsync(x => x.UserAccountId == employeeDtoObj.UserAccountId);
            if (isEmployeeExist == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Employee not found"
                });
            }
            var employee = _mapper.Map<EmployeeModel>(employeeDtoObj);
            employee.EmployeeName = CapitalizeFirstLetter(employee.EmployeeName);
            _employeeContext.Entry(employee).State = EntityState.Modified;
            await _employeeContext.SaveChangesAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "Success",
                Result = employee
            });
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<EmployeeDto>> DeleteEmployee (int id)
        {
            var employee = await _employeeContext.EmployeeModels.FindAsync(id);
            if(employee == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Employee not found"
                });
            }
            _employeeContext.EmployeeModels.Remove(employee);
            await _employeeContext.SaveChangesAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "Employee Deleted"
            });
        }
    }
}
