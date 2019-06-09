using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Commons.Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StatementGeneration.Entities;

namespace StatementGeneration.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [EnableCors("AccessAgencyBankingCorsPolicy")]
    [Produces("application/json")]
    [Route("v1/generate")]
    [ApiController]
    public class StatementController : Controller
    {
        
        private readonly ILogger<StatementController> _logger;
        private readonly IStatementRepository _orclRepo;
        private readonly AppSettings _appSettings;

        public StatementController(ILogger<StatementController> logger, IStatementRepository orclRepo
           , IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _orclRepo = orclRepo;
            _appSettings = appSettings.Value;
        }

        [HttpPost("statement")]
        [ProducesResponseType(typeof(List<StatementResponse>), 201)]
        [ProducesResponseType(typeof(Response), 400)]
        [ProducesResponseType(typeof(Response), 500)]
        public async Task<IActionResult> statement([FromBody] StatementRequest request)
        {
            List<StatementResponse> r = new List<StatementResponse>();

            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(Commons.Helpers.Utility.GetResponse(ModelState));             

                var gs = await _orclRepo.GenerateStatement(request);

                if (gs != null)
                {
                    r = await _orclRepo.FilterStatement(request);
                    if (request.noOfRecords > 0)
                    {
                        r = r.Take(request.noOfRecords).ToList();
                    }
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.ExpectationFailed, 
                        new Response() { message = "Staement Generation Failed", status = false });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                _logger.LogError($"{request.accountNumber} ::- {ex.ToString()}");
                return StatusCode((int)HttpStatusCode.InternalServerError, Commons.Helpers.Utility.GetResponse(ex));
            }

            return CreatedAtAction("statement", r);
        }

        [HttpGet("encdata/{value}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        [ProducesResponseType(typeof(Response), 500)]
        public async Task<IActionResult> encdata(string value)
        {
            return Ok(_orclRepo.EncData(value));
        }
    }
}