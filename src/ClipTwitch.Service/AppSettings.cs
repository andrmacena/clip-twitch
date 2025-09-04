using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace ClipTwitch.Service
{
    public class AppSettings
    {
        public static Credentials? credentials { get; set; }
        public static TwitchRoutes? twitchRoutes { get; set; }

    }

    public class Credentials
    {
        public string? ClientId { get; set; }
        public string? ClientSecret { get; set; }
    }

    public class TwitchRoutes
    {
        public string? GetStreams{ get; set; }
        public string? GetUsers { get; set; }
        public string? AuthorizeScopes { get; set; }
        public string? GetAccessToken { get; set; }

    }
}
