using System.IO;
using Newtonsoft.Json;

namespace PhlegmaticOne.SharpTennis.Game.Common.Sound.Models.Data
{
    public class JsonSoundSettingsProvider : ISoundSettingsProvider
    {
        private const string FilePath = @"Assets\Data\sound_settings.json";

        public JsonSoundSettingsProvider() => Settings = LoadSettings();

        public SoundSettings Settings { get; }
        public void ForceSave()
        {
            var json = JsonConvert.SerializeObject(Settings);
            File.WriteAllText(FilePath, json);
        }

        private SoundSettings LoadSettings()
        {
            if (File.Exists(FilePath) == false)
            {
                return new SoundSettings
                {
                    IsMuted = false,
                    Volume = 50
                };
            }
            var json = File.ReadAllText(FilePath);
            return JsonConvert.DeserializeObject<SoundSettings>(json);
        }
    }
}
