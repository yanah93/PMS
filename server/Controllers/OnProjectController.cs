using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Model;

namespace PMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OnProjectController : ControllerBase
    {
        private readonly PMScontext _pmsContext;
        public OnProjectController(PMScontext pmsContext)
        {
            this._pmsContext = pmsContext;
        }

        [HttpGet]
        public async Task<ActionResult<OnProjectModel>> GetAllOnProjects()
        {
            var onProjectList = await _pmsContext.OnProjectModels.ToListAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "Successfully displayed all projects",
                Result = onProjectList
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OnProjectModel>> GetOnProjectById(int id)
        {
            var onProject = await _pmsContext.OnProjectModels.FindAsync(id);
            if (onProject == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "OnProject does not exist"
                });
            }
            return Ok(new
            {
                StatusCode = 200,
                Message = "Successfully found OnProject ID",
                Result = onProject
            });
        }

        [HttpPost("add")]
        public async Task<ActionResult<OnProjectModel>> AddOnProject([FromBody] OnProjectModel onProjectObj)
        {
            if (onProjectObj == null)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Please input data to add!"
                });
            }
            var project = await _pmsContext.OnProjectModels.AddAsync(onProjectObj);
            await _pmsContext.SaveChangesAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "Successfully added OnProject"
            });
        }

        [HttpPut("update")]
        public async Task<ActionResult<OnProjectModel>> UpdateOnProject([FromBody] OnProjectModel onProjectObj)
        {
            if (onProjectObj == null)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Please send data to update!"
                });
            }
            var isOnProjectExist = await _pmsContext.OnProjectModels.AsNoTracking().FirstOrDefaultAsync(a => a.Id == onProjectObj.Id);
            if (isOnProjectExist == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "OnProject not Found"
                });
            }
            _pmsContext.Entry(onProjectObj).State = EntityState.Modified;
            await _pmsContext.SaveChangesAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "OnProject updated!"
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<OnProjectModel>> DeleteOnProject(int id)
        {
            var project = await _pmsContext.OnProjectModels.FindAsync(id);
            if (project == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "OnProject not Found!"
                });
            }
            _pmsContext.OnProjectModels.Remove(project);
            await _pmsContext.SaveChangesAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "OnProject deleted successfully"
            });
        }
    }
}
