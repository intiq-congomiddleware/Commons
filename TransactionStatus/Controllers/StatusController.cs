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
using TransactionStatus.Entities;

namespace TransactionStatus.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [EnableCors("AccessAgencyBankingCorsPolicy")]
    [Produces("application/json")]
    [Route("v1/status")]
    [ApiController]
    public class StatusController : Controller
    {
        private readonly ILogger<StatusController> _logger;
        private readonly ITransactionStatusRepository _orclRepo;
        private readonly AppSettings _appSettings;

        public StatusController(ILogger<StatusController> logger, ITransactionStatusRepository orclRepo
            , IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _orclRepo = orclRepo;
            _appSettings = appSettings.Value;
        }

        [HttpPost("check")]
        [ProducesResponseType(typeof(FundsTransferResponse), 201)]
        [ProducesResponseType(typeof(Response), 400)]
        [ProducesResponseType(typeof(Response), 500)]
        public async Task<IActionResult> check([FromBody] StatusRequest request)
        {
            FundsTransferResponse r = new FundsTransferResponse();

            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(Commons.Helpers.Utility.GetResponse(ModelState));
           
                r = await _orclRepo.ValidateTransactionByRef(request.transactionRef);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
               _logger.LogError($"{request.transactionRef} ::- {ex.ToString()}");
                return StatusCode((int)HttpStatusCode.InternalServerError, Commons.Helpers.Utility.GetResponse(ex));
            }

            return CreatedAtAction("check", r);
        }

        //[HttpGet("encdata/{value}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        [ProducesResponseType(typeof(Response), 500)]
        public async Task<IActionResult> encdata(string value)
        {
            return Ok(_orclRepo.EncData(value));
        }
    }
}