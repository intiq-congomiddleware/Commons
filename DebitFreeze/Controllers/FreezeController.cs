using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Commons.Entities;
using DebitFreeze.Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DebitFreeze.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [EnableCors("AccessAgencyBankingCorsPolicy")]
    [Produces("application/json")]
    [Route("v1/debit")]
    [ApiController]
    public class FreezeController : Controller
    {

        private readonly ILogger<FreezeController> _logger;
        private readonly IDebitFreezeRepository _orclRepo;
        private readonly AppSettings _appSettings;
        public FreezeController(ILogger<FreezeController> logger, IDebitFreezeRepository orclRepo
          , IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _orclRepo = orclRepo;
            _appSettings = appSettings.Value;
        }

        [HttpPost("freeze")]
        [ProducesResponseType(typeof(Response), 201)]
        [ProducesResponseType(typeof(Response), 400)]
        [ProducesResponseType(typeof(Response), 500)]
        public async Task<IActionResult> freeze([FromBody] DebitFreezeRequest request)
        {
            Response r = new Response();

            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(Commons.Helpers.Utility.GetResponse(ModelState));

                r = await _orclRepo.FreezeAccount(request.accountNumber);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                _logger.LogError($"{request.accountNumber} ::- {ex.ToString()}");
                return StatusCode((int)HttpStatusCode.InternalServerError, Commons.Helpers.Utility.GetResponse(ex));
            }

            return CreatedAtAction("freeze", r);
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