using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Commons.Entities;
using FundsTransfer.Entities;
using FundsTransfer.Helpers;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FundsTransfer.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [EnableCors("AccessAgencyBankingCorsPolicy")]
    [Produces("application/json")]
    [Route("v1/funds")]
    [ApiController]

    public class FundsTransferController : Controller
    {
        private readonly ILogger<FundsTransferController> _logger;
        private readonly IFundsTransferRepository _orclRepo;
        private readonly AuthSettings _authSettings;
        private readonly AppSettings _appSettings;

        public FundsTransferController(ILogger<FundsTransferController> logger, IFundsTransferRepository orclRepo
            , IOptions<AuthSettings> authSettings, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _orclRepo = orclRepo;
            _authSettings = authSettings.Value;
            _appSettings = appSettings.Value;
        }

        [HttpPost("transfer")]
        [ProducesResponseType(typeof(FundsTransferResponse), 201)]
        [ProducesResponseType(typeof(Response), 400)]
        [ProducesResponseType(typeof(Response), 500)]
        public async Task<IActionResult> transfer([FromBody] FundsTransferRequest request)
        {
            FundsTransferResponse resp = new FundsTransferResponse();
            AccountEnquiryResponse aresp = new AccountEnquiryResponse();

            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(Commons.Helpers.Utility.GetResponse(ModelState));

                //request.trnrefno = $"{request.branch_code}{request.product}{request.l_acs_ccy}" +
                //    $"{Commons.Helpers.Utility.RandomString(6)}";

                request.trans_type = 1;

                if (request.is_own_account && !await _orclRepo.IsOwnAccount(request))
                    return StatusCode((int)HttpStatusCode.BadRequest,
                        Commons.Helpers.Utility.GetResponse(Constant.ACCOUNT_NOT_LINKED, HttpStatusCode.BadRequest));

                if (!string.IsNullOrEmpty(request.cract) && request.cract.Length != 9)
                {
                    aresp = await _orclRepo.GetAccountEnquiryByAccountNumber(new AccountEnquiryRequest() { accountNumber = request.cract });

                    if (aresp?.ac_stat_no_cr?.ToUpper().Trim() != "N")
                    {
                        return StatusCode((int)HttpStatusCode.BadRequest,
                            Commons.Helpers.Utility.GetResponse(Constant.STAT_NO_CR, HttpStatusCode.BadRequest));
                    }
                }

                if (!string.IsNullOrEmpty(request.dract) && request.dract.Length != 9)
                {
                    aresp = await _orclRepo.GetAccountEnquiryByAccountNumber(new AccountEnquiryRequest() { accountNumber = request.dract });

                    if (aresp?.ac_stat_no_dr?.ToUpper().Trim() != "N")
                    {
                        return StatusCode((int)HttpStatusCode.BadRequest,
                            Commons.Helpers.Utility.GetResponse(Constant.STAT_NO_DR, HttpStatusCode.BadRequest));
                    }

                    if (aresp?.ac_stat_dormant?.ToUpper().Trim() != "N")
                    {
                        return StatusCode((int)HttpStatusCode.BadRequest,
                            Commons.Helpers.Utility.GetResponse(Constant.STAT_DORMANT, HttpStatusCode.BadRequest));
                    }
                }

                string sproc = (request.with_charges) ? _appSettings.ChrgsSproc : _appSettings.PaytSproc;

                resp = await _orclRepo.ExecuteTransaction(request, sproc);

                if (resp.status?.ToUpper().Trim() == "Y")
                {
                    resp.id = request.trnrefno;
                    resp.trnrefno = request.trnrefno;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{request.cract} : {request.dract}:- {ex.ToString()}");
                return StatusCode((int)HttpStatusCode.InternalServerError, Commons.Helpers.Utility.GetResponse(ex));
            }

            return CreatedAtAction("transfer", resp);
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