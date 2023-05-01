using PhlegmaticOne.SharpTennis.Game.Engine2D;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Popups;

namespace PhlegmaticOne.SharpTennis.Game.Game.Interface.Settings
{
    public class SettingsPopupFactory : PopupFactory<SettingsPopup>
    {
        public SettingsPopupFactory(SettingsPopup popup) : base(popup) { }

        public override Canvas SetupPopup(SettingsPopup popup)
        {
            throw new System.NotImplementedException();
        }
    }
}
