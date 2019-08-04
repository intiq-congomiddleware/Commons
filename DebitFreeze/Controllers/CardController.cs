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
    [Route("v1/card")]
    [ApiController]
    public class CardController : Controller
    {
        private readonly ILogger<CardController> _logger;
        private readonly IDebitFreezeRepository _orclRepo;
        private readonly AppSettings _appSettings;
        public CardController(ILogger<CardController> logger, IDebitFreezeRepository orclRepo
          , IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _orclRepo = orclRepo;
            _appSettings = appSettings.Value;
        }

        [HttpPost("block")]
        [ProducesResponseType(typeof(BlockAccountResponse), 201)]
        [ProducesResponseType(typeof(BlockAccountResponse), 400)]
        [ProducesResponseType(typeof(BlockAccountResponse), 500)]
        public async Task<IActionResult> block([FromBody] DebitFreezeRequest request)
        {
            BlockAccountResponse r = new BlockAccountResponse();

            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(Commons.Helpers.Utility.GetResponse(ModelState));

                r = await _orclRepo.BlockCard(request.accountNumber);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                _logger.LogError($"{request.accountNumber} ::- {ex.ToString()}");
                return StatusCode((int)HttpStatusCode.InternalServerError, Commons.Helpers.Utility.GetResponse(ex));
            }

            return CreatedAtAction("block", r);
        }
    }
}