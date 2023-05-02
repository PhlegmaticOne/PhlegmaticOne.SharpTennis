using PhlegmaticOne.SharpTennis.Game.Common.Commands;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Popups;
using PhlegmaticOne.SharpTennis.Game.Game.Interface.GameSettings;

namespace PhlegmaticOne.SharpTennis.Game.Game.Commands
{
    public class ContinueGameAfterWinCommand : ICommand
    {
        private readonly RestartGameCommand _restartGameCommand;
        private readonly PopupSystem _popupSystem;

        public ContinueGameAfterWinCommand(RestartGameCommand restartGameCommand, PopupSystem popupSystem)
        {
            _restartGameCommand = restartGameCommand;
            _popupSystem = popupSystem;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            var gameInfoPopup = _popupSystem.SpawnPopup<GameSettingsPopup>();
            gameInfoPopup.SetupHeaderText("Wanna change game settings?");
            gameInfoPopup.SetupStartGameCommand(_restartGameCommand, true);
        }
    }
}
