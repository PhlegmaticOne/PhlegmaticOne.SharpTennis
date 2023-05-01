using PhlegmaticOne.SharpTennis.Game.Common.Commands;

namespace PhlegmaticOne.SharpTennis.Game.Game.Interface.Pause
{
    public class PausePopupViewModel 
    {
        public PausePopupViewModel(ICommand onShowCommand, ICommand onContinueCommand, 
            ICommand onExitCommand, ICommand onSettingsCommand, ICommand restartCommand)
        {
            OnShowCommand = onShowCommand;
            OnContinueCommand = onContinueCommand;
            OnExitCommand = onExitCommand;
            OnSettingsCommand = onSettingsCommand;
            RestartCommand = restartCommand;
        }

        public ICommand RestartCommand { get; }
        public ICommand OnShowCommand { get; }
        public ICommand OnContinueCommand { get; }
        public ICommand OnExitCommand { get; }
        public ICommand OnSettingsCommand { get; }
    }
}
