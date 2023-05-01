using PhlegmaticOne.SharpTennis.Game.Common.Commands;

namespace PhlegmaticOne.SharpTennis.Game.Game.Interface.Settings
{
    public class SettingsPopupViewModel
    {
        public SettingsPopupViewModel(ICommand closeCommand, ICommand saveCommand)
        {
            CloseCommand = closeCommand;
            SaveCommand = saveCommand;
        }

        public ICommand CloseCommand { get; }
        public ICommand SaveCommand { get; }
    }
}
