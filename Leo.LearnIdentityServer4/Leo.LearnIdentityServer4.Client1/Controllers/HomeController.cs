using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Leo.LearnIdentityServer4.Client1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            // discover endpoints from metadata
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5011");
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            //_logger.LogDebug("client.GetDiscoveryDocumentAsync=>{0}", JsonConvert.SerializeObject(disco, settings));
            if (disco.IsError)
            {
                return Json(disco.Error);
            }
            var AuthorizationCodeTokenRequest = new AuthorizationCodeTokenRequest
            {
                Address = disco.AuthorizeEndpoint,
                ClientId = "mvc_client",
                ClientSecret = "secret",
                RedirectUri = "https://localhost:8001/signin-oidc"
            };
            var temp = await client.RequestAuthorizationCodeTokenAsync(AuthorizationCodeTokenRequest);

            var ClientCredentialsTokenRequest = new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = "client",
                ClientSecret = "secret",
                Scope = "api1"
            };
            // request token
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(ClientCredentialsTokenRequest);

            //_logger.LogDebug("client.RequestClientCredentialsTokenAsync=>{0}", JsonConvert.SerializeObject(tokenResponse));
            if (tokenResponse.IsError)
            {
                return Json(tokenResponse.Error);
            }
            //return Json(tokenResponse.Json);
            // call api
            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await apiClient.GetAsync("https://localhost:6001/api/identity");
            if (!response.IsSuccessStatusCode)
            {
                return Json(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                return Json(content);
            }
        }
    }
}
