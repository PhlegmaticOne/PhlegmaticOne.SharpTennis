using PhlegmaticOne.SharpTennis.Game.Common.Sound.Base;
using System.Collections.Generic;
using System;

namespace PhlegmaticOne.SharpTennis.Game.Common.Sound
{
    public class SoundManager<T> : ISoundManager<T> where T : Enum
    {
        private readonly IDictionary<T, SharpAudioVoice> _sounds;

        public SoundManager(IDictionary<T, SharpAudioVoice> sounds) => _sounds = sounds;

        public void Play(T sound)
        {
            if (_sounds.TryGetValue(sound, out var audioVoice))
            {
                audioVoice.Play();
            }
        }
    }
}
