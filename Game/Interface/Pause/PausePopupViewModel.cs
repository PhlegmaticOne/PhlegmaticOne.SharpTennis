using PhlegmaticOne.SharpTennis.Game.Common.Commands;

namespace PhlegmaticOne.SharpTennis.Game.Game.Interface.Pause
{
    public class PausePopupViewModel 
    {
        public ICommand OnShowCommand { get; set; }
        public ICommand OnContinueCommand { get; set; }
        public ICommand OnExitCommand { get; set; }
        public ICommand OnSettingsCommand { get; set; }
    }
}
