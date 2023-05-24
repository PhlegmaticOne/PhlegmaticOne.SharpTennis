using System.Linq;
using PhlegmaticOne.SharpTennis.Game.Common.Commands;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Popups;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.Pause;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Game.Base;

namespace PhlegmaticOne.SharpTennis.Game.Game.Commands.Global
{
    public class GameEscapeButtonHandler : ICommand
    {
        private readonly PopupSystem _popupSystem;
        private readonly IGamePauseFacade _gamePauseFacade;

        public GameEscapeButtonHandler(PopupSystem popupSystem, IGamePauseFacade gamePauseFacade)
        {
            _popupSystem = popupSystem;
            _gamePauseFacade = gamePauseFacade;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            if (_popupSystem.Popups.Any(x => x is PausePopup))
            {
                _gamePauseFacade.Continue();
                _popupSystem.CloseLastPopup(true);
            }
            else
            {
                _gamePauseFacade.Pause();
                _popupSystem.SpawnPopup<PausePopup>();
            }
        }
    }
}
