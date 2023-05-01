using PhlegmaticOne.SharpTennis.Game.Common.Commands;
using PhlegmaticOne.SharpTennis.Game.Game.Models.Game.Base;

namespace PhlegmaticOne.SharpTennis.Game.Game.Commands
{
    public class PauseGameCommand : ICommand
    {
        private readonly IGamePauseFacade _gameFacade;
        public PauseGameCommand(IGamePauseFacade gameFacade) => _gameFacade = gameFacade;
        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) => _gameFacade.Pause();
    }
}
