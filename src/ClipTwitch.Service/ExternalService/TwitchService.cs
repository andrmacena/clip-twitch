using ClipTwitch.Data.Models;
using ClipTwitch.Service.Interfaces;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace ClipTwitch.Service.ExternalService
{
    public class TwitchService : IOAuthService, ITwitchFunctions
    {
        private string? ClientId { get; set; }
        private string? ClientSecret { get; set; }
        private string? UrlAuthorizeScopes { get; set; }
        private string? AuthorizationUrl { get; set; }
        private string? UrlUsers { get; set; }
        private string? UrlAccessToken { get; set; }
        private string? UrlClips { get; set; }
        private string? UrlGames { get; set; }

        private readonly AppSettings _configuration;

        const string URL_CALLBACK_APP = "https://cliptwitch-fxcsgtdte5a6ckca.brazilsouth-01.azurewebsites.net/ClipTwitch/callback";
        public TwitchService(IOptions<AppSettings> configuration)
        {
            _configuration = configuration.Value;
            ClientId = _configuration.credentials?.ClientId;
            ClientSecret = _configuration.credentials?.ClientSecret;
            UrlAuthorizeScopes = _configuration.twitchRoutes?.GetAuthorizationCode;
            UrlUsers = _configuration.twitchRoutes?.GetUsers;
            UrlAccessToken = _configuration.twitchRoutes?.GetAccessToken;
            UrlClips = _configuration.twitchRoutes?.GetClips;
            UrlGames = _configuration.twitchRoutes?.GetGames;
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

            var result = await request.PostAsync(UrlAccessToken, content);

            var token = result.Content.ReadFromJsonAsync<Token>();


            return token.Result.access_token;
        }

        public async Task<string> GetAuthorizationCode()
        {
            var request = new HttpClient();

            var response = await request.GetAsync($"{UrlAuthorizeScopes}?response_type=code&client_id={ClientId}&redirect_uri={URL_CALLBACK_APP}&scope=clips%3Aedit");

            return response.RequestMessage.RequestUri.AbsoluteUri;
        }

        public async Task<string> GetAccessTokenWithScope(string authCode)
        {
            var request = new HttpClient();

            var response = await request.PostAsync($"{UrlAccessToken}?client_id={ClientId}&client_secret={ClientSecret}&code={authCode}&grant_type=authorization_code&redirect_uri={URL_CALLBACK_APP}", null);
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

                var response = await request.GetAsync($"{UrlUsers}?login={streamerNickname}");

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

        public async Task<BaseListItems<Game>> GetGames(string name)
        {
            try
            {

                var request = new HttpClient();

                request.DefaultRequestHeaders.Add("Client-ID", ClientId);
                request.DefaultRequestHeaders.Add("Authorization", "Bearer " + await GetAccessToken());

                var response = await request.GetAsync($"{UrlGames}?name={name}");

                var gamesResponse = await response.Content.ReadFromJsonAsync<BaseListItems<Game>>();

                if (gamesResponse == null)
                {
                    gamesResponse = new BaseListItems<Game> { data = [] };
                }

                return gamesResponse;
            }
            catch (HttpRequestException reqEx)
            {

                throw;
            }
        }

        public async Task<BaseListItems<Clip>> GetClips(string streamerId = null, string gameId = null, string clipId = null)
        {
            try
            {

                var request = new HttpClient();

                request.DefaultRequestHeaders.Add("Client-ID", ClientId);
                request.DefaultRequestHeaders.Add("Authorization", "Bearer " + await GetAccessToken());
              
                var response = await request.GetAsync($"{UrlClips}?broadcaster_id={streamerId}");

                var clipsResponse = await response.Content.ReadFromJsonAsync<BaseListItems<Clip>>();

                if (clipsResponse == null)
                {
                     clipsResponse = new BaseListItems<Clip> { data = [] };
                }

                return clipsResponse;
            }
            catch (HttpRequestException reqEx)
            {

                throw;
            }
        }
    }
}
