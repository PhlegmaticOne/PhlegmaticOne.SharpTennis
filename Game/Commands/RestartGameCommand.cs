using PhlegmaticOne.SharpTennis.Game.Common.Commands;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Popups;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Game.Base;

namespace PhlegmaticOne.SharpTennis.Game.Game.Commands
{
    public class RestartGameCommand : ICommand
    {
        private readonly PopupSystem _popupSystem;
        private readonly IGameRestartFacade _gameRestartFacade;
        private readonly IGamePauseFacade _gamePauseFacade;

        public RestartGameCommand(PopupSystem popupSystem, IGameRestartFacade gameRestartFacade, IGamePauseFacade gamePauseFacade)
        {
            _popupSystem = popupSystem;
            _gameRestartFacade = gameRestartFacade;
            _gamePauseFacade = gamePauseFacade;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            _popupSystem.CloseLastPopup(true);
            _gameRestartFacade.Restart();
            _gamePauseFacade.Continue();
        }
    }
}
