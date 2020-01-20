
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TeamsApi.Models;

namespace TeamsApi.Repository
{
    public class AuthProvider : IAuthProvider
    {
        private readonly string _tenant;
        private readonly string _clientID;
        private readonly string _clientSecret;
        private readonly string _url;

        public AuthProvider(IConfiguration configuration)
        {
            _tenant = configuration["TenantDomain"];
            _clientID = configuration["ClientID"];
            _clientSecret = configuration["ClientSecret"];
            _url = configuration["AccessTokenUrl"]+_tenant+configuration["AuthUrl"];
        }
        public async Task<string> GetAccessTokenAsync()
        {
            var accessToken = string.Empty;

            var body = $"grant_type=client_credentials&client_id={_clientID}@{_tenant}&client_secret={_clientSecret}&scope=https://graph.microsoft.com/.default";
            try
            {

                HttpClient httpClient = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, _url);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Content = (new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded"));
                HttpResponseMessage response = await httpClient.SendAsync(request);
                string responseBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw new Exception(responseBody);

                accessToken = JsonConvert.DeserializeObject<TokenResponse>(responseBody).access_token;
                return accessToken;
            }
            catch (Exception)
            {
                //TBD
                throw;
            }
        }
    }
}