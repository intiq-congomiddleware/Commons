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

            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(Commons.Helpers.Utility.GetResponse(ModelState));

                request.trnrefno = $"{request.branch_code}{request.product}{request.l_acs_ccy}" +
                    $"{Commons.Helpers.Utility.RandomString(6)}";
                request.trans_type = 1;

                //if (Utility.Authorization(request.authorization, _authSettings))
                //{
                //    if (Utility.FraudCheck(request))
                //    {

                string sproc = (request.with_charges) ? _appSettings.ChrgsSproc : _appSettings.PaytSproc;

                resp = await _orclRepo.ExecuteTransaction(request, sproc);

                if (resp.status.Trim() == "Y")
                {
                    resp.id = request.trnrefno;
                    resp.trnrefno = request.trnrefno;
                }

                //}
                //    }
                //    else
                //    {
                //        resp.status = "34";
                //        resp.message = "Suspicious Transaction";
                //    }
                //}
                //else
                //{
                //    _logger.LogInformation($"payment Request : {JsonHelper.toJson(request)} | Header authorization failed");
                //    resp.status = "96";
                //    resp.message = "Unauthorized";
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError($"{request.cract} : {request.dract}:- {ex.ToString()}");
                return StatusCode((int)HttpStatusCode.InternalServerError, Commons.Helpers.Utility.GetResponse(ex));
            }

            return CreatedAtAction("transfer", resp);
        }
    }
}