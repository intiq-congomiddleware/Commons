using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AccountOpening.Entities;
using AccountOpening.Helpers;
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
    [Route("v1")]
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
        [ProducesResponseType(typeof(AccountOpeningResponse), 400)]
        [ProducesResponseType(typeof(AccountOpeningResponse), 500)]
        public async Task<IActionResult> create([FromBody] Customer request)
        {
            AccountOpeningResponse a = new AccountOpeningResponse();
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
                    return BadRequest(Utility.GetResponse(ModelState));

                bool customerExists = !string.IsNullOrEmpty(request.CUSTOMER_NO)
                    && _orclRepo.CustomerExist(request.CUSTOMER_NO);

                if (customerExists)
                {
                    a.status = CreateAccount(request, e);
                }
                else
                {
                    a.status = CreateCustomer(request, e) && CreateAccount(request, e);
                }

                messages.Add((a.status) ? "Account Created Successfully" : "Account Creation Failed");
                a.message = messages;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), a);
                return StatusCode((int)HttpStatusCode.InternalServerError, Utility.GetResponse(ex));
            }

            return CreatedAtAction("create", a);
        }

        private bool CreateAccount(Customer request, ExecuteCustomer e)
        {
            bool isAccountAdded = false;
            bool isAccountExecuted = false;

            UploadAccount u = new UploadAccount();

            try
            {
                //Get UploadAccount
                u = Utility.GetAccountUpload(request);
                _logger.LogInformation("requested upload account");

                //InsertAccount Detail
                isAccountAdded = _orclRepo.AddAccount(u);
                _logger.LogInformation($"requested add account with output: {isAccountAdded}");

                //Execute New Customer
                isAccountExecuted = _orclRepo.ExecuteNewAccount(e);
                _logger.LogInformation($"requested execute customer with output: {isAccountExecuted}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }

            return isAccountAdded && isAccountExecuted;
        }

        private bool CreateCustomer(Customer request, ExecuteCustomer e)
        {
            UploadCustomer c = new UploadCustomer();
            UploadPersonal p = new UploadPersonal();

            bool isCustomerAdded = false;
            bool isCustomerExecuted = false;

            try
            {
                //Get UploadCustomer
                c = Utility.GetUploadCustomer(request);
                _logger.LogInformation("requested upload customer");

                //Get UploadPersonal
                p = Utility.GetUploadPersonal(request);
                _logger.LogInformation("requested upload personal");


                //Insert Customer Details
                isCustomerAdded = _orclRepo.AddCustomer(p, c);
                _logger.LogInformation($"requested add customer with output: {isCustomerAdded}");

                //Execute New Customer

                isCustomerExecuted = _orclRepo.ExecuteNewCustomer(e);
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