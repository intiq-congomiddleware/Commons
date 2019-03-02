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
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        [ProducesResponseType(typeof(Response), 500)]
        public async Task<IActionResult> create([FromBody] AccountOpeningRequest request)
        {
            Response a = new Response();
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

                bool customerExists = !string.IsNullOrEmpty(request.CUSTOMER_NO)
                    && await _orclRepo.CustomerExist(request.CUSTOMER_NO);

                if (customerExists)
                {
                    a.status = await CreateAccount(request, e);
                }
                else
                {
                    a.status = await CreateCustomer(request, e) && await CreateAccount(request, e);
                }

                a.message = (a.status) ? "Account Created Successfully" : "Account Creation Failed";
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.ToString(), a);
                return StatusCode((int)HttpStatusCode.InternalServerError, Commons.Helpers.Utility.GetResponse(ex));
            }

            return CreatedAtAction("create", a);
        }

        private async Task<bool> CreateAccount(AccountOpeningRequest request, ExecuteCustomer e)
        {
            bool isAccountAdded = false;
            bool isAccountExecuted = false;

            Account u = new Account();

            try
            {
                //Get Account
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

            return isAccountAdded && isAccountExecuted;
        }

        private async Task<bool> CreateCustomer(AccountOpeningRequest request, ExecuteCustomer e)
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

            return isCustomerAdded && isCustomerExecuted;
        }
    }
}