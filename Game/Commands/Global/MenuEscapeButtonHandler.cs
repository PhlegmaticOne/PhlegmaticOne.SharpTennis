using PhlegmaticOne.SharpTennis.Game.Common.Commands;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Popups;

namespace PhlegmaticOne.SharpTennis.Game.Game.Commands.Global
{
    public class MenuEscapeButtonHandler : ICommand
    {
        private readonly PopupSystem _popupSystem;
        private readonly ExitGameCommand _exitGameCommand;

        public MenuEscapeButtonHandler(PopupSystem popupSystem, ExitGameCommand exitGameCommand)
        {
            _popupSystem = popupSystem;
            _exitGameCommand = exitGameCommand;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            if (_popupSystem.Popups.Count > 1)
            {
                _popupSystem.CloseLastPopup();
                return;
            }

            _exitGameCommand.Execute(parameter);
        }
    }
}
