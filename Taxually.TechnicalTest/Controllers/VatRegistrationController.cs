using Microsoft.AspNetCore.Mvc;
using Taxually.TechnicalTest.Exceptions;
using Taxually.TechnicalTest.Factories;
using Taxually.TechnicalTest.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Taxually.TechnicalTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VatRegistrationController(CountryFactory countryFactory, ILogger<VatRegistrationController> logger) : ControllerBase
    {
        /// <summary>
        /// Registers a company for a VAT number in a given country
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] VatRegistrationRequest request)
        {
            try
            {
                logger.LogInformation($"Request handled: country: \"{request.Country}\", company name: \"{request.CompanyName}\", company id: \"{request.CompanyId}\"");
                await countryFactory.HandleAsync(request);
            }
            catch (CountryNotSupportedException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }
    }
}
