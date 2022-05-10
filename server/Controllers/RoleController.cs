using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Model;

namespace PMS.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly PMScontext _roleContext;

        public RoleController(PMScontext roleContext)
        {
            this._roleContext = roleContext;
        }
        [HttpGet("getAllRoles")]
        public async Task<ActionResult<RoleModel>>GetAllRoles()
        {
            var roleList = await _roleContext.RoleModels.ToListAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "Success",
                Result = roleList
            });
        }
        [HttpPost("addRole")]
        public async Task<ActionResult<RoleModel>> AddRole([FromBody]RoleModel roleObj)
        {
            if (roleObj == null)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Please send in data"
                });
            }
            await _roleContext.RoleModels.AddAsync(roleObj);
            await _roleContext.SaveChangesAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "Successfully Added"
            });
        }
        [HttpPut("updateRole")]
        public async Task<ActionResult<RoleModel>> UpdateRole([FromBody]RoleModel roleObj)
        {
            if (roleObj == null)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Please send in data"
                });
            }
            var isRoleExist = await _roleContext.RoleModels.AsNoTracking().FirstOrDefaultAsync(x => x.Id == roleObj.Id);
            if (isRoleExist == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Role not found"
                });
            }
            _roleContext.Entry(roleObj).State = EntityState.Modified;
            await _roleContext.SaveChangesAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "Role updated"
            });
        }
        [HttpDelete("{id}")]
        public async Task <ActionResult<RoleModel>> DeleteRole(int id)
        {
            var role = await _roleContext.RoleModels.FindAsync(id);
            if (role == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Role not found"
                });
            }
            _roleContext.RoleModels.Remove(role);
            await _roleContext.SaveChangesAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "Role deleted"
            });
        }
    }
}
