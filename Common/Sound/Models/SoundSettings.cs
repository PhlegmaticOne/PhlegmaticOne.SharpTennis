using Newtonsoft.Json;

namespace PhlegmaticOne.SharpTennis.Game.Common.Sound.Models
{
    public class SoundSettings
    {
        [JsonProperty("volume")]
        public float Volume;

        [JsonProperty("isMuted")]
        public bool IsMuted;

        [JsonIgnore]
        public float NormalizedVolume => Volume / 100f;
    }
}
