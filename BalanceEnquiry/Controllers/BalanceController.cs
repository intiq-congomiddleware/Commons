using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BalanceEnquiry.Entities;
using BalanceEnquiry.Helpers;
using Commons.Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BalanceEnquiry.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [EnableCors("AccessAgencyBankingCorsPolicy")]
    [Produces("application/json")]
    [Route("v1/balance")]
    [ApiController]
    public class BalanceController : Controller
    {
        private readonly ILogger<BalanceController> _logger;
        private readonly IBalanceEnquiryRepository _orclRepo;

        public BalanceController(ILogger<BalanceController> logger, IBalanceEnquiryRepository orclRepo)
        {
            _logger = logger;
            _orclRepo = orclRepo;
        }

        [HttpPost("enquiry")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        [ProducesResponseType(typeof(Response), 500)]
        public async Task<IActionResult> enquiry([FromBody] BalanceEnquiryRequest request)
        {
            BalanceEnquiryResponse b = new BalanceEnquiryResponse();
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(Commons.Helpers.Utility.GetResponse(ModelState));

                request.userId = "SYSTEM";

                b = await _orclRepo.GetBalanceEnquiry(request);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{request.accountNumber}:- {Environment.NewLine} {ex.ToString()}");
                return StatusCode((int)HttpStatusCode.InternalServerError, Commons.Helpers.Utility.GetResponse(ex));
            }

            return CreatedAtAction("enquiry", b);
        }
    }
}