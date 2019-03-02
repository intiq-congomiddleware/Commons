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

        public FundsTransferController(ILogger<FundsTransferController> logger, IFundsTransferRepository orclRepo)
        {
            _logger = logger;
            _orclRepo = orclRepo;
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

                TransLog tLog = Utility.GetTransLog(request);

                if (await _orclRepo.ValidateTransactionByRef(tLog))
                {
                    resp = await _orclRepo.ExecuteTransaction(request);
                }

                if (resp.response_code.Trim() == "Y")
                {
                    resp.id = request.trnrefno;
                    resp.trnrefno = request.trnrefno;
                }

                await _orclRepo.UpdateTransactionResponse(resp);
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