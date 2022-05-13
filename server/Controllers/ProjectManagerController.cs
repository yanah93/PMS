using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Model;

namespace PMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectManagerController : ControllerBase
    {
        private readonly PMScontext _pmContext;
        public ProjectManagerController(PMScontext pmContext)
        {
            this._pmContext = pmContext;
        }

        [HttpGet]
        public async Task<ActionResult<ProjectManagerModel>> GetAllProjectManagers()
        {
            var projectManagerList = await _pmContext.ProjectManagerModels.Include(a => a.UserAccount).ToListAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "Successfully displayed all project managers",
                Result = projectManagerList
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectManagerModel>> GetProjectManagersById(int id)
        {
            var projectManagers = await _pmContext.ProjectManagerModels.FindAsync(id);
            if (projectManagers == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Project Manager does not exist"
                });
            }
            return Ok(new
            {
                StatusCode = 200,
                Message = "Successfully found Project Manager",
                Result = projectManagers
            });
        }

        [HttpPost("add")]
        public async Task<ActionResult<ProjectManagerModel>> AddProjectManager([FromBody] ProjectManagerModel projectMgrObj)
        {
            if (projectMgrObj == null)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Please input data to add!"
                });
            }
            var projectMgr = await _pmContext.ProjectManagerModels.AddAsync(projectMgrObj);
            await _pmContext.SaveChangesAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "Successfully added Project"
            });
        }

        [HttpPut("update")]
        public async Task<ActionResult<ProjectManagerModel>> UpdateProjectManager([FromBody] ProjectManagerModel projectMgrObj)
        {
            if (projectMgrObj == null)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Please send data to update!"
                });
            }
            var isPMExist = await _pmContext.ProjectManagerModels.AsNoTracking().FirstOrDefaultAsync(a => a.Id == projectMgrObj.Id);
            if (isPMExist == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "PM not Found"
                });
            }
            _pmContext.Entry(projectMgrObj).State = EntityState.Modified;
            await _pmContext.SaveChangesAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "Project manager updated!"
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ProjectManagerModel>> DeleteProjectMgr(int id)
        {
            var projectMgr = await _pmContext.ProjectManagerModels.FindAsync(id);
            if (projectMgr == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Project Manager not Found!"
                });
            }
            _pmContext.ProjectManagerModels.Remove(projectMgr);
            await _pmContext.SaveChangesAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "Project Manager deleted successfully"
            });
        }
    }
}
