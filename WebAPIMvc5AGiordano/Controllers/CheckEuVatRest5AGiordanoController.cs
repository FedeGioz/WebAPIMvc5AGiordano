using LibAPI5AGiordano;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.PortableExecutable;
using System.Text.Json;

namespace WebAPIMvc5AGiordano.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CheckEuVatRest5AGiordanoController : ControllerBase
    {
        [HttpPost]
        public async Task<string> CheckEuVatRest5AGiordano([FromHeader(Name = "DevName")] string devName, [FromHeader(Name = "Team")] string team, [FromBody] Customer customer)
        {
            var customerService = new Customer();


            string isoCodeResult = await customerService.GetISOCode5AGiordano(customer);

            customer.CountryCode = isoCodeResult;

            string vatCheckResult = await customerService.CheckEuVat5AGiordano(customer);

            var vatCheckJson = JsonDocument.Parse(vatCheckResult).RootElement;
            var isoCodeJson = JsonDocument.Parse(isoCodeResult).RootElement;

            var result = new
            {
                name = vatCheckJson.GetProperty("name").GetString(),
                address = vatCheckJson.GetProperty("address").GetString(),
                isocode = isoCodeJson.GetProperty("isocode").GetString(),
                devname = devName,
                team = team
            };

            string jsonResponse = JsonSerializer.Serialize(result);

            return jsonResponse;
        }
    }
}
