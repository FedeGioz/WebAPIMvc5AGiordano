using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json;
using System.Xml.Linq;
using Microsoft.Extensions.Logging;

namespace LibAPI5AGiordano
{
    public class Customer
    {
        private static readonly ILogger<Customer> _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<Customer>();

        [StringLength(3)]
        [RegularExpression("^[A-Z]+$", ErrorMessage = "CountryCode must contain only uppercase letters.")]
        public string CountryCode { get; set; }

        [StringLength(13)]
        [RegularExpression("^[A-Z0-9]+$", ErrorMessage = "VatNumber must contain only uppercase letters and digits.")]
        public string VatNumber { get; set; }

        [StringLength(50)]
        [RegularExpression("^[A-Z0-9]+$", ErrorMessage = "CountryDescription must contain only uppercase letters and digits.")]
        public string CountryDescription { get; set; }

        [Required]
        [StringLength(30)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateRequest { get; set; }

        public async Task<string> CheckEuVat5AGiordano(Customer customer)
        {
            try
            {
                string isoCodeResult = await GetISOCode5AGiordano(customer);
                var isoCodeJson = JsonDocument.Parse(isoCodeResult).RootElement;
                string isoCode = isoCodeJson.GetProperty("isocode").GetString();

                customer.CountryCode = isoCode;

                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, $"https://ec.europa.eu/taxation_customs/vies/rest-api/ms/{customer.CountryCode}/vat/{customer.VatNumber}");
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                using (JsonDocument doc = JsonDocument.Parse(responseBody))
                {
                    JsonElement root = doc.RootElement;
                    string name = root.GetProperty("name").GetString();
                    string address = root.GetProperty("address").GetString();

                    var result = new
                    {
                        name,
                        address,
                        isocode = isoCode
                    };

                    string jsonResponse = JsonSerializer.Serialize(result);

                    return jsonResponse;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while checking EU VAT.");
                throw;
            }
        }

        public async Task<string> GetISOCode5AGiordano(Customer customer)
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "http://webservices.oorsprong.org/websamples.countryinfo/CountryInfoService.wso");
                request.Headers.Add("Host", "webservices.oorsprong.org");
                var content = new StringContent($@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
  <soap:Body>
    <CountryISOCode xmlns=""http://www.oorsprong.org/websamples.countryinfo"">
      <sCountryName>{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(customer.CountryDescription.ToLower())}</sCountryName>
    </CountryISOCode>
  </soap:Body>
</soap:Envelope>", System.Text.Encoding.UTF8, "text/xml");
                request.Content = content;
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                XDocument doc = XDocument.Parse(responseBody);
                XNamespace ns = "http://www.oorsprong.org/websamples.countryinfo";
                var isoCode = doc.Descendants(ns + "CountryISOCodeResult").FirstOrDefault()?.Value;

                var result = new
                {
                    isocode = isoCode
                };

                string jsonResponse = JsonSerializer.Serialize(result);

                return jsonResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting ISO code.");
                throw;
            }
        }
    }
}
