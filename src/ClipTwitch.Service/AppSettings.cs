using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace ClipTwitch.Service
{
    public class AppSettings
    {
        public Credentials? credentials { get; set; }
        public TwitchRoutes? twitchRoutes { get; set; }

    }

    public class Credentials
    {
        public string? ClientId { get; set; }
        public string? ClientSecret { get; set; }
    }

    public class TwitchRoutes
    {
        public string? GetStreams { get; set; }
        public string? GetUsers { get; set; }
        public string? GetAuthorizationCode { get; set; }
        public string? GetAccessToken { get; set; }
        public string? GetClips { get; set; }
        public string? GetGames { get; set; }

    }
}
