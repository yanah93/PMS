using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.DTOs;
using PMS.Model;
using System.Collections.Generic;

namespace PMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly PMScontext _projectContext;
        private readonly IMapper _mapper;
        public ProjectController(PMScontext projectContext, IMapper mapper)
        {
            this._projectContext = projectContext;
            this._mapper = mapper;

        }

        [HttpGet]
        public async Task<ActionResult<ProjectDto>> GetAllProjects()
        {
            var projectList = await _projectContext.ProjectModels.ToListAsync();
            var mappedProjectList = _mapper.Map<List<ProjectDto>>(projectList);
            return Ok(new
            {
                StatusCode = 200,
                Message = "Successfully displayed all projects",
                Result = mappedProjectList
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDto>> GetProjectById(int id)
        {
            var project = await _projectContext.ProjectModels.FindAsync(id);
            if (project == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Project does not exist"
                });
            }
            return Ok(new
            {
                StatusCode = 200,
                Message = "Successfully found Project ID",
                Result = project
            });
        }

        [HttpPost("add")]
        public async Task<ActionResult<ProjectDto>> AddProject([FromBody] ProjectDto projectDtoObj)
        {
            if (projectDtoObj == null)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Please input data to add!"
                });
            }
            var mappedProject = _mapper.Map<ProjectModel>(projectDtoObj);
            _projectContext.ProjectModels.Add(mappedProject);
            await _projectContext.SaveChangesAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "Successfully added Project"
            });
        }

        [HttpPut("update")]
        public async Task<ActionResult<ProjectDto>> UpdateProject([FromBody] ProjectDto projectDtoObj)
        {
            if (projectDtoObj == null)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Please send data to update!"
                });
            }
            var isProjectExist = await _projectContext.ProjectModels.AsNoTracking().FirstOrDefaultAsync(a => a.Id == projectDtoObj.Id);
            if (isProjectExist == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Project not Found"
                });
            }
            _projectContext.Entry(projectDtoObj).State = EntityState.Modified;
            await _projectContext.SaveChangesAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "Project updated!"
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ProjectDto>> DeleteProject(int id)
        {
            var project = await _projectContext.ProjectModels.FindAsync(id);
            if(project == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Project not Found!"
                });
            }
            _projectContext.ProjectModels.Remove(project);  
            await _projectContext.SaveChangesAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "Project deleted successfully"
            });
        }
    }
}
