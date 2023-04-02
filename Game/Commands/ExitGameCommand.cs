using PhlegmaticOne.SharpTennis.Game.Common.Commands;
using SharpDX.Windows;

namespace PhlegmaticOne.SharpTennis.Game.Game.Commands
{
    public class ExitGameCommand : ICommand
    {
        private readonly RenderForm _renderForm;

        public ExitGameCommand(RenderForm renderForm)
        {
            _renderForm = renderForm;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            _renderForm.Close();
        }
    }
}
