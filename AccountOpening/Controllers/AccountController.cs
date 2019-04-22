using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AccountOpening.Entities;
using AccountOpening.Helpers;
using Commons.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AccountOpening.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [EnableCors("AccessAgencyBankingCorsPolicy")]
    [Produces("application/json")]
    [Route("v1/account")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountOpeningRepository _orclRepo;

        public AccountController(ILogger<AccountController> logger, IAccountOpeningRepository orclRepo)
        {
            _logger = logger;
            _orclRepo = orclRepo;
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(AccountOpeningResponse), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        [ProducesResponseType(typeof(Response), 500)]
        public async Task<IActionResult> create([FromBody] AccountOpeningRequest request, bool customerOnly = false)
        {
            AccountOpeningResponse aor = new AccountOpeningResponse();
            List<string> messages = new List<string>();
            ExecuteCustomer e = new ExecuteCustomer()
            {
                BranchCode = request.BRANCH_CODE,
                BranchNode = "DRCONGDB",
                UserId = "SYSTEM",
                SourceCode = "CUST_UPD"
            };

            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(Commons.Helpers.Utility.GetResponse(ModelState));

                if (!string.IsNullOrEmpty(request.CUSTOMER_NO))
                {
                    bool customerExists = await _orclRepo.CustomerExist(request.CUSTOMER_NO);

                    if (customerExists)
                    {
                        aor = await CreateAccount(request, e, request.CUSTOMER_NO);
                    }
                }
                else
                {
                    var tuple = await CreateCustomer(request, e);

                    if (tuple.Item1)
                    {
                        if (customerOnly)
                            return CreatedAtAction("create", await _orclRepo.GetAccountOpeningResponseCustomerOnly(tuple.Item2.MAINTENANCE_SEQ_NO));

                        aor = await CreateAccount(request, e, tuple.Item2.MAINTENANCE_SEQ_NO);
                    }
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.ToString(), a);
                return StatusCode((int)HttpStatusCode.InternalServerError, Commons.Helpers.Utility.GetResponse(ex));
            }

            return CreatedAtAction("create", aor);
        }

        private async Task<AccountOpeningResponse> CreateAccount(AccountOpeningRequest request, ExecuteCustomer e, string param)
        {
            bool isAccountAdded = false;
            bool isAccountExecuted = false;

            Account u = new Account();
            AccountOpeningRequest req = null;
            AccountOpeningResponse res = new AccountOpeningResponse();

            try
            {

                //Get Account
                if (!string.IsNullOrEmpty(request.CUSTOMER_NO))
                {
                    req = await _orclRepo.GetCustomerByNumber(param, request.ACCOUNT_CLASS);
                }
                else
                {
                    req = await _orclRepo.GetCustomer(param, request.ACCOUNT_CLASS);
                }

                //Remove Debugger on go live
                _logger.LogInformation($"MAINTENANCE_SEQ_NO : {param} ACCOUNT_CLASS " +
                    $": {request.ACCOUNT_CLASS} CustomerNo: {req.CUSTOMER_NO} CR_HO_LINE: {req.CR_HO_LINE} " +
                    $"DR_HO_LINE: {req.DR_HO_LINE}");

                request.CUSTOMER_NO = req?.CUSTOMER_NO;
                request.DR_CB_LINE = req?.DR_CB_LINE;
                request.CR_CB_LINE = req?.CR_CB_LINE;
                request.CR_HO_LINE = req?.CR_HO_LINE;
                request.DR_HO_LINE = req?.DR_HO_LINE;
                request.DR_GL = req?.DR_GL;
                request.CR_GL = req?.CR_GL;

                e.SourceCode = "CUSTACC_UPLOAD";

                u = Utility.GetAccount(request);
                _logger.LogInformation("requested upload account");

                //InsertAccount Detail
                isAccountAdded = await _orclRepo.AddAccount(u);
                _logger.LogInformation($"requested add account with output: {isAccountAdded}");

                //Execute New Customer
                isAccountExecuted = await _orclRepo.ExecuteNewAccount(e);
                _logger.LogInformation($"requested execute customer with output: {isAccountExecuted}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }

            if (isAccountAdded && isAccountExecuted)
            {
                try
                {
                    res = await _orclRepo.GetAccountOpeningResponse(u.MAINTENANCE_SEQ_NO);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                }
            }

            return res;
        }

        private async Task<Tuple<bool, Customer>> CreateCustomer(AccountOpeningRequest request, ExecuteCustomer e)
        {
            Customer c = new Customer();
            Personal p = new Personal();

            bool isCustomerAdded = false;
            bool isCustomerExecuted = false;

            try
            {
                //Get Customer
                c = Utility.GetCustomer(request);
                _logger.LogInformation("requested upload customer");

                //Get Personal
                p = Utility.GetPersonal(request);
                _logger.LogInformation("requested upload personal");


                //Insert Customer Details
                isCustomerAdded = await _orclRepo.AddCustomer(p, c);
                _logger.LogInformation($"requested add customer with output: {isCustomerAdded}");

                //Execute New Customer

                isCustomerExecuted = await _orclRepo.ExecuteNewCustomer(e);
                _logger.LogInformation($"requested execute customer with output: {isCustomerExecuted}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }

            return new Tuple<bool, Customer>(isCustomerAdded && isCustomerExecuted, c);
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