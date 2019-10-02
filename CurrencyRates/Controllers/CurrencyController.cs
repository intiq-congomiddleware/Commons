using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Commons.Entities;
using CurrencyRates.Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CurrencyRates.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [EnableCors("AccessAgencyBankingCorsPolicy")]
    [Produces("application/json")]
    [Route("v1/currency")]
    [ApiController]
    public class CurrencyController : Controller
    {
        private readonly ILogger<CurrencyController> _logger;
        private readonly ICurrencyRepository _orclRepo;

        public CurrencyController(ILogger<CurrencyController> logger, ICurrencyRepository orclRepo)
        {
            _logger = logger;
            _orclRepo = orclRepo;
        }

        [HttpPost("rates")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        [ProducesResponseType(typeof(Response), 500)]
        public async Task<IActionResult> rates([FromBody] CurrencyRequest request)
        {
            CurrencyResponse cr = new CurrencyResponse();
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(Commons.Helpers.Utility.GetResponse(ModelState));

                _logger.LogInformation($"request ID:{request.requestId} {Environment.NewLine} User ID:{request.userName}");

                cr = await _orclRepo.GetRates();
            }
            catch (Exception ex)
            {
                _logger.LogError($"request ID:{request.requestId} User ID:{request.userName}:- {Environment.NewLine} {ex.ToString()}");
                return StatusCode((int)HttpStatusCode.InternalServerError, Commons.Helpers.Utility.GetResponse(ex));
            }

            return CreatedAtAction("rates", cr);
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