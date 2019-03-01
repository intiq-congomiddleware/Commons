using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BalanceEnquiry.Entities;
using BalanceEnquiry.Helpers;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BalanceEnquiry.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [EnableCors("AccessAgencyBankingCorsPolicy")]
    [Produces("application/json")]
    [Route("v1")]
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

        [HttpPost("Enquiry")]
        [ProducesResponseType(typeof(BalanceEnquiryResponse), 200)]
        [ProducesResponseType(typeof(BalanceEnquiryResponse), 400)]
        [ProducesResponseType(typeof(BalanceEnquiryResponse), 500)]
        public async Task<IActionResult> Enquiry([FromBody] BalanceEnquiryRequest request)
        {
            BalanceEnquiryResponse b = new BalanceEnquiryResponse();
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(Utility.GetResponse(ModelState));

                request.userId = "SYSTEM";

                b = _orclRepo.GetBalanceEnquiry(request);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.ToString()} :  {b}");
                return StatusCode((int)HttpStatusCode.InternalServerError, Utility.GetResponse(ex));
            }

            return CreatedAtAction("Enquiry", b);
        }
    }
}