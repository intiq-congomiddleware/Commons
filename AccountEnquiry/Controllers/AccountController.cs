using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AccountEnquiry.Entities;
using Commons.Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AccountEnquiry.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [EnableCors("AccessAgencyBankingCorsPolicy")]
    [Produces("application/json")]
    [Route("v1/account")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountEnquiryRepository _orclRepo;

        public AccountController(ILogger<AccountController> logger, IAccountEnquiryRepository orclRepo)
        {
            _logger = logger;
            _orclRepo = orclRepo;
        }

        [HttpPost("enquirybyaccountno")]
        [ProducesResponseType(typeof(AccountEnquiryResponse), 201)]
        [ProducesResponseType(typeof(Response), 400)]
        [ProducesResponseType(typeof(Response), 500)]
        public async Task<IActionResult> enquirybyaccountno([FromBody] AccountEnquiryRequest request)
        {
            AccountEnquiryResponse b = new AccountEnquiryResponse();
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(Commons.Helpers.Utility.GetResponse(ModelState));


                b = await _orclRepo.GetAccountEnquiryByAccountNumber(request);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{request.accountNumber}:- {Environment.NewLine} {ex.ToString()}");
                return StatusCode((int)HttpStatusCode.InternalServerError, Commons.Helpers.Utility.GetResponse(ex));
            }

            return CreatedAtAction("enquirybyaccountno", b);
        }

        [HttpPost("enquirybycustomerno")]
        [ProducesResponseType(typeof(List<AccountEnquiryResponse>), 201)]
        [ProducesResponseType(typeof(Response), 400)]
        [ProducesResponseType(typeof(Response), 500)]
        public async Task<IActionResult> enquirybycustomerno([FromBody] CustomerEnquiryRequest request)
        {
            List<AccountEnquiryResponse> b = new List<AccountEnquiryResponse>();
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(Commons.Helpers.Utility.GetResponse(ModelState));


                b = await _orclRepo.GetAccountEnquiryByCustomerNumber(request);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{request.customerNumber}:- {Environment.NewLine} {ex.ToString()}");
                return StatusCode((int)HttpStatusCode.InternalServerError, Commons.Helpers.Utility.GetResponse(ex));
            }

            return CreatedAtAction("enquirybycustomerno", b);
        }

        [HttpPost("enquirybyphoneno")]
        [ProducesResponseType(typeof(List<AccountEnquiryResponse>), 201)]
        [ProducesResponseType(typeof(Response), 400)]
        [ProducesResponseType(typeof(Response), 500)]
        public async Task<IActionResult> enquirybyphoneno([FromBody] PhoneEnquiryRequest request)
        {
            List<AccountEnquiryResponse> b = new List<AccountEnquiryResponse>();
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(Commons.Helpers.Utility.GetResponse(ModelState));


                b = await _orclRepo.GetAccountEnquiryByPhoneNumber(request);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{request.phoneNumber}:- {Environment.NewLine} {ex.ToString()}");
                return StatusCode((int)HttpStatusCode.InternalServerError, Commons.Helpers.Utility.GetResponse(ex));
            }

            return CreatedAtAction("enquirybyphoneno", b);
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