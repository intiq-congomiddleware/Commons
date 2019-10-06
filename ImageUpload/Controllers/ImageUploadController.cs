using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Commons.Entities;
using ImageUpload.Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ImageUpload.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [EnableCors("AccessAgencyBankingCorsPolicy")]
    [Produces("application/json")]
    [Route("v1/image")]
    [ApiController]
    public class ImageUploadController : Controller
    {
        private readonly ILogger<ImageUploadController> _logger;
        private readonly IImageUploadRepository _orclRepo;

        public ImageUploadController(ILogger<ImageUploadController> logger, IImageUploadRepository orclRepo)
        {
            _logger = logger;
            _orclRepo = orclRepo;
        }

        [HttpPost("upload")]
        [ProducesResponseType(typeof(Response), 201)]
        [ProducesResponseType(typeof(Response), 400)]
        [ProducesResponseType(typeof(Response), 500)]
        public async Task<IActionResult> upload([FromBody] ImageUploadRequest request)
        {
            ImageUploadResponse cr = new ImageUploadResponse();
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(Commons.Helpers.Utility.GetResponse(ModelState));

                _logger.LogInformation($"request ID:{request.requestId} {Environment.NewLine} User ID:{request.userName}");

                cr = await _orclRepo.UploadImage(request);
            }
            catch (Exception ex)
            {
                _logger.LogError($"request ID:{request.requestId} User ID:{request.userName}:- {Environment.NewLine} {ex.ToString()}");
                return StatusCode((int)HttpStatusCode.InternalServerError, Commons.Helpers.Utility.GetResponse(ex));
            }

            return CreatedAtAction("upload", cr);
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