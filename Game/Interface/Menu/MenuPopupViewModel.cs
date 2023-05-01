using PhlegmaticOne.SharpTennis.Game.Common.Commands;

namespace PhlegmaticOne.SharpTennis.Game.Game.Interface.Menu
{
    public class MenuPopupViewModel
    {
        public MenuPopupViewModel(ICommand playButtonCommand, ICommand exitButtonCommand, ICommand settingsClickCommand)
        {
            PlayButtonCommand = playButtonCommand;
            ExitButtonCommand = exitButtonCommand;
            SettingsClickCommand = settingsClickCommand;
        }

        public ICommand PlayButtonCommand { get; }
        public ICommand ExitButtonCommand { get; }
        public ICommand SettingsClickCommand { get; }
    }
}
