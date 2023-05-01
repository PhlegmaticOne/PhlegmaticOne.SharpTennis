using System;

namespace PhlegmaticOne.SharpTennis.Game.Common.Sound.Base
{
    public interface ISoundManager<in T> where T : Enum
    {
        void Play(T sound);
    }
}
