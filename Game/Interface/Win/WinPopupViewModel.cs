using PhlegmaticOne.SharpTennis.Game.Common.Commands;

namespace PhlegmaticOne.SharpTennis.Game.Game.Interface.Win
{
    public class WinPopupViewModel
    {
        public WinPopupViewModel(ICommand continueGameCommand, ICommand exitGameCommand)
        {
            ContinueGameCommand = continueGameCommand;
            ExitGameCommand = exitGameCommand;
        }

        public ICommand ContinueGameCommand { get; }
        public ICommand ExitGameCommand { get; }
    }
}
