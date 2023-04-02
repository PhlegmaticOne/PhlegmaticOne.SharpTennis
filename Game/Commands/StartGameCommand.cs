using PhlegmaticOne.SharpTennis.Game.Common.Commands;
using PhlegmaticOne.SharpTennis.Game.Engine2D;

namespace PhlegmaticOne.SharpTennis.Game.Game.Commands
{
    internal class StartGameCommand : ICommand
    {
        private readonly CanvasManager _canvasManager;

        public StartGameCommand(CanvasManager canvasManager) => _canvasManager = canvasManager;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            _canvasManager.RemoveLast(true);
        }
    }
}
