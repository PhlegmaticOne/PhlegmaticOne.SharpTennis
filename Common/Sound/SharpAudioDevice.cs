using System;
using SharpDX.XAudio2;

namespace PhlegmaticOne.SharpTennis.Game.Common.Sound
{
    public class SharpAudioDevice : IDisposable
    {
        public XAudio2 Device { get; }
        public MasteringVoice Master { get; set; }

        public SharpAudioDevice()
        {
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
