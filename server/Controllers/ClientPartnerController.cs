using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Model;

namespace PMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientPartnerController : ControllerBase
    {
        private readonly PMScontext _pmsContext;
        public ClientPartnerController(PMScontext pmsContext)
        {
            this._pmsContext = pmsContext;
        }

        [HttpGet]
        public async Task<ActionResult<ClientPartnerModel>> GetAllClients()
        {
            var clientList = await _pmsContext.ClientPartnerModels.ToListAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "Successfully displayed all client partners",
                Result = clientList
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClientPartnerModel>> GetClientById(int id)
        {
            var client = await _pmsContext.ClientPartnerModels.FindAsync(id);
            if (client == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Client does not exist"
                });
            }
            return Ok(new
            {
                StatusCode = 200,
                Message = "Successfully found client partner",
                Result = client
            });
        }

        [HttpPost("add")]
        public async Task<ActionResult<ClientPartnerModel>> AddClient([FromBody] ClientPartnerModel clientObj)
        {
            if (clientObj == null)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Please input data to add!"
                });
            }
            var client = await _pmsContext.ClientPartnerModels.AddAsync(clientObj);
            await _pmsContext.SaveChangesAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "Successfully added client"
            });
        }

        [HttpPut("update")]
        public async Task<ActionResult<ClientPartnerModel>> UpdateClient([FromBody] ClientPartnerModel clientObj)
        {
            if (clientObj == null)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Please send data to update!"
                });
            }
            var isClientExist = await _pmsContext.ClientPartnerModels.AsNoTracking().FirstOrDefaultAsync(a => a.Id == clientObj.Id);
            if (isClientExist == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Client not Found"
                });
            }
            _pmsContext.Entry(clientObj).State = EntityState.Modified;
            await _pmsContext.SaveChangesAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "Client details updated!"
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ClientPartnerModel>> DeleteClient(int id)
        {
            var client = await _pmsContext.ClientPartnerModels.FindAsync(id);
            if (client == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Client not Found!"
                });
            }
            _pmsContext.ClientPartnerModels.Remove(client);
            await _pmsContext.SaveChangesAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "Client deleted successfully"
            });
        }
    }
}
