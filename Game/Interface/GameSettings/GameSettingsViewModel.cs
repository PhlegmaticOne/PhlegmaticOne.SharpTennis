using PhlegmaticOne.SharpTennis.Game.Common.Commands;

namespace PhlegmaticOne.SharpTennis.Game.Game.Interface.GameSettings
{
    public class GameSettingsViewModel
    {
        public GameSettingsViewModel(ICommand closeCommand, ICommand startGameCommand)
        {
            CloseCommand = closeCommand;
            StartGameCommand = startGameCommand;
        }

        public ICommand StartGameCommand { get; }
        public ICommand CloseCommand { get; }
    }
}
