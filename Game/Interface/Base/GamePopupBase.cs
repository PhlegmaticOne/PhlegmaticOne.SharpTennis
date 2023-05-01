using PhlegmaticOne.SharpTennis.Game.Common.Sound.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Popups;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Game;

namespace PhlegmaticOne.SharpTennis.Game.Game.Interface.Base
{
    public abstract class GamePopupBase : PopupBase
    {
        protected readonly ISoundManager<GameSounds> SoundManager;

        protected GamePopupBase(ISoundManager<GameSounds> soundManager)
        {
            SoundManager = soundManager;
        }

        protected override void OnShow()
        {
            SoundManager.Play(GameSounds.PopupIn);
        }

        protected override void OnClose()
        {
            SoundManager.Play(GameSounds.PopupOut);
        }
    }
}
