using PhlegmaticOne.SharpTennis.Game.Common.Sound.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Popups;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Base;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Game;

namespace PhlegmaticOne.SharpTennis.Game.Game.Interface.Settings
{
    public class SettingsPopup : GamePopupBase
    {
        public SettingsPopup(ISoundManager<GameSounds> soundManager) : base(soundManager)
        {
        }
    }
}
