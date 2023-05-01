using System;
using PhlegmaticOne.SharpTennis.Game.Common.Sound.Models.Data;
using SharpDX.XAudio2;

namespace PhlegmaticOne.SharpTennis.Game.Common.Sound
{
    public class SharpAudioDevice : IDisposable
    {
        public ISoundSettingsProvider SoundSettingsProvider { get; }
        public XAudio2 Device { get; }
        public MasteringVoice Master { get; set; }

        public SharpAudioDevice(ISoundSettingsProvider soundSettingsProvider)
        {
            SoundSettingsProvider = soundSettingsProvider;
            Device = new XAudio2();
            Master = new MasteringVoice(Device);
        }

        public void Dispose()
        {
            Master.Dispose();
            Device.Dispose();
        }
    }
}
