using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ClipTwitch.Data.Models
{
    public class Game
    {
        public int id { get; set; }
        public string name { get; set; }
        [JsonPropertyName("box_art_url")]
        public string url_image { get; set; }
        public int igdb_id { get; set; }
    }
}
