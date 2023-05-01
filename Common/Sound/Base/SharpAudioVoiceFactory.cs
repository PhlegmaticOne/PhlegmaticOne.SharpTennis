using PhlegmaticOne.SharpTennis.Game.Common.Sound.Models.Data;

namespace PhlegmaticOne.SharpTennis.Game.Common.Sound.Base
{
    public class SharpAudioVoiceFactory
    {
        private readonly SharpAudioDevice _sharpAudioDevice;
        private readonly ISoundSettingsProvider _soundSettingsProvider;

        public SharpAudioVoiceFactory(SharpAudioDevice sharpAudioDevice, ISoundSettingsProvider soundSettingsProvider)
        {
            _sharpAudioDevice = sharpAudioDevice;
            _soundSettingsProvider = soundSettingsProvider;
        }

        public SharpAudioVoice CreateVoice(string fileName) => 
            new SharpAudioVoice(_sharpAudioDevice, _soundSettingsProvider, fileName);
    }
}
