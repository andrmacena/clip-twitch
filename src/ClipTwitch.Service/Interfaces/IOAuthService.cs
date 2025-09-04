using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClipTwitch.Service.Interfaces
{
    public interface IOAuthService
    {
        public Task<string> GetAccessToken();
        public Task<string> GetAccessTokenWithScope(string authCode);
        public Task<string> GetAuthorizationCode();

    }
}
