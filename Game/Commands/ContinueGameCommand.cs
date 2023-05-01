using PhlegmaticOne.SharpTennis.Game.Common.Commands;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Popups;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Game.Base;

namespace PhlegmaticOne.SharpTennis.Game.Game.Commands
{
    public class ContinueGameCommand : ICommand
    {
        private readonly IGamePauseFacade _gameFacade;
        private readonly PopupSystem _popupSystem;

        public ContinueGameCommand(IGamePauseFacade gameFacade, PopupSystem popupSystem)
        {
            _gameFacade = gameFacade;
            _popupSystem = popupSystem;
        }

        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter)
        {
            _popupSystem.CloseLastPopup(true);
            _gameFacade.Continue();
        }
    }
}
