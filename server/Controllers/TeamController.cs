using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Dtos;
using PMS.Model;

namespace PMS.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly PMScontext _teamContext;

        public TeamController(PMScontext teamContext)
        {
            this._teamContext = teamContext;
        }
        [HttpGet("getAllTeams")]
        public async Task<ActionResult<TeamModel>> GetAllTeams ()
        {
            var teamList = await _teamContext.TeamModels.ToListAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "Success",
                Result = teamList
            });
        }
        
        [HttpPost("addTeam")]
        public async Task<ActionResult<TeamModel>> AddTeam ([FromBody]TeamModel teamModelObj)
        {
            if (teamModelObj == null)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Please send in data"
                });
            }
            var team = await _teamContext.TeamModels.FirstOrDefaultAsync(x => x.TeamName == teamModelObj.TeamName);
            if (team != null)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Team already exist!"
                });
            }
            await _teamContext.TeamModels.AddAsync(teamModelObj);
            await _teamContext.SaveChangesAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "Success"
            });
        }
        
        [HttpPut("editTeam")]
        public async Task <ActionResult<TeamModel>> UpdateTeam ([FromBody]TeamModel teamModelObj)
        {
            if(teamModelObj == null)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Please send in data"
                });
            }
            var isTeamExist = await _teamContext.TeamModels.AsNoTracking().FirstOrDefaultAsync(x => x.Id == teamModelObj.Id);
            if(isTeamExist == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Team not found"
                });
            }
            _teamContext.Entry(teamModelObj).State = EntityState.Modified;
            await _teamContext.SaveChangesAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "Success",
                Result = teamModelObj
            });
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult<TeamModel>> DeleteTeam(int id)
        {
            var team = await _teamContext.TeamModels.FindAsync(id);
            if (team == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Team not found"
                });
            }
            _teamContext.TeamModels.Remove(team);
            await _teamContext.SaveChangesAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "Team Deleted"
            });
        }


    }
}
