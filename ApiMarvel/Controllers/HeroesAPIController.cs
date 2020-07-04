using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ApiMarvel.Controllers
{
    [Route("api/HeroesAPI")]
    [ApiController]
    public class HeroesAPIController : ControllerBase 
    {
        private readonly IConfiguration _configuration;

        public HeroesAPIController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        // GET api/values
        [HttpGet]
        [Route("GetAllCharacters")]
        public async Task<dynamic> GetAllCharacters()
        {
            try
            {
                dynamic result = null;
                string ts = DateTime.Now.Ticks.ToString();
                string privateKey = _configuration["ApiMarvel:PrivateKey"];
                string publicKey = _configuration["ApiMarvel:PublicKey"];
                string url = _configuration["ApiMarvel:Url"];
                string hash = GenerateHash(ts, publicKey, privateKey);

                string endpoint = $"{url}/v1/public/characters?ts={ts}&apikey={publicKey}&hash={hash}&limit={100}";

                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(endpoint))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();

                        result = JsonConvert.DeserializeObject(apiResponse);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return JsonConvert.DeserializeObject(ex.Message);
            }
        }

        [HttpPost]
        [Route("GetCharacterByName")]
        public async Task<dynamic> GetCharacterByName(string name)
        {
            try
            {
                dynamic result = null;
                string ts = DateTime.Now.Ticks.ToString();
                string privateKey = _configuration["ApiMarvel:PrivateKey"];
                string publicKey = _configuration["ApiMarvel:PublicKey"];
                string url = _configuration["ApiMarvel:Url"];
                string hash = GenerateHash(ts, publicKey, privateKey);

                string endpoint = $"{url}/v1/public/characters?ts={ts}&apikey={publicKey}&hash={hash}&" +
                                  $"name={Uri.EscapeUriString(name)}";

                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(endpoint))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();

                        result = JsonConvert.DeserializeObject(apiResponse);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return JsonConvert.DeserializeObject(ex.Message);
            }
        }

        private string GenerateHash(string ts, string publicKey, string privateKey)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(ts + privateKey + publicKey);
            var generator = MD5.Create();
            byte[] bytesHash = generator.ComputeHash(bytes);

            return BitConverter.ToString(bytesHash).ToLower().Replace("-", String.Empty);
        }
    }
}
