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
        private readonly PMScontext _pmsContext;
        public ProjectManagerController(PMScontext pmsContext)
        {
            this._pmsContext = pmsContext;
        }

        [HttpGet]
        public async Task<ActionResult<ProjectManagerModel>> GetAllProjectManagers()
        {
            var projectManagerList = await _pmsContext.ProjectManagerModels.ToListAsync();
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
            var projectManagers = await _pmsContext.ProjectManagerModels.FindAsync(id);
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
            var projectMgr = await _pmsContext.ProjectManagerModels.AddAsync(projectMgrObj);
            await _pmsContext.SaveChangesAsync();
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
            var isPMExist = await _pmsContext.ProjectManagerModels.AsNoTracking().FirstOrDefaultAsync(a => a.Id == projectMgrObj.Id);
            if (isPMExist == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "PM not Found"
                });
            }
            _pmsContext.Entry(projectMgrObj).State = EntityState.Modified;
            await _pmsContext.SaveChangesAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "Project manager updated!"
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ProjectManagerModel>> DeleteProjectMgr(int id)
        {
            var projectMgr = await _pmsContext.ProjectManagerModels.FindAsync(id);
            if (projectMgr == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Project Manager not Found!"
                });
            }
            _pmsContext.ProjectManagerModels.Remove(projectMgr);
            await _pmsContext.SaveChangesAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "Project Manager deleted successfully"
            });
        }
    }
}
