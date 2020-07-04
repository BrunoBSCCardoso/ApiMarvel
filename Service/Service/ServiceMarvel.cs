using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Service.Interface;

namespace Service
{
    public class ServiceMarvel : IServiceMarvel
    {
        private readonly IConfiguration _configuration;

        public ServiceMarvel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

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
                                  $"nameStartsWith={Uri.EscapeUriString(name)}&limit={100}";

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
