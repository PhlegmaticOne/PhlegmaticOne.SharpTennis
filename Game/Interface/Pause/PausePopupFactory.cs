using PhlegmaticOne.SharpTennis.Game.Engine2D;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Popups;

namespace PhlegmaticOne.SharpTennis.Game.Game.Interface.Pause
{
    public class PausePopupFactory : PopupFactory<PausePopup>
    {
        public PausePopupFactory(PausePopup popup) : base(popup) { }

        public override Canvas SetupPopup(PausePopup popup)
        {
            throw new System.NotImplementedException();
        }
    }
}
