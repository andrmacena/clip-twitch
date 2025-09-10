using ClipTwitch.Data.Models;

namespace ClipTwitch.Service.Interfaces
{
    public interface ITwitchFunctions
    {
        public Task<BaseListItems<Streamer>> GetStreamers(string streamerNickname);
        public Task<BaseListItems<Game>> GetGames(string name);
    }
}
