using ClipTwitch.Service;
using ClipTwitch.Service.Interfaces;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Primitives;
using System.Configuration;

namespace ClipTwitch.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClipTwitchController : ControllerBase
    {
        private readonly IOAuthService _authService;
        private readonly ITwitchFunctions _twitchFunctions;
        public ClipTwitchController(IOAuthService authService, ITwitchFunctions twitchFunctions)
        {
            _authService = authService;
            _twitchFunctions = twitchFunctions;
        }

        [HttpGet]
        [Route("login")]
        public async Task<IActionResult> OpenTwitchLogin()
        {
            var redirectUrl = await _authService.GetAuthorizationCode();

            return Ok(redirectUrl);
        }

        [HttpGet]
        [Route("callback")]
        public async Task<IActionResult> CallbackWithToken()
        {
            StringValues code;

            if (this.Request.Query.TryGetValue("code", out code))
            {
                Console.WriteLine(code);
            }
            else
            {
                Console.WriteLine("Code not found in query parameters.");
            }

            var token = await _authService.GetAccessTokenWithScope(code);

            return Ok(token);
        }

        [HttpGet]
        [Route("streamers/{nickname}")]
        public async Task<IActionResult> GetStreamers([FromRoute] string nickname)
        {
            var resultado = await _twitchFunctions.GetStreamers(nickname);

            return Ok(resultado);
        }

    }
}
