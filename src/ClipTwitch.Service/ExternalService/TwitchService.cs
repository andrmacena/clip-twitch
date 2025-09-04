using ClipTwitch.Data.Models;
using ClipTwitch.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ClipTwitch.Service.ExternalService
{
    public class TwitchService : IOAuthService, ITwitchFunctions
    {
        private string ClientId { get; set; }
        private string ClientSecret { get; set; }
        private string AuthenticationUrl { get; set; }
        private string AuthorizationUrl { get; set; }

        private IConfiguration _configuration;
        public TwitchService(IConfiguration configuration)
        {
            _configuration = configuration;
            ClientId = _configuration["Credentials:ClientId"];
            ClientSecret = _configuration["Credentials:ClientSecret"];
            AuthenticationUrl = _configuration["TwitchRoutes:AuthorizeScopes"];
        }

        public async Task<string> GetAccessToken()
        {
            var request = new HttpClient();

            var requestBody = new Dictionary<string, string>
                            {
                                { "client_id", ClientId },
                                { "client_secret", ClientSecret },
                                { "grant_type", "client_credentials" }
                            };
            var content = new FormUrlEncodedContent(requestBody);

            var result = await request.PostAsync($"https://id.twitch.tv/oauth2/token", content);

            var token = result.Content.ReadFromJsonAsync<Token>();


            return token.Result.access_token;
        }

        public async Task<string> GetAuthorizationCode()
        {
            var request = new HttpClient();

            var response = await request.GetAsync($"https://id.twitch.tv/oauth2/authorize?response_type=code&client_id={ClientId}&redirect_uri=https://localhost:7143/ClipTwitch/callback&scope=clips%3Aedit");

            return response.RequestMessage.RequestUri.AbsoluteUri;
        }

        public async Task<string> GetAccessTokenWithScope(string authCode)
        {
            var request = new HttpClient();

            var response = await request.PostAsync($"https://id.twitch.tv/oauth2/token?client_id={ClientId}&client_secret={ClientSecret}&code={authCode}&grant_type=authorization_code&redirect_uri=https://localhost:7143/ClipTwitch/callback", null);
            Console.WriteLine(response.Content);

            return response.Content.ReadAsStringAsync().Result;

        }

        public async Task<BaseListItems<Streamer>> GetStreamers(string streamerNickname)
        {
            try
            {

                var request = new HttpClient();

                request.DefaultRequestHeaders.Add("Client-ID", ClientId);
                request.DefaultRequestHeaders.Add("Authorization", "Bearer " + await GetAccessToken());

                var response = await request.GetAsync($"https://api.twitch.tv/helix/users?login={streamerNickname}");

                var streamersResponse = await response.Content.ReadFromJsonAsync<BaseListItems<Streamer>>();

                if (streamersResponse == null)
                {
                    streamersResponse = new BaseListItems<Streamer> { data = [] };
                }

                return streamersResponse;
            }
            catch (HttpRequestException reqEx)
            {

                throw;
            }

        }
    }
}
