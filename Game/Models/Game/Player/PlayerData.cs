using Newtonsoft.Json;

namespace PhlegmaticOne.SharpTennis.Game.Game.Models.Game.Player
{
    public class PlayerData
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
