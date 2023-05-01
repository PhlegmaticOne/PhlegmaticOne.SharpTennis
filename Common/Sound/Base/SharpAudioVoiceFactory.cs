namespace PhlegmaticOne.SharpTennis.Game.Common.Sound.Base
{
    public class SharpAudioVoiceFactory
    {
        private readonly SharpAudioDevice _sharpAudioDevice;

        public SharpAudioVoiceFactory(SharpAudioDevice sharpAudioDevice) => _sharpAudioDevice = sharpAudioDevice;

        public SharpAudioVoice CreateVoice(string fileName) => new SharpAudioVoice(_sharpAudioDevice, fileName);
    }
}
