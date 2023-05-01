using PhlegmaticOne.SharpTennis.Game.Common.Commands;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Popups;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.GameSettings;

namespace PhlegmaticOne.SharpTennis.Game.Game.Commands
{
    public class ShowGameSettingsPopupCommand : ICommand
    {
        private readonly PopupSystem _popupSystem;
        public ShowGameSettingsPopupCommand(PopupSystem popupSystem) => _popupSystem = popupSystem;
        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) => _popupSystem.SpawnPopup<GameSettingsPopup>();
    }
}
