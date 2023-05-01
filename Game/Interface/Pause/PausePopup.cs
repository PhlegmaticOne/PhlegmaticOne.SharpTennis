using PhlegmaticOne.SharpTennis.Game.Common.Sound.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Popups;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Base;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Game;

namespace PhlegmaticOne.SharpTennis.Game.Game.Interface.Pause
{
    public class PausePopup : GamePopupBase
    {
        public PausePopup(ISoundManager<GameSounds> soundManager) : base(soundManager)
        {
        }
    }
}
