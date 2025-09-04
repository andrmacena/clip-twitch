using ClipTwitch.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClipTwitch.Service.Interfaces
{
    public interface ITwitchFunctions
    {
        public Task<BaseListItems<Streamer>> GetStreamers(string streamerNickname);
    }
}
