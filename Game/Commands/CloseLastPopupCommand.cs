using PhlegmaticOne.SharpTennis.Game.Common.Commands;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Popups;

namespace PhlegmaticOne.SharpTennis.Game.Game.Commands
{
    public class CloseLastPopupCommand : ICommand
    {
        private readonly PopupSystem _popupSystem;
        public CloseLastPopupCommand(PopupSystem popupSystem) => _popupSystem = popupSystem;
        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) => _popupSystem.CloseLastPopup();
    }
}
