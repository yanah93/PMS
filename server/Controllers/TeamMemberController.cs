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
    public class TeamMemberController : ControllerBase
    {
        private readonly PMScontext _teamMemberContext;
        private readonly IMapper _mapper;

        public TeamMemberController(PMScontext teamMemberContext, IMapper mapper)
        {
            this._teamMemberContext = teamMemberContext;
            this._mapper = mapper;
        }
        [HttpGet("getAllTeamMembers")]
        public async Task<ActionResult<TeamMembersDto>> GetAllTeamMembers()
        {
            var teamMembersList = await _teamMemberContext.TeamMemberModels.Include(x => x.Team).Include(x => x.Role).Include(x => x.Employee).ToListAsync();
            var mappedMembersList = _mapper.Map<List<TeamMembersDto>>(teamMembersList);
            return Ok(new
            {
                StatusCode = 200,
                Message = "Success",
                Result = teamMembersList
            });
        }
        [HttpGet("getTeamMembersByTeamId_{id}")]
        public async Task<ActionResult<TeamMemberModel>> GetTeamMembersByTeamId(int id)
        {
            var teamMember = _teamMemberContext.TeamMemberModels.Where(x => x.TeamId == id).Include(x => x.Team).Include(x => x.Role).Include(x => x.Employee);
            var teamMem = await _teamMemberContext.TeamMemberModels.FirstOrDefaultAsync(x => x.TeamId == id);
            if (teamMem == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Team not found"
                });
            }

            return Ok(new
            {
                StatusCode = 200,
                Message = "Success",
                Result = teamMember
            });
        }
        [HttpGet("getTeamMembersByEmployeeId_{id}")]
        public async Task<ActionResult<TeamMemberModel>> GetTeamMembersByEmployeeId(int id)
        {
            var teamMember = _teamMemberContext.TeamMemberModels.Where(x => x.EmployeeId == id).Include(x => x.Employee).Include(x => x.Team).Include(x => x.Role);
            var teamMem = await _teamMemberContext.TeamMemberModels.FirstOrDefaultAsync(x => x.EmployeeId == id);
            if (teamMem == null)
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
                Result = teamMember
            });
        }

        [HttpGet("getTeamMembersByTeamName_{teamName}")]
        public async Task<ActionResult<TeamMemberModel>> GetTeamMembersByTeamName(string teamName)
        {
            var teamMember = _teamMemberContext.TeamMemberModels.Where(x => x.Team.TeamName.Equals(teamName)).Include(x => x.Team).Include(x => x.Role).Include(x => x.Employee);
            var teamMem = await _teamMemberContext.TeamMemberModels.FirstOrDefaultAsync(x => x.Team.TeamName.Equals(teamName));
            if (teamMem == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Team not found"
                });
            }

            return Ok(new
            {
                StatusCode = 200,
                Message = "Success",
                Result = teamMember
            });
        }

        [HttpPost("addTeamMembersToTeam")]
        public async Task<ActionResult<TeamMembersDto>> AddTeamMember(TeamMembersDto teamMemberDtoObj)
        {
            if (teamMemberDtoObj == null)
            {
                return BadRequest(new
                {
                    StatusCode = 200,
                    Message = "Please send in data"
                });
            }
            var teamMemberObj = _mapper.Map<TeamMemberModel>(teamMemberDtoObj);
            teamMemberObj.TeamId = teamMemberDtoObj.TeamId;
            teamMemberObj.RoleId = teamMemberDtoObj.RoleId;
            teamMemberObj.EmployeeId = teamMemberDtoObj.EmployeeId;
            await _teamMemberContext.TeamMemberModels.AddAsync(teamMemberObj);
            await _teamMemberContext.SaveChangesAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "Success"
            });
        }
        [HttpPut("editMember")]
        public async Task<ActionResult<TeamMembersDto>> UpdateTeamMembers(TeamMembersDto teamMemberDtoObj)
        {
            if (teamMemberDtoObj == null)
            {
                return BadRequest(new
                {
                    Status = 200,
                    Message = "Please send in data"
                });
            }
            var isTeamMembersExist = await _teamMemberContext.TeamMemberModels.AsNoTracking().FirstOrDefaultAsync(x => x.Id == teamMemberDtoObj.TeamId);
            if (isTeamMembersExist == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Member not found"
                });
            }
            var teamMem = _mapper.Map<TeamMemberModel>(teamMemberDtoObj);
            _teamMemberContext.Entry(teamMem).State = EntityState.Modified;
            await _teamMemberContext.SaveChangesAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "Team Member updated"
            });
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<TeamMemberModel>> RemoveTeamMember(int id)
        {
            var member = await _teamMemberContext.TeamMemberModels.FindAsync(id);
            if (member == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Member not found"
                });
            }
            _teamMemberContext.TeamMemberModels.Remove(member);
            await _teamMemberContext.SaveChangesAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "Member Deleted"
            });
        }


    }
}
