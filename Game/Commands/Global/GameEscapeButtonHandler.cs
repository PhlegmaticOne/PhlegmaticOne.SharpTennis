using System.Linq;
using PhlegmaticOne.SharpTennis.Game.Common.Commands;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Popups;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Pause;

namespace PhlegmaticOne.SharpTennis.Game.Game.Commands.Global
{
    public class GameEscapeButtonHandler : ICommand
    {
        private readonly PopupSystem _popupSystem;

        public GameEscapeButtonHandler(PopupSystem popupSystem) => _popupSystem = popupSystem;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            if (_popupSystem.Popups.Any(x => x is PausePopup))
            {
                _popupSystem.CloseLastPopup(true);
            }
            else
            {
                _popupSystem.SpawnPopup<PausePopup>();
            }
        }
    }
}
