using ClipTwitch.Data.Models;

namespace ClipTwitch.Service.Interfaces
{
    public interface ITwitchFunctions
    {
        public Task<BaseListItems<Streamer>> GetStreamers(string streamerNickname);
        public Task<BaseListItems<Game>> GetGames(string name);
        public Task<BaseListItems<Clip>> GetClips(string streamerId = null, string gameId = null, string clipId = null);
    }
}
